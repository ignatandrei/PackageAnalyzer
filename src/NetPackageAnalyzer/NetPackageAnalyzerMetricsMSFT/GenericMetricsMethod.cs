namespace NetPackageAnalyzerMetricsMSFT;

public class GenericMetricsMethod : GenericMetrics
{
    public GenericMetricsMethod()
    {

    }
    public bool IsGetSetWith100()
    {
        if (this.Name.EndsWith(".get") || this.Name.EndsWith(".set"))
        {
            return this.metrics[eMSFTMetrics.MaintainabilityIndex].Value == 100;
        }
        return false; 
    }
}