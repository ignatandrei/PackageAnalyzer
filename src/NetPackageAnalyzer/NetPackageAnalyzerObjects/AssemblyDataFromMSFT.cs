using NetPackageAnalyzerMetricsMSFT;

namespace NetPackageAnalyzerObjects;
public class AssemblyDataFromMSFT
{
    private readonly GenericMetricsAssembly[] genericMetricsAssembly;

    public AssemblyDataFromMSFT(GenericMetricsAssembly[] genericMetricsAssembly)
    {
        this.genericMetricsAssembly = genericMetricsAssembly;
    }
    public NamePerCount[] AssemblyMetric(eMSFTMetrics metrics)
    {
        return genericMetricsAssembly
            .Select(it => new
                    NamePerCount(it.Name, it.metrics[metrics].Value ?? -1))
            .ToArray();      
    }
}
