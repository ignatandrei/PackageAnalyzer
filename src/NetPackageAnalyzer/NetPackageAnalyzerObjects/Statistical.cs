using System.Collections.Immutable;
using System.Numerics;

namespace NetPackageAnalyzerObjects;
public static class Statistical<T>
        where T : INumber<T>
{
    public static T Median(T[]? values)
    {
        var size = values?.Length??0;
        if (size == 0)
        {
            return T.Zero;
        }
        ArgumentNullException.ThrowIfNull(values);
        if (size ==1)
        {
            return values![0];
        }
        Array.Sort(values);
        int mid = size / 2;
        T median = values[mid];
        if(size % 2 == 0)
        {
            median = (values[mid] + values[mid - 1]);
            median = median / T.CreateChecked( 2);
        }
        return median;
    }
}
