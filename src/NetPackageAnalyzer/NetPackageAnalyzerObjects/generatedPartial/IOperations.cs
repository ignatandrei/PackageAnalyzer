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
    public void VerifyWhy()
    {
        if(Why.Length>0)return;

        ProcessOutput processOutput = new();
        try
        {

            var str = processOutput.OutputWhy(GlobalsForGenerating.FullPathToSolution, PackageId);
            var arrStr = str.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries); ;
            foreach(var item in arrStr)
            {
                if (!item.Contains("does not have a dependency on"))
                {
                    Why += item +Environment.NewLine;                    
                }
            }
        }
        catch (Exception ex)
        {
            Why = ex.Message;
        }
    }

    public string Why=string.Empty;


}