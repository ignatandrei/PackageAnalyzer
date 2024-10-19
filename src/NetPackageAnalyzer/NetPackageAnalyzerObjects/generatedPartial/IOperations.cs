namespace NetPackageAnalyzerObjects;

public interface IOperations
{
    void ClearWrongData();
    public string[] ProjectsPath();
    public string[] TopLevelPackagesIDs();
    Dictionary<string, PackageWithVersion[]> PerProjectPathWithVersion();

}
public class WhyData
{
    public string ProjectText { get; set; } = string.Empty;
    public string ProjectName()
    {
        var last = "has the following";
        var start = ProjectText.IndexOf(last);
        var text = ProjectText.Substring(0,ProjectText.IndexOf(last));
        text=text.Replace("'", string.Empty);
        text=text.Trim();
        return text;
    }
    public string WhyText { get; set; } = string.Empty;
}
public record PackageWithVersion(string PackageId,string RequestedVersion, PackageOptions PackageOptions = PackageOptions.None)
{
    public void CopyWhyFrom(PackageWithVersion from)
    {
        this.Why = from.Why;
    }
    public ProjectData[] Projects { get; set; } = Array.Empty<ProjectData>();
    public void VerifyWhy()
    {
        if(Why.Length>0)return;
        List<WhyData> whyDatas = new();
        ProcessOutput processOutput = new();
        try
        {

            var str = processOutput.OutputWhy(GlobalsForGenerating.FullPathToSolution, PackageId);
            var arrStr = str.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries); ;
            WhyData? whyData = null;
            foreach (var item in arrStr)
            {
                if(string.IsNullOrWhiteSpace(item?.Trim()))
                {
                    continue;
                }
                if (item.Contains("does not have a dependency on"))
                    continue;
                if(string.IsNullOrWhiteSpace(item?.Trim()))
                {
                    continue;
                }
                if (item.Contains("has the following dependency"))
                {
                    if(whyData != null)
                    {
                        whyDatas.Add(whyData);
                    }
                    whyData = new ();
                    whyData.ProjectText = item;
                    continue;
                }
                whyData!.WhyText += Environment.NewLine+ item ;
            }
            if (whyData != null)
            {
                whyDatas.Add(whyData);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"!!!Error in  dependency {PackageId} : "+ ex.Message);
        }
        Why = whyDatas.ToArray();
    }

    public WhyData[] Why =[];


}