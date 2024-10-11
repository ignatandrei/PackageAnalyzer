namespace Statistical;
public record ModeResult<T>(T[] Values, int Count)
{
    public static ModeResult<T> Empty { get; } = new ModeResult<T>(Array.Empty<T>(), 0);

}