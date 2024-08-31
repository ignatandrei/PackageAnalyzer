using NetPackageAnalyzerObjects;

namespace NetPackageAnalyzerExportHTML;

public class GenerateHTML : GenerateFiles
{
    public GenerateHTML(IFileSystem system) : base(system)
    {

    }
    public override async Task<string> GenerateNow(string folder, string where)
    {
        var folderResults = string.IsNullOrWhiteSpace(where) ? Path.Combine(folder, "Analysis") : where;
        var tempFolder = GenerateDocsForClasses(GlobalsForGenerating.FullPathToSolution, folderResults);
        var (refSummary, publicClassRefData) = AnalyzeDiagrams(tempFolder);
        //var x = new HtmlSummary(infoSol, projectsDict, modelMore1Version, refSummary, publicClassRefData);
        var x = new HtmlSummary(Tuple.Create(infoSol, projectsDict, modelMore1Version, refSummary,publicClassRefData));
        var html = x.Render();
        var nameFile = Path.Combine(where, $"{NameSolution}_summary.html");
        await system.File.WriteAllTextAsync(nameFile, html);
        WriteMermaidJs(where);
        return nameFile;
    }
    void WriteMermaidJs(string where)
    {
        var res = MyResource.GetMermaidJs();
        var nameFileJs = Path.Combine(where, "mermaid.min.js");
        system.File.WriteAllBytes(nameFileJs, res.ToArray());
    }
}


