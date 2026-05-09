using NPA.ProcessRunner;

namespace NetPackageAnalyzerObjects;
public record PackageWithVersion(string PackageId,string RequestedVersion, PackageOptions PackageOptions = PackageOptions.None, IProcessRunner? processRunner = null)
: PackageGatherInfo(PackageId, processRunner ?? new SystemProcessRunner())
{
    
    public ProjectData[] Projects { get; set; } = Array.Empty<ProjectData>();
    


}