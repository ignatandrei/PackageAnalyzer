global using static System.Console;
global using NetPackageAnalyzerObjects;
global using OutDated = NS_GeneratedJson_outdatedV1_gen_json;
global using Deprecated = NS_GeneratedJson_deprecatedV1_gen_json;
global using All = NS_GeneratedJson_includeV1_gen_json;
global using System;
global using System.Collections.Generic;
global using System.IO.Abstractions;
global using System.Linq;
global using System.Text.Json;
global using System.Threading.Tasks;
global using RSCG_ExportDiagram_Import;
public static class GlobalsForGenerating
{
    public static string prefixSite = "pathname:///docs/Analysis/";//for markdown

    public static string FullPathToSolution = "";
    public static string NameSolution = "";
    public static string Version { get; set; }=string.Empty;
    public static string globalPrefix() 
    {
        return prefixSite + NameSolution;
    }
    public static string RelativePath(string? pathProject)
    {
        if(string.IsNullOrWhiteSpace(pathProject))
            return pathProject;

        return Path.GetRelativePath(FullPathToSolution, pathProject);
    }
}

