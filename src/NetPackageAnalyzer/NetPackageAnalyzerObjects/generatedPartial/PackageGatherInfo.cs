namespace NetPackageAnalyzerObjects;

public record PackageGatherInfo(string PackageId)
{
    static Dictionary<string, PackageGatherInfo> _allPackages = new();
    public void CopyWhyFrom(PackageGatherInfo from)
    {
        this.Why = from.Why;
    }
    public void VerifyWhy()
    {
        //if (PackageId == "Microsoft.NETCore.Platforms") return;
        if (Why.Length > 0) return;
        if(_allPackages.ContainsKey(PackageId))
        {
            CopyWhyFrom(_allPackages[PackageId]);
            return;
        }
        List<WhyData> whyDatas = new();
        ProcessOutput processOutput = new();
        try
        {

            var str = processOutput.OutputWhy(GlobalsForGenerating.FullPathToSolution, PackageId);
            var arrStr = str.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries); ;
            int tooManyLines = 5000;
            bool StopTooManyLines = (arrStr.Length > tooManyLines);
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
                    if (StopTooManyLines) break;
                    continue;
                }
                whyData!.WhyText += Environment.NewLine + item;
            }
            if (whyData != null)
            {
                if (StopTooManyLines)
                {
                    whyData.ProjectText = $"Too many lines {arrStr.Length} to show full dependency";
                }
                whyDatas.Add(whyData);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"!!!Error in  dependency {PackageId} : " + ex.Message);
        }
        Why = whyDatas.ToArray();
        _allPackages[PackageId] = this;
    }
    public WhyData[] Why = [];

    public string[] ProjectIDsFromWhy() { 
        return Why.Select(x => x.ProjectName()).ToArray();
    }

}
