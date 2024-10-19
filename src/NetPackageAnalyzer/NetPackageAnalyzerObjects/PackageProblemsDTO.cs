namespace NetPackageAnalyzerObjects;

public class PackageProblemsDTO
{
    public PackageWithVersionDeprecated[] deprecated = [];
    public PackageWithVersionOutdated[] outdated = [];
    public PackageWithVersionVulnerable[] vulnerable = [];
    
    public PackageWithVersion[] All()
    {

        return Vuln().Concat(Out())
            .Concat(Depr())
            .Distinct()
            .ToArray();

    }
    public PackageWithVersion[] Vuln()
    {
        return vulnerable.Distinct().ToArray();
    }
    public PackageWithVersion[] Out()
    {
        return outdated.Distinct().ToArray();
    }
    public PackageWithVersion[] Depr()
    {
        return deprecated.Distinct().ToArray();
    }
    public void VerifyWhy()
    {
        foreach (var item in All())
        {
            item.VerifyWhy();
        }
    }
}
