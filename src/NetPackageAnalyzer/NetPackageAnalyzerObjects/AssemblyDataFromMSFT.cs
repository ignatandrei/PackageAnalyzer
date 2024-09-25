namespace NetPackageAnalyzerObjects;
/// <summary> 
/// https://learn.microsoft.com/en-us/visualstudio/code-quality/code-metrics-values?view=vs-2022
/// </summary>
public class AssemblyDataFromMSFT
{
    private readonly GenericMetricsAssembly[] genericMetricsAssembly;
    
    public AssemblyDataFromMSFT(GenericMetricsAssembly[] genericMetricsAssembly)
    {
        this.genericMetricsAssembly = genericMetricsAssembly;
    }
    public NamePerCount[] AssemblyNumberClasses()
    {
        return genericMetricsAssembly
            .Select(it => new NamePerCount(it.Name.Replace(".csproj", ""), it.Childs.Length))
            .ToArray();
    }
    public NamePerCount[] AssemblyMetric(eMSFTMetrics metrics)
    {
        return genericMetricsAssembly
            .Select(it => new
                    NamePerCount(it.Name.Replace(".csproj",""), it.metrics[metrics].Value ?? -1))
            .ToArray();      
    }
    public NamePerCount? AssemblyMetricMax(eMSFTMetrics metrics)
    {
        var data= AssemblyMetric(metrics);
        return data.OrderByDescending(it => it.Count).FirstOrDefault();
    }
    public NamePerCount? AssemblyMetricMin(eMSFTMetrics metrics)
    {
        var data = AssemblyMetric(metrics);
        return data.OrderBy(it => it.Count).FirstOrDefault();
    }
    public NamePerCount[] ClassesMetrics(eMSFTMetrics metrics)
    {
        var data= genericMetricsAssembly
            .SelectMany(it => it.Childs)
            .Select(it => new
                    NamePerCount(it.Name, it.metrics[metrics].Value ?? -1))
            .Where(it=>it.Count!=-1)
            .ToArray();
        return data; 
    }
    public NamePerCount? ClassesMetricMax(eMSFTMetrics metrics)
    {
        var data = ClassesMetrics(metrics);
        return data.OrderByDescending(it => it.Count).FirstOrDefault();
    }
    public NamePerCount? ClassesMetricMin(eMSFTMetrics metrics)
    {
        var data = ClassesMetrics(metrics);
        return data.OrderBy(it => it.Count).FirstOrDefault();
    }
    public NamePerCount[] MethodsMetrics(eMSFTMetrics metrics)
    {
        var data = genericMetricsAssembly
            .SelectMany(it => it.Childs)
            .SelectMany(it => it.Childs)
            .Where(it=>it is GenericMetricsMethod)
            .Select(it => new
                    NamePerCount(it.Name, it.metrics[metrics].Value ?? -1))
            .Where(it => it.Count != -1)
            .ToArray();
        return data;
    }
}
