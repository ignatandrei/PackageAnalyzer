using System.Numerics;

namespace Statistical;
public record ModeResult<T>(T[] Values, T Value)
    where T : INumber<T>
{
    public long Count() => Values.Length;
    public static ModeResult<T> Empty { get; } = new ModeResult<T>(Array.Empty<T>(), T.Zero);

}