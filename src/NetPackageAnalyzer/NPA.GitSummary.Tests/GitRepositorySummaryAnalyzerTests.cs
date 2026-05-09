using NPA.ProcessRunner;

namespace NPA.GitSummary.Tests;

public class GitRepositorySummaryAnalyzerTests
{
    
    

    [Fact]
    public void CreateChurnHotspotReport_GroupsAndOrdersFiles()
    {
        var report = GitRepositorySummaryAnalyzer.CreateChurnHotspotReport(
            ["src/B.cs", "src/A.cs", "src/B.cs", "src/C.cs", "src/B.cs", "src/A.cs"],
            topCount: 2,
            since: "1 year ago");

        Assert.Equal("1 year ago", report.Since);
        Assert.Collection(
            report.Files,
            item =>
            {
                Assert.Equal("src/B.cs", item.FilePath);
                Assert.Equal(3, item.Count);
            },
            item =>
            {
                Assert.Equal("src/A.cs", item.FilePath);
                Assert.Equal(2, item.Count);
            });
    }

    [Fact]
    public void CreateBusFactorReport_ComputesTopContributorRatio()
    {
        var report = GitRepositorySummaryAnalyzer.CreateBusFactorReport(
            ["Alice", "Bob", "Alice", "Alice"],
            topCount: 10);

        Assert.Equal(4, report.TotalCommits);
        Assert.NotNull(report.TopContributor);
        Assert.Equal("Alice", report.TopContributor!.Contributor);
        Assert.Equal(3, report.TopContributor.CommitCount);
        Assert.Equal(0.75, report.TopContributorRatio);
    }

    [Fact]
    public void CreateCommitVelocityReport_GroupsCommitsByMonth()
    {
        var report = GitRepositorySummaryAnalyzer.CreateCommitVelocityReport(
            [
                "2026-04-03T08:30:00+00:00",
                "2026-04-01T09:00:00+00:00",
                "2026-03-28T10:00:00+00:00"
            ]);

        Assert.Collection(
            report.Months,
            item =>
            {
                Assert.Equal("2026-03", item.YearMonth);
                Assert.Equal(1, item.CommitCount);
            },
            item =>
            {
                Assert.Equal("2026-04", item.YearMonth);
                Assert.Equal(2, item.CommitCount);
            });
    }

    [Fact]
    public void CreateFirefightingReport_FiltersRelevantSubjects()
    {
        var report = GitRepositorySummaryAnalyzer.CreateFirefightingReport(
            [
                "2026-04-03T08:30:00+00:00\tabc123\tHotfix authentication flow",
                "2026-04-02T08:30:00+00:00\tdef456\tAdd diagnostics report",
                "2026-04-01T08:30:00+00:00\tghi789\tRollback failing deployment"
            ],
            since: "1 year ago");

        Assert.Equal("1 year ago", report.Since);
        Assert.Collection(
            report.Commits,
            item =>
            {
                Assert.Equal("abc123", item.Sha);
                Assert.Equal("Hotfix authentication flow", item.Subject);
            },
            item =>
            {
                Assert.Equal("ghi789", item.Sha);
                Assert.Equal("Rollback failing deployment", item.Subject);
            });
    }
}
