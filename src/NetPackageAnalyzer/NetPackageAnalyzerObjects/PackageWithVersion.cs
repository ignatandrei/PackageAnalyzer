using NPA.ProcessRunner;

namespace NetPackageAnalyzerObjects;

public record PackageWithVersionOutdated(string packageId, string version, IProcessRunner processRunner)
    : PackageWithVersion(packageId, version, PackageOptions.Outdated, processRunner)
{
    
}
public record PackageWithVersionDeprecated(string packageId, string version, IProcessRunner processRunner)
    : PackageWithVersion(packageId, version,PackageOptions.Deprecated, processRunner)
{
    
}
public record PackageWithVersionVulnerable(string packageId, string version, IProcessRunner processRunner)
    : PackageWithVersion(packageId, version, PackageOptions.Vulnerable, processRunner)
{

}
