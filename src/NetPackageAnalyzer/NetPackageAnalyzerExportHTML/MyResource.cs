namespace NetPackageAnalyzerExportHTML;

internal partial class MyResource
{
    [EmbedResourceCSharp.FileEmbed("mermaid.min.js")]
    public static partial System.ReadOnlySpan<byte> GetMermaidJs();
    [EmbedResourceCSharp.FileEmbed("echarts.min.js")]
    public static partial System.ReadOnlySpan<byte> GetEchartsJs();

    [EmbedResourceCSharp.FileEmbed("vis-network.min.js")]
    public static partial System.ReadOnlySpan<byte> GetVisJs();

}


