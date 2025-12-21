using NPA.BigResources;
using System.Diagnostics;
using System.IO.Compression;

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
            var projectFiles = (projectsDict!.Select(it => it.Value?.PathProject).ToArray())??[];
            tempFolder = await GenerateDocsForClasses(projectFiles, folderResults ?? "") ?? "";
            var (refSummary, publicClassRefData, assemblyDataFromMSFT) = AnalyzeDiagrams(tempFolder);
            //var x = new HtmlSummary(infoSol, projectsDict, modelMore1Version, refSummary, publicClassRefData);
            if (projectsDict == null || modelMore1Version == null)
                return string.Empty;
            var packDTO = base.packDTO;
            var modelData = Tuple.Create(infoSol, projectsDict, modelMore1Version, refSummary, publicClassRefData, assemblyDataFromMSFT, packDTO);
            if (modelData == null)
                return string.Empty;
            
            var r = new matzehuels_stacktower(modelData);
            var jsonStackTower = await r.RenderAsync();

            var x = new HtmlSummary(modelData);
            var html = await x.RenderAsync();


            var nameFile = Path.Combine(where, $"{NameSolution}_summary.html");
            await system.File.WriteAllTextAsync(nameFile, html);
            var okTower = await GenerateStackTower(where, jsonStackTower);
            WriteJs(where);
            var ex = new ExtractImages(nameFile);
            await ex.GetImagesAsync();
            MDSummaryData md = new();
            md.nameSolution = GlobalsForGenerating.NameSolution;
            md.ExistsMajorDiffers = (modelMore1Version.KeysPackageMultipleMajorDiffers().Length > 0);
            md.ExistsVulnerable = infoSol.nrVulnerable > 0;

            var mdSummary = new MDSummary(md);
            var mdHtml = await mdSummary.RenderAsync();
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

    private async Task<bool> GenerateStackTower(string where, string jsonStackTower)
    {
        var temp = Path.GetTempPath();
        var pathZip = Path.Combine(temp, "stacktower" + DateTime.Now.ToString("yyyyMMdd"));
        await ZipBigFiles.SaveToFile(pathZip, EmbeddedResource.stacktower_exe_zip);

        var tower = Path.Combine(where, $"{NameSolution}_tower.json");
        await system.File.WriteAllTextAsync(tower, jsonStackTower);
        string outputSvgFile = await ExecuteStackTower(pathZip, tower, "barycentric");
        outputSvgFile = await ExecuteStackTower(pathZip, tower, "optimal");
        //TODO : log the result
        return File.Exists(outputSvgFile);

    }

    private static async Task<string> ExecuteStackTower(string pathZip, string tower ,string ordering)
    {
        //TODO: move to images_eShop_summary
        string nameSolution = GlobalsForGenerating.NameSolution;
        var pathTower = Path.GetDirectoryName(tower);
        var outputSvgFile = Path.Combine(pathTower!, $"images_{nameSolution}_summary", $"{tower}_{ordering}.svg");
        Console.WriteLine("Writing output to " + outputSvgFile);
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = Path.Combine(pathZip, "stacktower.exe"),
            Arguments = $"render {tower} -t tower --style handdrawn --popups --ordering {ordering} -o {outputSvgFile}",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            WorkingDirectory = pathZip
        };
        try
        {
            // Create and start the process
            Process process = new Process { StartInfo = startInfo };
            process.Start();
            // Wait for the process to finish
            await process.WaitForExitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("error for generating stacks" + ex.Message);
            throw;
        }

        return outputSvgFile;
    }

    void WriteJs(string where)
    {
        var res = MyResource.GetMermaidJs();
        var nameFileJs = Path.Combine(where, "mermaid.min.js");
        system.File.WriteAllBytes(nameFileJs, res.ToArray());
        
        res = MyResource.GetEchartsJs();
        nameFileJs = Path.Combine(where, "echarts.min.js");
        system.File.WriteAllBytes(nameFileJs, res.ToArray());

        //res = MyResource.GetVisJs();
        //nameFileJs = Path.Combine(where, "vis-network.min.js");
        //system.File.WriteAllBytes(nameFileJs, res.ToArray());

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


