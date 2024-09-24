using System.ComponentModel.DataAnnotations;

namespace NetPackageAnalyzerMetricsMSFT;

public record Metric(eMSFTMetrics Name): IValidatableObject
{
    public int? Value { get; set; }
    public bool IsInitialized => Value.HasValue;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!IsInitialized)
            yield return new ValidationResult("must have value");
    }
}
