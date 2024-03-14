namespace NetPackageAnalyzerObjects;

public record PackageWithVersionOutdated(string packageId, string version) 
    : PackageWithVersion(packageId, version, PackageOptions.Outdated)
{
    
}
public record PackageWithVersionDeprecated(string packageId, string version)
    : PackageWithVersion(packageId, version,PackageOptions.Deprecated)
{

}
