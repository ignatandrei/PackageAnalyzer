namespace NetPackageAnalyzerObjects;
public record PackageGatherInfo(string PackageId)
{
    public void CopyWhyFrom(PackageGatherInfo from)
    {
        this.Why = from.Why;
    }
    public void VerifyWhy()
    {
        if (PackageId == "Microsoft.NETCore.Platforms") return;
        if (Why.Length > 0) return;
        List<WhyData> whyDatas = new();
        ProcessOutput processOutput = new();
        try
        {

            var str = processOutput.OutputWhy(GlobalsForGenerating.FullPathToSolution, PackageId);
            var arrStr = str.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries); ;
            WhyData? whyData = null;
            foreach (var item in arrStr)
            {
                if (string.IsNullOrWhiteSpace(item?.Trim()))
                {
                    continue;
                }
                if (item.Contains("does not have a dependency on"))
                    continue;
                if (string.IsNullOrWhiteSpace(item?.Trim()))
                {
                    continue;
                }
                if (item.Contains("has the following dependency"))
                {
                    if (whyData != null)
                    {
                        whyDatas.Add(whyData);
                    }
                    whyData = new();
                    whyData.ProjectText = item;
                    continue;
                }
                whyData!.WhyText += Environment.NewLine + item;
            }
            if (whyData != null)
            {
                whyDatas.Add(whyData);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"!!!Error in  dependency {PackageId} : " + ex.Message);
        }
        Why = whyDatas.ToArray();
    }
    public WhyData[] Why = [];

}
public record PackageWithVersion(string PackageId,string RequestedVersion, PackageOptions PackageOptions = PackageOptions.None)
: PackageGatherInfo(PackageId)
{
    
    public ProjectData[] Projects { get; set; } = Array.Empty<ProjectData>();
    


}