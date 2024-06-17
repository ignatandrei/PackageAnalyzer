namespace NetPackageAnalyzerObjects;

public interface IOperations
{
    void ClearWrongData();
    public string[] ProjectsPath();
    public string[] TopLevelPackagesIDs();
    Dictionary<string, PackageWithVersion[]> PerProjectPathWithVersion();

}
public record PackageWithVersion(string PackageId,string RequestedVersion, PackageOptions PackageOptions = PackageOptions.None)
{
    public ProjectData[] Projects { get; set; } = Array.Empty<ProjectData>();
}