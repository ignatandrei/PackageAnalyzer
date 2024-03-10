global using static System.Console;
global using NetPackageAnalyzerObjects;
global using OutDated = NS_GeneratedJson_outdatedV1_gen_json;
global using Deprecated = NS_GeneratedJson_deprecatedV1_gen_json;
global using All = NS_GeneratedJson_includeV1_gen_json;

public static class GlobalsForGenerating
{
    public static string prefixSite = "pathname:///docs/Analysis/";//for markdown
    public static string NameSolution = "";

    public static string globalPrefix()
    {
        return prefixSite + NameSolution;
    }
}