using NetPackageAnalyzerObjects;
using NPA.HtmlData;

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
            tempFolder = GenerateDocsForClasses(GlobalsForGenerating.FullPathToSolution??"", folderResults??"")??"";
            var (refSummary, publicClassRefData, assemblyDataFromMSFT) = AnalyzeDiagrams(tempFolder);
            //var x = new HtmlSummary(infoSol, projectsDict, modelMore1Version, refSummary, publicClassRefData);
            if (projectsDict == null || modelMore1Version== null)
                return string.Empty;

            var modelData = Tuple.Create(infoSol, projectsDict, modelMore1Version, refSummary, publicClassRefData, assemblyDataFromMSFT);
            if(modelData == null)
                return string.Empty;
            var x = new HtmlSummary(modelData);
            var html = x.Render();


            var nameFile = Path.Combine(where, $"{NameSolution}_summary.html");
            await system.File.WriteAllTextAsync(nameFile, html);
            WriteJs(where); 
            var ex = new ExtractImages(nameFile);
            await ex.GetImagesAsync();
            MDSummaryData md = new ();
            md.nameSolution = GlobalsForGenerating.NameSolution;
            md.ExistsMajorDiffers= (modelMore1Version.KeysPackageMultipleMajorDiffers().Length > 0);
            var mdSummary = new MDSummary(md);
            var mdHtml = mdSummary.Render();
            var nameFileMD = Path.Combine(where, $"{NameSolution}_summary.md");
            await system.File.WriteAllTextAsync(nameFileMD, mdHtml);
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

        res = MyResource.GetVisJs();
        nameFileJs = Path.Combine(where, "vis-network.min.js");
        system.File.WriteAllBytes(nameFileJs, res.ToArray());

        res = MyResource.GetTabulatorCss();
        nameFileJs = Path.Combine(where, "tabulator.min.css");
        system.File.WriteAllBytes(nameFileJs, res.ToArray());

        res = MyResource.GetTabulatorJs();
        nameFileJs = Path.Combine(where, "tabulator.min.js");
        system.File.WriteAllBytes(nameFileJs, res.ToArray());

        res = MyResource.GetTabulatorTheme();
        nameFileJs = Path.Combine(where, "tabulator.theme.min.js");
        system.File.WriteAllBytes(nameFileJs, res.ToArray());

        res = MyResource.GetDriverCss();
        nameFileJs = Path.Combine(where, "driver.css");
        system.File.WriteAllBytes(nameFileJs, res.ToArray());

        res = MyResource.GetDriverJS();
        nameFileJs = Path.Combine(where, "driver.js.iife.js");
        system.File.WriteAllBytes(nameFileJs, res.ToArray());

    }
}


