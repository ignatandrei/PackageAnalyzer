namespace NetPackageAnalyzerObjects;
public record PackageWithVersion(string PackageId,string RequestedVersion, PackageOptions PackageOptions = PackageOptions.None)
: PackageGatherInfo(PackageId)
{
    
    public ProjectData[] Projects { get; set; } = Array.Empty<ProjectData>();
    


}