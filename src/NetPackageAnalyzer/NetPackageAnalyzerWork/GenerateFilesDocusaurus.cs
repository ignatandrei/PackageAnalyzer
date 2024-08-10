namespace NetPackageAnalyzerDocusaurus;
public record NamePerCount(string Name, int Count)
{


}



public class GenerateFilesDocusaurus:GenerateFiles
{
    public GenerateFilesDocusaurus(IFileSystem system):base(system)
    {
        
    }
    
    public override async Task<int> GenerateNow(string folder, string where)
    {

        var folderResults = string.IsNullOrWhiteSpace(where) ? Path.Combine(folder, "Analysis") : where;
        if(!Directory.Exists(folderResults))
            Directory.CreateDirectory(folderResults);
        var zip = Path.Combine(folderResults, "docusaurus.zip");
        Console.WriteLine("generate docusaurus at " + zip);
        await File.WriteAllBytesAsync(zip, MyResource.GetDocusaurusZip().ToArray());
        ZipFile.ExtractToDirectory(zip, folderResults,true);

        //generate copyright
        var fileConfig = Path.Combine(folderResults, "docusaurus.config.ts");
        var data=await File.ReadAllTextAsync(fileConfig);
        data = data.Replace("My Project, Inc", "SolutionAnalyzer");
        data=data.Replace("Built with Docusaurus", "version "+GlobalsForGenerating.Version);
        await File.WriteAllTextAsync(fileConfig, data);
        string generalSolution = $$"""
{
  "label": "Solutions",
  "position": 1,
  "link": {
    "type": "generated-index"
  }
}
""";
        folderResults = Path.Combine(folderResults, "docs");
        var fileRoot = Path.Combine(folderResults, "_category_.json");
        await File.WriteAllTextAsync(fileRoot, generalSolution);


        folderResults = Path.Combine(folderResults,"Analysis", NameSolution);
        WriteLine($"generate documentation in {folderResults}");
        if (!Directory.Exists(folderResults))
            Directory.CreateDirectory(folderResults);
        
        TemplateGenerator generator = new();

        var file = Path.Combine(folderResults, "DisplayAllVersions.html");
        await File.WriteAllTextAsync(file, await generator.Generate_DisplayAllVersions(modelMore1Version));

        file = Path.Combine(folderResults, "DisplayOutdatedDeprecated.md");

        await File.WriteAllTextAsync(file, await generator.Generate_OutDeprMarkdown(Problems()));

        file = Path.Combine(folderResults, "DisplayAllVersions.md");
        await File.WriteAllTextAsync(file, await generator.Generate_DisplayAllVersionsMarkdown(modelMore1Version));

        file = Path.Combine(folderResults, $"MermaidVisualizerMajorDiffer.md");
        await File.WriteAllTextAsync(file, await generator.Generate_MermaidVisualizerMajorDiffer(modelMore1Version));

        file = Path.Combine(folderResults, "ProjectRelation.md");
        ArgumentNullException.ThrowIfNull(projectsDict);
        await File.WriteAllTextAsync(file, await generator.Generate_SolutionRelations(projectsDict));

        var folderProjects = Path.Combine(folderResults, "Projects");
        if (!Directory.Exists(folderProjects))
            Directory.CreateDirectory(folderProjects);

        var projects = $$"""
{
  "label": "Projects",
  "position": 1000,
  "link": {
    "type": "generated-index"
  }
}
""";

        await File.WriteAllTextAsync(Path.Combine(folderProjects, "_category_.json"), projects);

        foreach (var projData in projectsDict.AlphabeticOrderedProjects)
        {
            var folderProject = Path.Combine(folderProjects, projData.NameCSproj());
            if (!Directory.Exists(folderProject))
                Directory.CreateDirectory(folderProject);

            var project = $$"""
{
  "label": "{{projData.NameCSproj()}}",
  "position": 1,
  "link": {
    "type": "generated-index"
  }
}
""";

            await File.WriteAllTextAsync(Path.Combine(folderProject, "_category_.json"), project);

            file = Path.Combine(folderProject, "ProjectReferences.md");
            await File.WriteAllTextAsync(file, await generator.Generate_ProjectRelations(projData));

            file = Path.Combine(folderProject, "Packages.md");
            await File.WriteAllTextAsync(file, await generator.Generate_ProjectPackages(projData));

            file = Path.Combine(folderProject, "Commits.md");
            await File.WriteAllTextAsync(file, await generator.Generate_ProjectCommits(projData));


        }

        file = Path.Combine(folderResults, "_category_.json");
        string categoryGenerated = $$"""
{
  "label": "{{NameSolution}}",
  "position": 1
}
""";
        await File.WriteAllTextAsync(file, categoryGenerated);

        file = Path.Combine(folderResults, "index.md");
        var nrOutdated=outdated.GroupBy(it => it.PackageId).Count();
        var nrDeprecated=deprecated.GroupBy(it => it.PackageId).Count();
        var infoSol=new InfoSolution(
            this.projectsDict!.Count, 
            packagedDict.Count, nrOutdated,nrDeprecated,
            this.projectsDict!.TotalCommits(),
            this.projectsDict!.TestsProjects.Count(),
            modelMore1Version.KeysPackageMultipleMajorDiffers().Length
            );
        await File.WriteAllTextAsync(file, await generator.Generate_SolutionIntroduction(infoSol));
        file = Path.Combine(folderResults, "BlogPost.md");
        await File.WriteAllTextAsync(file, await generator.Generate_BlogPost(infoSol, projectsDict, modelMore1Version));
        file = Path.Combine(folderResults, "BuildingBlocks.md");
        await File.WriteAllTextAsync(file, await generator.Generate_BuildingBlocks(projectsDict));

        file = Path.Combine(folderResults, "Commits.md");
        await File.WriteAllTextAsync(file, await generator.Generate_Commits(projectsDict));


        file = Path.Combine(folderResults, "TestProjects.md");
        await File.WriteAllTextAsync(file, await generator.Generate_TestProjects(projectsDict));

        file = Path.Combine(folderResults, "RootProjects.md");
        await File.WriteAllTextAsync(file, await generator.Generate_RootProjects(projectsDict));

        //file = Path.Combine(folderResults, "DisplayAllVersionsWithProblems.md");
        //ArgumentNullException.ThrowIfNull(projectsDict);
        //await File.WriteAllTextAsync(file, await generator.Generate_DisplayAllVersionsWithProblemsMarkdown(model));
        var tempFolder = GenerateDocsForClasses(GlobalsForGenerating.FullPathToSolution, folderResults);
        if(tempFolder != null)
        {
            var refSummary= AnalyzeDiagrams(tempFolder);
            file = Path.Combine(folderResults, "ReferencesSummaryProjects.md");
            await File.WriteAllTextAsync(file, await generator.Generate_ReferencesSummaryProjects(refSummary));
        }
        return 1;
    }

    private ClassesRefData AnalyzeDiagrams(string tempFolder)
    {
        List<ExportAssembly> expAss = new ();
        var files = Directory.GetFiles(tempFolder, "*.json");
        foreach (var fileJson in files)
        {
            var json = File.ReadAllText(fileJson);
            ExportAssembly? ex = JsonSerializer.Deserialize<ExportAssembly>(json);
            if (ex == null)
                continue;
            expAss.Add(ex);
        }
        var allExtReferences = expAss
            .SelectMany(it=>it.ClassesWithExternalReferences)
            .SelectMany(it=>it.MethodsWithExternalReferences)
            .SelectMany(it=>it.References)
            .ToArray();
        
        var maxRefAssembly = allExtReferences
            .GroupBy(it=>it.AssemblyName)
            .Select(it => new NamePerCount (it.Key, it.Count()))
            .OrderByDescending(it => it.Count)
            .Take(10)
            .ToArray();

        var maxRefMethods = allExtReferences
            .GroupBy(it => it.FullName)
            .Select(it => new NamePerCount (it.Key, it.Count() ))
            .OrderByDescending(it => it.Count)
            .Take(10)
            .ToArray();

        ClassesRefData ret = new();
        ret.MethodsReferences = maxRefMethods;
        ret.AssembliesReferences = maxRefAssembly;
        return ret;
    }

    private string? GenerateDocsForClasses(string fullPathToSolution, string folderResults)
    {
        try
        {
            var folder = Path.GetDirectoryName(fullPathToSolution);
            var fldTemp = folderResults + "_Temp";
            if (!Directory.Exists(fldTemp))
                Directory.CreateDirectory(fldTemp);
            RscgExportDataDiagram pwsh = new("2024.810.832", fldTemp);
            var code = pwsh.GenerateCode();
            var file = Path.Combine(folder, "ExportDiagram.ps1");
            File.WriteAllText(file, code);
            Console.WriteLine("Please wait - generate diagram classes");
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                WorkingDirectory = folder,
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{file}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"PowerShell Error: {error}");
                }
                else
                {
                    Console.WriteLine(output);
                }
                return fldTemp;

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception!! " + ex.Message);
            Console.WriteLine("Exception!! " + ex.StackTrace);
            return null;
        }
    }
}
