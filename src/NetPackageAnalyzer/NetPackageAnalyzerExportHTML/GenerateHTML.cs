using NetPackageAnalyzerObjects;

namespace NetPackageAnalyzerExportHTML;

public class GenerateHTML : GenerateFiles
{
    public GenerateHTML(IFileSystem system) : base(system)
    {

    }
    public override async Task<string> GenerateNow(string folder, string where)
    {
        string tempFolder = string.Empty;
        try
        {
            if (!Directory.Exists(where))
                Directory.CreateDirectory(where);

            var folderResults = string.IsNullOrWhiteSpace(where) ? Path.Combine(folder, "Analysis") : where;
            tempFolder = GenerateDocsForClasses(GlobalsForGenerating.FullPathToSolution, folderResults);
            var (refSummary, publicClassRefData, assemblyDataFromMSFT) = AnalyzeDiagrams(tempFolder);
            //var x = new HtmlSummary(infoSol, projectsDict, modelMore1Version, refSummary, publicClassRefData);
            var x = new HtmlSummary(Tuple.Create(infoSol, projectsDict, modelMore1Version, refSummary, publicClassRefData));
            var html = x.Render();


            var nameFile = Path.Combine(where, $"{NameSolution}_summary.html");
            await system.File.WriteAllTextAsync(nameFile, html);
            WriteJs(where);
            return nameFile;
        }
        finally
        {
            if ((!string.IsNullOrWhiteSpace(tempFolder)) && Directory.Exists(tempFolder))
            {
                try
                {
                    Console.WriteLine($"Deleting {tempFolder}");
                    Directory.Delete(tempFolder, true);
                }
                catch (Exception)
                {
    //TODO : log                    
                }
            }
        }
    }
    void WriteJs(string where)
    {
        var res = MyResource.GetMermaidJs();
        var nameFileJs = Path.Combine(where, "mermaid.min.js");
        system.File.WriteAllBytes(nameFileJs, res.ToArray());
        
        res = MyResource.GetEchartsJs();
        nameFileJs = Path.Combine(where, "echarts.min.js");
        system.File.WriteAllBytes(nameFileJs, res.ToArray());


    }
}


