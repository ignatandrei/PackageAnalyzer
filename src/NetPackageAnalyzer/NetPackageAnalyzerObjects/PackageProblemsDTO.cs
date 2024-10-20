namespace NetPackageAnalyzerObjects;

public class PackageProblemsDTO
{
    public PackageWithVersionDeprecated[] deprecated = [];
    public PackageWithVersionOutdated[] outdated = [];
    public PackageWithVersionVulnerable[] vulnerable = [];
    
    public NamePerCount[] NamePerCounts()
    {
        return new NamePerCount[]
        {
            new NamePerCount("Vulnerable",vulnerable.Length),
            new NamePerCount("Outdated",outdated.Length),
            new NamePerCount("Deprecated",deprecated.Length)
        };
    }
    public PackageWithVersion[] All()
    {

        return AllNotDistinct()
            .Distinct()
            .ToArray();

    }
    public PackageWithVersion[] AllNotDistinct()
    {

        return Vuln().Concat(Out())
            .Concat(Depr())            
            .ToArray();

    }
    public PackageWithVersion[] Vuln()
    {
        return vulnerable.ToArray();
    }
    public PackageWithVersion[] Out()
    {
        return outdated.ToArray();
    }
    public PackageWithVersion[] Depr()
    {
        return deprecated.ToArray();
    }
    public void VerifyWhy()
    {
        deprecated = deprecated.Distinct().ToArray();
        outdated = outdated.Distinct().ToArray();
        vulnerable = vulnerable.Distinct().ToArray();

        var notDistinct = AllNotDistinct();
        foreach (var item in All())
        {
            item.VerifyWhy();
            var more= notDistinct
                .Where(x => x.PackageId == item.PackageId)
                .Where(x => x!=item)
                .ToArray();
            if (more.Length == 0) continue;
            foreach (var itemMore in more)
            {
                itemMore.CopyWhyFrom(item);
            }
        }
    }
}
