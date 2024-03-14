﻿using System.IO.Compression;

namespace NetPackageAnalyzerDocusaurus;

public class GenerateFilesDocusaurus:GenerateFiles
{
    public GenerateFilesDocusaurus(IFileSystem system):base(system)
    {
        
    }
    
    public override async Task GenerateNow(string folder, string where)
    {

        var folderResults = string.IsNullOrWhiteSpace(where) ? Path.Combine(folder, "Documentation") : where;
        if(!Directory.Exists(folderResults))
            Directory.CreateDirectory(folderResults);
        var zip = Path.Combine(folderResults, "docusaurus.zip");
        Console.WriteLine("generate docusaurus at" + zip);
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
        DisplayDataMoreThan1Version model = new(packagedDict, folder);

        TemplateGenerator generator = new();

        var file = Path.Combine(folderResults, "DisplayAllVersions.html");
        await File.WriteAllTextAsync(file, await generator.Generate_DisplayAllVersions(model));

        file = Path.Combine(folderResults, "DisplayOutdatedDeprecated.md");
        await File.WriteAllTextAsync(file, await generator.Generate_OutDeprMarkdown(base.Problems()));

        file = Path.Combine(folderResults, "DisplayAllVersions.md");
        await File.WriteAllTextAsync(file, await generator.Generate_DisplayAllVersionsMarkdown(model));

        file = Path.Combine(folderResults, $"MermaidVisualizerMajorDiffer.md");
        await File.WriteAllTextAsync(file, await generator.Generate_MermaidVisualizerMajorDiffer(model));

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



        }

        file = Path.Combine(folderResults, "_category_.json");
        string categoryGenerated = $$"""
{
  "label": "{{NameSolution}}",
  "position": 1,
  "link": {
    "type": "generated-index"
  }
}
""";
        await File.WriteAllTextAsync(file, categoryGenerated);


        file = Path.Combine(folderResults, "BuildingBlocks.md");
        await File.WriteAllTextAsync(file, await generator.Generate_BuildingBlocks(projectsDict));

        file = Path.Combine(folderResults, "TestProjects.md");
        await File.WriteAllTextAsync(file, await generator.Generate_TestProjects(projectsDict));

        file = Path.Combine(folderResults, "RootProjects.md");
        await File.WriteAllTextAsync(file, await generator.Generate_RootProjects(projectsDict));

        //file = Path.Combine(folderResults, "DisplayAllVersionsWithProblems.md");
        //ArgumentNullException.ThrowIfNull(projectsDict);
        //await File.WriteAllTextAsync(file, await generator.Generate_DisplayAllVersionsWithProblemsMarkdown(model));

        WriteLine($"now npm i && npm run start in  {where}");

    }
}
