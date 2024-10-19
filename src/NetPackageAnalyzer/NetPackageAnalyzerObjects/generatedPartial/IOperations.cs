namespace NetPackageAnalyzerObjects;

public interface IOperations
{
    void ClearWrongData();
    public string[] ProjectsPath();
    public string[] TopLevelPackagesIDs();
    Dictionary<string, PackageWithVersion[]> PerProjectPathWithVersion();

}
