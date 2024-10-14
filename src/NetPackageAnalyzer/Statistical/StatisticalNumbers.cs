using System.Drawing;
using System.Numerics;
namespace Statistical;

public static class StatisticalNumbers<T>
        where T : INumber<T>, IDivisionOperators<T, T, T>
{
    public static T? Max(T[] values) {

        if (values == null || values.Length == 0)
        {
            return default(T?);
        }
        return values.Max();
    }
    public static T? Min(T[] values) {
        if (values == null || values.Length == 0)
        {
            return default(T?);
        }
        return values.Min();
    }
    public static T Median(T[]? values)
    {
        var size = values?.Length ?? 0;
        if (size == 0)
        {
            return T.Zero;
        }
        ArgumentNullException.ThrowIfNull(values);
        if (size == 1)
        {
            return values![0];
        }
        Array.Sort(values);
        int mid = size / 2;
        T median = values[mid];
        if (size % 2 == 0)
        {
            median = (values[mid] + values[mid - 1]);
            median = median / T.CreateChecked(2);
        }
        return median;
    }
    public static T ArithmeticMean(T[]? values)
    {
        var size = values?.Length ?? 0;
        if (size == 0)
        {
            return T.Zero;
        }
        ArgumentNullException.ThrowIfNull(values);
        
        if(!T.TryParse(size.ToString(), null, out var mid )) {
            throw new ArgumentException("Cannot parse the size of the array to T");
        };
        

        if (size == 1)
        {
            return values![0];
        }
        T res= T.Zero;
        foreach (var value in values)
        {
            res += value;
        }
        
        return res / mid;
        
    }

    public static ModeResult<T>[] Mode(T[]? values)
    {
        var size = values?.Length ?? 0;
        if (size == 0)
        {
            return new[] { ModeResult<T>.Empty };
        }
        ArgumentNullException.ThrowIfNull(values);

        
        var modeWithCount = values
            .GroupBy(item => item)
            .OrderByDescending(group => group.Count())
            .ToArray();
        var nr = modeWithCount.First().Count();
        var modes = modeWithCount
            .Where(group => group.Count() == nr)
            .ToArray();

        return modes.Select(
            mode =>
            {
                var vals = values
                .Where(it => it == mode.Key)
                .Distinct()
                .ToArray();
                return new ModeResult<T>(vals, mode.Key);
            }).ToArray();
    }
    public static T Variance(T[] values)
    {
        var size = values.Length;
        if (values.Length == 0)
            return T.Zero;
         
        if (!T.TryParse(size.ToString(), null, out var mid))
        {
            throw new ArgumentException("Cannot parse the size of the array to T");
        };
        var avg = ArithmeticMean(values);
        var variance = T.Zero;
        foreach (var value in values)
        {
            var val = value - avg;
            val= val * val;
            variance += val;
        }
        return variance / mid; // For sample variance
    }
    //public static T Percentile(T[] sequence, int percentile, Func<T,double,T> f)
    //{
    //    double excelPercentile= ((double)percentile) / 100;
    //    Array.Sort(sequence);
    //    int N = sequence.Length;
    //    double n = (N - 1) * excelPercentile + 1;
    //    // Another method: double n = (N + 1) * excelPercentile;
    //    if (n == 1d) return sequence[0];
    //    else if (n == N) return sequence[N - 1];
    //    else
    //    {
    //        int k = (int)n;
    //        double d = n - k;
    //        return sequence[k - 1] +  f(sequence[k] - sequence[k - 1],d);
    //    }
    //}
}
public record Statistics<T>(T[] values)
    where T : INumber<T>, IDivisionOperators<T, T, T>
{
    public T Median => StatisticalNumbers<T>.Median(values);
    public T ArithmeticMean => StatisticalNumbers<T>.ArithmeticMean(values);
    public ModeResult<T>[] Mode => StatisticalNumbers<T>.Mode(values);
    public T Variance => StatisticalNumbers<T>.Variance(values);

    public T? Max => StatisticalNumbers<T>.Max(values);
    public T? Min => StatisticalNumbers<T>.Min(values);
    //public T Percentile(int percentile, Func<T,double,T> multiply) => StatisticalNumbers<T>.Percentile(values, percentile, multiply);
} 

