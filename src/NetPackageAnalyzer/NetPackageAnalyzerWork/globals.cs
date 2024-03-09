global using NetPackageAnalyzerConsole;
global using System.Diagnostics;
global using System.Text.Json;
//global using NS_;
global using OutDated = NS_GeneratedJson_outdatedV1_gen_json;
global using Deprecated = NS_GeneratedJson_deprecatedV1_gen_json;
global using All = NS_GeneratedJson_includeV1_gen_json;
global using static System.Console;
global using NetPackageAnalyzerConsole.generatedPartial;
global using NS_GeneratedJson_deprecatedV1_gen_json;
global using NetPackageAnalyzerWork;
global using NetPackageAnalyzerWork.Templates;
global using System.IO.Abstractions;

public static class GlobalsForGenerating
{
    public static string prefixSite = "pathname:///docs/Analysis/";//for markdown
    public static string NameSolution = "";

    public static string globalPrefix()
    {
        return prefixSite + NameSolution;
    }
}