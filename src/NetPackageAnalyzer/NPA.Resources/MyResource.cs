namespace NPA.Resources;

public partial class MyResource
{
    [EmbedResourceCSharp.FileEmbed("mermaid.min.js")]
    public static partial System.ReadOnlySpan<byte> GetMermaidJs();
    [EmbedResourceCSharp.FileEmbed("echarts.min.js")]
    public static partial System.ReadOnlySpan<byte> GetEchartsJs();

    //[EmbedResourceCSharp.FileEmbed("vis-network.min.js")]
    //public static partial System.ReadOnlySpan<byte> GetVisJs();

    [EmbedResourceCSharp.FileEmbed("tabulator.min.js")]
    public static partial System.ReadOnlySpan<byte> GetTabulatorJs();

    [EmbedResourceCSharp.FileEmbed("tabulator.min.css")]
    public static partial System.ReadOnlySpan<byte> GetTabulatorCss();

    [EmbedResourceCSharp.FileEmbed("tabulator_midnight.min.css")]
    public static partial System.ReadOnlySpan<byte> GetTabulatorTheme();
    [EmbedResourceCSharp.FileEmbed("driver.css")]
    public static partial System.ReadOnlySpan<byte> GetDriverCss();

    [EmbedResourceCSharp.FileEmbed("driver.js.iife.js")]
    public static partial System.ReadOnlySpan<byte> GetDriverJS();

}
