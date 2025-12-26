using DotnetWhyParserObjects;

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
        if (Why != null) return;
        if(_allPackages.ContainsKey(PackageId))
        {
            if (_allPackages[PackageId].Why != null)
            {
                CopyWhyFrom(_allPackages[PackageId]);
                return;
            }
        }
        DotnetWhyParser parser = new();
        ProcessOutput processOutput = new();
        try
        {

            var str = processOutput.OutputWhy(GlobalsForGenerating.FullPathToSolution, PackageId);
            try
            {
                Why = parser.Parse(str);
            }
            catch (Exception ex)
            {
                Console.WriteLine("======="+ex.Message);
                throw;
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"!!!Error in  dependency {PackageId} : " + ex.Message);
        }
        
        _allPackages[PackageId] = this;
    }
    public DotnetWhyOutput? Why { get; private set; } =null;

    public string[] ProjectIDsFromWhy() {
        if (Why == null) return Array.Empty<string>();
        return Why
            .ProjectNames()
            .Distinct()
            .Where(it=>it.Length > 0)
            .ToArray();
    }

}
