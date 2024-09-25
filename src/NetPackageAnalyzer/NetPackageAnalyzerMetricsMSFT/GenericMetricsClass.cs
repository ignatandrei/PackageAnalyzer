namespace NetPackageAnalyzerMetricsMSFT;

public class GenericMetricsClass : GenericMetrics
{
    public GenericMetricsClass()
    {

    }
    public int NumberOfMethods()
    {
        return Childs.Length;
    }
}
