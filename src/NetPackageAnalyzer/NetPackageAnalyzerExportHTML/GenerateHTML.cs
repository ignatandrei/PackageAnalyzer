using NetPackageAnalyzerObjects;

namespace NetPackageAnalyzerExportHTML;

public class GenerateHTML : GenerateFiles
{
    public GenerateHTML(IFileSystem system) : base(system)
    {

    }
    public override async Task<int> GenerateNow(string folder, string where)
    {

        //var x = new HtmlSummary(infoSol, projectsDict, modelMore1Version, refSummary, publicClassRefData);
        var x = new HtmlSummary(Tuple.Create((InfoSolution)null, projectsDict, modelMore1Version, (ClassesRefData)null,(PublicClassRefData) null));
        var html = x.Render();
        var nameFile = Path.Combine(where, $"{NameSolution}_summary.html");
        await system.File.WriteAllTextAsync(nameFile, html);
        return 0;
    }
}

