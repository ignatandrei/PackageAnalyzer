using System.Numerics;
namespace Statistical;

public static class StatisticalNumbers<T>
        where T : INumber<T>,IDivisionOperators<T, T, T>
{
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

    public static ModeResult<T> Mode(T[]? values)
    {
        var size = values?.Length ?? 0;
        if (size == 0)
        {
            return ModeResult<T>.Empty;
        }
        ArgumentNullException.ThrowIfNull(values);

        var data = values
            .GroupBy(v=>v)
            .ToDictionary(v=>v,v=>v.Count());

        var max = data.Max(it=>it.Value);
        var vals = data
            .Where(it => it.Value == max)
            .Select(it => it.Key.Key)
            .ToArray();

        return new ModeResult<T>(vals ?? [] , max);

    }
    
}
public record Statistics<T>(T[] values)
    where T : INumber<T>, IDivisionOperators<T, T, T>
{
    public T Median => StatisticalNumbers<T>.Median(values);
    public T ArithmeticMean => StatisticalNumbers<T>.ArithmeticMean(values);
    public ModeResult<T> Mode => StatisticalNumbers<T>.Mode(values);
}

