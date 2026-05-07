using System.Diagnostics;
using System.Globalization;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using NPA.ProcessRunner;

namespace NPA.GitSummary;

public sealed partial class GitRepositorySummaryAnalyzer
{
    private readonly string gitExecutable;
    private readonly IFileSystem fileSystem;
    private readonly IProcessRunner processRunner;

    public GitRepositorySummaryAnalyzer(string gitExecutable = "git", IProcessRunner? processRunner = null, IFileSystem? fileSystem = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(gitExecutable);
        this.gitExecutable = gitExecutable;
        this.processRunner = processRunner ?? new SystemProcessRunner();
        this.fileSystem = fileSystem ?? new FileSystem();
    }

    public async Task<GitRepositorySummaryReport> AnalyzeAsync(
        string repositoryPath,
        GitRepositorySummaryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(repositoryPath);

        if (!fileSystem.Directory.Exists(repositoryPath))
        {
            throw new DirectoryNotFoundException($"Repository path '{repositoryPath}' does not exist.");
        }

        options ??= new();

        var churnTask = ExecuteGitLinesAsync(repositoryPath, BuildChurnArguments(options), cancellationToken);
        var busFactorTask = ExecuteGitLinesAsync(repositoryPath, BuildBusFactorArguments(), cancellationToken);
        var bugTask = ExecuteGitLinesAsync(repositoryPath, BuildBugArguments(options), cancellationToken);
        var velocityTask = ExecuteGitLinesAsync(repositoryPath, BuildVelocityArguments(), cancellationToken);
        var firefightingTask = ExecuteGitLinesAsync(repositoryPath, BuildFirefightingArguments(options), cancellationToken);

        await Task.WhenAll(churnTask, busFactorTask, bugTask, velocityTask, firefightingTask);

        return new GitRepositorySummaryReport(
            CreateChurnHotspotReport(await churnTask, options.TopCount, options.ChurnSince),
            CreateBusFactorReport(await busFactorTask, options.TopCount),
            CreateBugHotspotReport(await bugTask, options.TopCount, options.BugSince),
            CreateCommitVelocityReport(await velocityTask),
            CreateFirefightingReport(await firefightingTask, options.FirefightingSince));
    }

    public static GitChurnHotspotReport CreateChurnHotspotReport(
        IEnumerable<string> fileLines,
        int topCount = 20,
        string since = "1 year ago")
        => new(since, BuildFileFrequency(fileLines, topCount));

    public static GitBusFactorReport CreateBusFactorReport(
        IEnumerable<string> contributorLines,
        int topCount = 20)
        => new(BuildContributorFrequency(contributorLines, topCount));

    public static GitBugHotspotReport CreateBugHotspotReport(
        IEnumerable<string> fileLines,
        int topCount = 20,
        string? since = null)
        => new(since, BuildFileFrequency(fileLines, topCount));

    public static GitCommitVelocityReport CreateCommitVelocityReport(IEnumerable<string> commitDateLines)
    {
        var months = commitDateLines
            .Select(static line => line.Trim())
            .Where(static line => !string.IsNullOrWhiteSpace(line))
            .Select(static line =>
            {
                if (!DateTimeOffset.TryParse(line, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var commitDate))
                {
                    throw new InvalidOperationException($"Unable to parse git commit date '{line}'.");
                }

                return commitDate;
            })
            .GroupBy(static date => new { date.Year, date.Month })
            .OrderBy(static group => group.Key.Year)
            .ThenBy(static group => group.Key.Month)
            .Select(static group => new GitMonthlyCommitFrequency(group.Key.Year, group.Key.Month, group.Count()))
            .ToArray();

        return new GitCommitVelocityReport(months);
    }

    public static GitFirefightingReport CreateFirefightingReport(
        IEnumerable<string> commitLines,
        string since = "1 year ago")
    {
        var commits = commitLines
            .Select(ParseFirefightingCommit)
            .Where(static commit => commit is not null)
            .Select(static commit => commit!)
            .ToArray();

        return new GitFirefightingReport(since, commits);
    }

    private async Task<IReadOnlyList<string>> ExecuteGitLinesAsync(
        string repositoryPath,
        IReadOnlyList<string> arguments,
        CancellationToken cancellationToken)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = gitExecutable,
            WorkingDirectory = repositoryPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        var result = await processRunner.RunAsync(startInfo, cancellationToken);

        if (result.ExitCode != 0)
        {
            var command = string.Join(" ", arguments);
            throw new InvalidOperationException(
                $"git {command} failed with exit code {result.ExitCode}:{Environment.NewLine}{result.StandardError}");
        }

        return result.StandardOutput
            .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .Select(static line => line.Trim())
            .Where(static line => !string.IsNullOrWhiteSpace(line))
            .ToArray();
    }

    private static IReadOnlyList<string> BuildChurnArguments(GitRepositorySummaryOptions options)
    {
        List<string> arguments = ["log", "--format=format:", "--name-only"];
        AppendSince(arguments, options.ChurnSince);
        return arguments;
    }

    private static IReadOnlyList<string> BuildBusFactorArguments()
        => ["log", "--format=%aN", "--no-merges"];

    private static IReadOnlyList<string> BuildBugArguments(GitRepositorySummaryOptions options)
    {
        List<string> arguments =
        [
            "log",
            "--format=format:",
            "--name-only",
            "--regexp-ignore-case",
            "--extended-regexp",
            "--grep=fix|bug|broken"
        ];

        AppendSince(arguments, options.BugSince);
        return arguments;
    }

    private static IReadOnlyList<string> BuildVelocityArguments()
        => ["log", "--format=%aI"];

    private static IReadOnlyList<string> BuildFirefightingArguments(GitRepositorySummaryOptions options)
    {
        List<string> arguments =
        [
            "log",
            "--format=%aI%x09%H%x09%s"
        ];

        AppendSince(arguments, options.FirefightingSince);
        return arguments;
    }

    private static void AppendSince(List<string> arguments, string? since)
    {
        if (string.IsNullOrWhiteSpace(since))
        {
            return;
        }

        arguments.Add($"--since={since}");
    }

    private static IReadOnlyList<GitFileFrequency> BuildFileFrequency(IEnumerable<string> fileLines, int topCount)
    {
        var take = NormalizeTopCount(topCount);

        return fileLines
            .Select(static line => line.Trim())
            .Where(static line => !string.IsNullOrWhiteSpace(line))
            .GroupBy(static line => line, StringComparer.Ordinal)
            .OrderByDescending(static group => group.Count())
            .ThenBy(static group => group.Key, StringComparer.Ordinal)
            .Take(take)
            .Select(static group => new GitFileFrequency(group.Key, group.Count()))
            .ToArray();
    }

    private static IReadOnlyList<GitContributorFrequency> BuildContributorFrequency(IEnumerable<string> contributorLines, int topCount)
    {
        var take = NormalizeTopCount(topCount);

        return contributorLines
            .Select(static line => line.Trim())
            .Where(static line => !string.IsNullOrWhiteSpace(line))
            .GroupBy(static line => line, StringComparer.Ordinal)
            .OrderByDescending(static group => group.Count())
            .ThenBy(static group => group.Key, StringComparer.Ordinal)
            .Take(take)
            .Select(static group => new GitContributorFrequency(group.Key, group.Count()))
            .ToArray();
    }

    private static int NormalizeTopCount(int topCount)
        => topCount <= 0 ? int.MaxValue : topCount;

    private static GitFirefightingCommit? ParseFirefightingCommit(string line)
    {
        var parts = line.Split('\t', 3, StringSplitOptions.None);
        if (parts.Length != 3)
        {
            throw new InvalidOperationException($"Unable to parse git firefighting output '{line}'.");
        }

        if (!DateTimeOffset.TryParse(parts[0], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var commitDate))
        {
            throw new InvalidOperationException($"Unable to parse git firefighting date '{parts[0]}'.");
        }

        var sha = parts[1].Trim();
        var subject = parts[2].Trim();

        if (string.IsNullOrWhiteSpace(sha) || string.IsNullOrWhiteSpace(subject))
        {
            throw new InvalidOperationException($"Unable to parse git firefighting output '{line}'.");
        }

        return FirefightingPattern().IsMatch(subject)
            ? new GitFirefightingCommit(commitDate, sha, subject)
            : null;
    }

    [GeneratedRegex(@"\b(revert|hotfix|emergency|rollback)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex FirefightingPattern();
}
