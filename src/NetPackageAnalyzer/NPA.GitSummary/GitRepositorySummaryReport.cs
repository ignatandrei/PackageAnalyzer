namespace NPA.GitSummary;

public sealed record GitFileFrequency(string FilePath, int Count);

public sealed record GitContributorFrequency(string Contributor, int CommitCount)
{
    public double ShareOfCommits(int totalCommits)
        => totalCommits == 0 ? 0 : (double)CommitCount / totalCommits;
}
public sealed record GitYearCommitFrequency(int Year,int CommitCount)
{
}
public sealed record GitMonthlyCommitFrequency(int Year, int Month, int CommitCount)
{
    public string YearMonth => $"Year {Year:D4}-Month {Month:D2}:";
}

public sealed record GitFirefightingCommit(DateTimeOffset CommitDate, string Sha, string Subject);

public sealed record GitChurnHotspotReport(string Since, IReadOnlyList<GitFileFrequency> Files);

public sealed record GitBusFactorReport(IReadOnlyList<GitContributorFrequency> Contributors)
{
    public int TotalCommits => Contributors.Sum(it => it.CommitCount);

    public GitContributorFrequency? TopContributor => Contributors.FirstOrDefault();

    public double? TopContributorRatio
        => TopContributor is null || TotalCommits == 0
            ? null
            : TopContributor.ShareOfCommits(TotalCommits);
}

public sealed record GitBugHotspotReport(string? Since, IReadOnlyList<GitFileFrequency> Files);

public sealed record GitCommitVelocityReport(IReadOnlyList<GitMonthlyCommitFrequency> Months)
{
    public bool HasMoreThanOneYear => Months.Select(it=>it.Year).Distinct().Count() > 1;

    public IReadOnlyList<GitYearCommitFrequency> Years=> Months
        .GroupBy(it => it.Year)
        .Select(g => new GitYearCommitFrequency(g.Key, g.Sum(it => it.CommitCount)))
        .ToList();
}

public sealed record GitFirefightingReport(string Since, IReadOnlyList<GitFirefightingCommit> Commits);

public sealed record GitRepositorySummaryReport(
    GitChurnHotspotReport ChurnHotspots,
    GitBusFactorReport BusFactor,
    GitBugHotspotReport BugHotspots,
    GitCommitVelocityReport CommitVelocity,
    GitFirefightingReport Firefighting);

public sealed record GitRepositorySummaryOptions
{
    public int TopCount { get; init; } = 100;

    public string ChurnSince { get; init; } = "2 years ago";

    public string? BugSince { get; init; } = "2 years ago";

    public string FirefightingSince { get; init; } = "2 years ago";
}
