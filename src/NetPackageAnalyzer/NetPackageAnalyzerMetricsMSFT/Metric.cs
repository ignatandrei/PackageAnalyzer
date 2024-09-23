namespace NetPackageAnalyzerMetricsMSFT;

public record Metric(string Name)
{
    public int? Value { get; set; }
    public bool IsInitialized => Value.HasValue;
}
