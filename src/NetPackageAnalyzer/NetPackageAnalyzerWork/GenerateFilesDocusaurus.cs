using NetPackageAnalyzerObjects;

namespace NetPackageAnalyzerDocusaurus;



public class GenerateFilesDocusaurus:GenerateFiles
{
    public GenerateFilesDocusaurus(IFileSystem system):base(system)
    {
        
    }
    
    public override async Task<string> GenerateNow(string folder, string where)
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
        
        await File.WriteAllTextAsync(file, await generator.Generate_SolutionIntroduction(infoSol));
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
        ClassesRefData? refSummary = null;
        PublicClassRefData? publicClassRefData = null;
        AssemblyDataFromMSFT? assemblyDataFromMSFT = null;
        if (tempFolder != null)
        {
            (refSummary,publicClassRefData, assemblyDataFromMSFT) = AnalyzeDiagrams(tempFolder);
            file = Path.Combine(folderResults, "ReferencesSummaryProjects.md");
            await File.WriteAllTextAsync(file, await generator.Generate_ReferencesSummaryProjects(refSummary));

            file = Path.Combine(folderResults, "PublicClassesProject.md");
            await File.WriteAllTextAsync(file, await generator.Generate_PublicClasses(publicClassRefData));
            
            //move .md files to the right place
            var files = Directory.GetFiles(tempFolder, "*.md");
            foreach (var fileMd in files)
            {
                string nameCsproj = Path.GetFileNameWithoutExtension(fileMd);
                nameCsproj = nameCsproj.Replace("_rel_csproj", "");

                var fileDest = Path.Combine(folderResults,"Projects",nameCsproj );
                if(!Directory.Exists(fileDest))
                {
                    //TB:2025-01-01 solve wrong the name of the csproj 
                    //Console.WriteLine($"Directory {fileDest} does not exist");
                    File.Delete(fileMd);
                    continue;
                }
                fileDest = Path.Combine(fileDest, Path.GetFileName(fileMd));
                try
                {
                    File.Move(fileMd, fileDest,true);
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Exception moving {fileMd} to {fileDest} ");
                    Console.WriteLine("Exception"+ ex.Message);
                    Console.WriteLine("Exception!! " + ex.StackTrace);
                }
            }
        }
        file = Path.Combine(folderResults, "BlogPost.md");
        await File.WriteAllTextAsync(file, await generator.Generate_BlogPost(infoSol, projectsDict, modelMore1Version, refSummary, publicClassRefData));

        return "";
    }
}
