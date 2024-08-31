namespace NetPackageAnalyzerExportHTML;

public class GenerateHTML : GenerateFiles
{
    public GenerateHTML(IFileSystem system) : base(system)
    {

    }
    public override async Task<int> GenerateNow(string folder, string where)
    {

        //var x = new HtmlSummary(infoSol, projectsDict, modelMore1Version, refSummary, publicClassRefData);
        var x = new HtmlSummary(Tuple.Create(infoSol, projectsDict, modelMore1Version, (ClassesRefData)null,(PublicClassRefData) null));
        var html = x.Render();
        var nameFile = Path.Combine(where, $"{NameSolution}_summary.html");
        await system.File.WriteAllTextAsync(nameFile, html);
        WriteMermaidJs(where);
        return 0;
    }
    void WriteMermaidJs(string where)
    {
        var res = MyResource.GetMermaidJs();
        var nameFileJs = Path.Combine(where, "mermaid.min.js");
        system.File.WriteAllBytes(nameFileJs, res.ToArray());
    }
}


