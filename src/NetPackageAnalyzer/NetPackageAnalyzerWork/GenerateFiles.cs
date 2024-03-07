namespace NetPackageAnalyzerConsole;

public class GenerateFiles
{
    Dictionary<string, PackageData> packagedDict=new();
    ProjectsDict? projectsDict;
    public async Task<bool> GenerateData(string folder)
    {
        
        await Task.Delay(100);
        WriteLine($"Start analyzing {folder}");
        var p = new ProcessOutput();
        var build = p.Build(folder);
        if(!build)
        {
            WriteLine($"cannot build solution from {folder}");
            return false;
        }

        string text;

        text = p.OutputDotnetPackage(folder, PackageOptions.Outdated);

        var outdatedPackages = JsonSerializer.Deserialize<OutDated.outdatedV1_gen_json>(text);

        
        text = p.OutputDotnetPackage(folder, PackageOptions.Deprecated);

        var deprecatedPackages = JsonSerializer.Deserialize<Deprecated.deprecatedV1_gen_json>(text);

        text = p.OutputDotnetPackage(folder, PackageOptions.Include_Transitive);
        var allPackages = JsonSerializer.Deserialize<All.includeV1_gen_json>(text);
        IOperations[] operations = new IOperations?[3]
        {
    outdatedPackages,
    deprecatedPackages,
    allPackages
        }
        .Where(it => it != null)
        .Select(it => it!)
        .ToArray();

        List<string> arrDataProjectsPath = new();
        List<string> arrData = new();

        foreach (var operation in operations)
        {
            operation.ClearWrongData();
            arrDataProjectsPath.AddRange(operation.ProjectsPath());
            arrData.AddRange(operation.TopLevelPackagesIDs());
        }

        if (arrDataProjectsPath.Count == 0)
        {
            WriteLine($"No projects in folder {folder}");
            return false;
        }
        projectsDict =new ProjectsDict(
            arrDataProjectsPath
            .Distinct()
            .ToDictionary(it => it, it => new ProjectData(it, folder))
            ); 

        WriteLine($"Number projects : {projectsDict.Count}");
        projectsDict.FindReferences();    
        projectsDict.FindUpStreamReferences();
        //adding transitive packages
        if (allPackages?.Frameworks()?.Length > 0)
        {
            var newData = allPackages.TransitivePackagesIDs();
            if (newData?.Length > 0)
                arrData.AddRange(newData!);

        }

        packagedDict = arrData.Distinct()
            .ToDictionary(it => it, it => new PackageData(it));


        WriteLine($"Number references : {packagedDict.Count}");

        var allProjectPathWithVersion = operations
            .SelectMany(it => it.PerProjectPathWithVersion())
            .ToArray();
        if (allPackages != null)
            allProjectPathWithVersion = allProjectPathWithVersion
                .Union(allPackages.PerProjectPathWithVersionTransitive())
                .ToArray();

        foreach (var pathPackage in allProjectPathWithVersion)
        {
            var projData = projectsDict[pathPackage.Key];
            foreach (var package in pathPackage.Value)
            {
                var vers = packagedDict[package.PackageId].VersionsPerProject;
                if (!vers.ContainsKey(package.RequestedVersion))
                    vers.Add(package.RequestedVersion, new());
                var relPath= projData.RelativePath();
                if (!vers[package.RequestedVersion].Any(item=>item.RelativePath()== relPath))
                {
                    vers[package.RequestedVersion].Add(projData);

                }
            }
        }

        var problems=
            outdatedPackages!.PerProjectPathWithVersion()
            .Union(deprecatedPackages!.PerProjectPathWithVersion())
            .ToArray();

        foreach (var pathPackage in problems)
        {
            var projData = projectsDict[pathPackage.Key];
            foreach (var package in pathPackage.Value)
            {
                var vers = packagedDict[package.PackageId].VersionsPerProjectWithProblems;
                if (!vers.ContainsKey(package.RequestedVersion))
                    vers.Add(package.RequestedVersion, new());
                vers[package.RequestedVersion].Add(projData);
            }
        }
        //now we have all data
        //add to the projects the package references
        foreach (var item in this.projectsDict)
        {
            var pathProject = item.Value.RelativePath();
            foreach (var package in packagedDict.Values)
            {
                foreach (var vers in package.VersionsPerProject)
                {
                    foreach (var proj in vers.Value)
                    {
                        if (proj.RelativePath() == pathProject)
                        {
                            item.Value.Packages.Add(package);                            
                        }
                    }
                }
            }
        }
        return true;
    }
    public async Task GenerateNow(string folder)
    {
        
        var folderResults = Path.Combine(folder, "Analysis");
        WriteLine($"generate in {folderResults}");
        if (!Directory.Exists(folderResults))
            Directory.CreateDirectory(folderResults);
        DisplayDataMoreThan1Version model = new(packagedDict, folder);

        TemplateGenerator generator = new();
 
        var file = Path.Combine(folderResults, "DisplayAllVersions.html");
        await File.WriteAllTextAsync(file, await generator.Generate_DisplayAllVersions(model));

        file = Path.Combine(folderResults, "DisplayAllVersions.md");
        await File.WriteAllTextAsync(file, await generator.Generate_DisplayAllVersionsMarkdown(model));
         
        file = Path.Combine(folderResults, $"MermaidVisualizerMajorDiffer.md");
        await File.WriteAllTextAsync(file, await generator.Generate_MermaidVisualizerMajorDiffer(model));

        file = Path.Combine(folderResults, "ProjectRelation.md");
        ArgumentNullException.ThrowIfNull(projectsDict);
        await File.WriteAllTextAsync(file, await generator.Generate_ProjectsRelations(projectsDict));

        var folderProjects= Path.Combine(folderResults, "Projects");
        if (!Directory.Exists(folderProjects))
            Directory.CreateDirectory(folderProjects);
        foreach (var projData in projectsDict.AlphabeticOrderedProjects)
        {
            var folderProject = Path.Combine(folderProjects, projData.NameCSproj());
            if (!Directory.Exists(folderProject))
                Directory.CreateDirectory(folderProject);
            
            file= Path.Combine(folderProject, "ProjectReferences.md");
            await File.WriteAllTextAsync(file, await generator.Generate_ProjectRelations(projData));

            file = Path.Combine(folderProject, "Packages.md");
            await File.WriteAllTextAsync(file, await generator.Generate_ProjectPackages(projData));



        }
        //file = Path.Combine(folderResults, "DisplayAllVersionsWithProblems.md");
        //ArgumentNullException.ThrowIfNull(projectsDict);
        //await File.WriteAllTextAsync(file, await generator.Generate_DisplayAllVersionsWithProblemsMarkdown(model));


    }
}
