namespace NetPackageAnalyzerConsole.generatedPartial;

internal interface IOperations
{
    void ClearWrongData();
    public string[] ProjectsPath();
    public string[] TopLevelPackagesIDs();
    Dictionary<string, PackageWithVersion[]> PerProjectPathWithVersion();

}
public record PackageWithVersion(string PackageId,string RequestedVersion)
{

}