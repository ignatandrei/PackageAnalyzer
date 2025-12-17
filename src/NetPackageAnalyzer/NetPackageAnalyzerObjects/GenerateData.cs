using NPA.BigResources;

namespace NetPackageAnalyzerObjects;
public class GenerateData
{
    public GenerateData(IFileSystem system)
    {
        this.system = system;
    }
    protected string NameSolution = "";
    protected internal Dictionary<string, PackageData> packagedDict = new();
    protected internal ProjectsDict? projectsDict;
    protected readonly IFileSystem system;
    protected DisplayDataMoreThan1Version? modelMore1Version;
    protected PackageProblemsDTO packDTO = new();
    public InfoSolution infoSol
    {
        get
        {
            var nrOutdated = packDTO.outdated.GroupBy(it => it.PackageId).Count();
            var nrDeprecated = packDTO.deprecated.GroupBy(it => it.PackageId).Count();
            var nrVulnerable = packDTO.vulnerable.GroupBy(it => it.PackageId).Count();
            var infoSol = new InfoSolution(
                this.projectsDict!.Count,
                packagedDict.Count, nrOutdated, nrDeprecated,nrVulnerable,
                this.projectsDict!.TotalCommits(),
                this.projectsDict!.TestsProjects.Count(),
                modelMore1Version!.KeysPackageMultipleMajorDiffers().Length
                );
            return infoSol;
        }
    }
    public string[] MajorWithMoreVersions()
    {
        return modelMore1Version!.KeysPackageMultipleMajorDiffers();
    }
    public Dictionary<string,PackageData> MajorWithMoreVersionsPackages()
    {
        var res = new Dictionary<string, PackageData>();
        foreach (var item in MajorWithMoreVersions())
        {
            res.Add(item, packagedDict[item]);

        }
        return res;
    }

    public PackageWithVersion[] Problems()
    {
        var res= packDTO.All();
            
        foreach (var item in res)
        {
            item.Projects = packagedDict[item.PackageId].VersionsPerProjectWithProblems[item.RequestedVersion].ToArray();            
        }
        return res;
    }
    public async Task<bool> GenerateDataForSln(string folder)
    {
        var sln = system.Directory.GetFiles(folder, "*.sln*");
        if (sln.Length != 1)
        {
            WriteLine($"Must be 1 sln in the {folder}");
            //throw new ArgumentException($"Must be 1 sln in the {folder}");
            return false;
        }

        GlobalsForGenerating.FullPathToSolution = sln[0];
        NameSolution = system.Path.GetFileNameWithoutExtension(sln[0]);
        GlobalsForGenerating.NameSolution = NameSolution;
        await Task.Delay(100);
        WriteLine($"Start analyzing {folder} for solution {NameSolution}");
        var p = new ProcessOutput();
        Console.WriteLine("List SDKs");
        var listSDKs = p.ListSDKS(folder);
        WriteLine(listSDKs);
        var build = p.Build(folder);
        if (!build)
        {
            WriteLine($"cannot build solution from {folder}");
            return false;
        }

        string text;
        OutDated.outdatedV1_gen_json? outdatedPackages = null;
        Deprecated.deprecatedV1_gen_json? deprecatedPackages = null;
        Vulnerable.vulnerablev1_gen_json? vulnerablePackages= null;
        All.includeV1_gen_json? allPackages = null;
        try
        {
            text = p.OutputDotnetPackage(folder, PackageOptions.Outdated);

            outdatedPackages = JsonSerializer.Deserialize<OutDated.outdatedV1_gen_json>(text);


            text = p.OutputDotnetPackage(folder, PackageOptions.Deprecated);

            deprecatedPackages = JsonSerializer.Deserialize<Deprecated.deprecatedV1_gen_json>(text);

            text = p.OutputDotnetPackage(folder, PackageOptions.Include_Transitive);
            allPackages = JsonSerializer.Deserialize<All.includeV1_gen_json>(text);

            text = p.OutputDotnetPackage(folder, PackageOptions.Vulnerable);
            vulnerablePackages = JsonSerializer.Deserialize<NS_GeneratedJson_vulnerablev1_gen_json.vulnerablev1_gen_json>(text);
        }
        catch (ExceptionReadingPackages ex)
        {
            WriteLine($"Error reading packages! {ex.Message}");
        }
        catch (Exception ex)
        {
            WriteLine($"Error! {ex.Message}");
            WriteLine("please run the latest dotnet command");
            WriteLine("and , if possible, make an issue at https://github.com/ignatandrei/packageAnalyzer/issues with the result");
            return false;
        }
        if (outdatedPackages?.TopLevelPackagesIDs()?.Length > 0)
        {
            packDTO.outdated = outdatedPackages
                .TopLevelPackages()
                .Where(it => it != null)
                .Select(it => it!)
                .Where(it => it.Id != null && it.RequestedVersion !=null)
                .Select(it=> new PackageWithVersionOutdated(it.Id??"",it.RequestedVersion??""))
                .ToArray();
        }
        if(deprecatedPackages?.TopLevelPackagesIDs()?.Length > 0)
        {
            packDTO.deprecated = deprecatedPackages
                .TopLevelPackages()
                .Where(it=>it!=null)
                .Select(it=>it!)
                .Where(it => it.Id != null && it.RequestedVersion != null)
                .Select(it => new PackageWithVersionDeprecated(it.Id??"", it.RequestedVersion ?? ""))
                .ToArray();
        }

        if (vulnerablePackages?.TopLevelPackagesIDs()?.Length > 0)
        {
            packDTO.vulnerable = vulnerablePackages
                .TopLevelPackages()
                .Where(it => it != null)
                .Select(it => it!)
                .Where(it => it.Id != null && it.RequestedVersion != null)
                .Select(it => new PackageWithVersionVulnerable(it.Id ?? "", it.RequestedVersion ?? ""))
                .ToArray();
        }
        build = p.Build(folder);
        packDTO.VerifyWhy();

        IOperations[] operations = new IOperations?[4]
        {
    outdatedPackages,
    deprecatedPackages,
    vulnerablePackages,
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
            arrDataProjectsPath = p.ListOfProjects(GlobalsForGenerating.FullPathToSolution).ToList();
        }
            if (arrDataProjectsPath.Count == 0)
        {
            //obtain list of projects
            
            WriteLine($"No projects in folder {folder}");
            return false;
        }


        projectsDict = new ProjectsDict(
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
                var relPath = projData.RelativePath();
                if (!vers[package.RequestedVersion].Any(item => item.RelativePath() == relPath))
                {
                    vers[package.RequestedVersion].Add(projData);

                }
            }
        }

        var problems =
            (outdatedPackages?.PerProjectPathWithVersion() ?? [])
            .Union((deprecatedPackages?.PerProjectPathWithVersion() ?? []))
            .Union((vulnerablePackages?.PerProjectPathWithVersion() ?? []))
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
        this.modelMore1Version= new(packagedDict, folder);
        return true;
    }

    public void AddHistoryCsproj()
    {
        ArgumentNullException.ThrowIfNull(projectsDict);
        projectsDict.FindHistoryProjects();
        //foreach (var item in projectsDict)
        //{
        //    Console.WriteLine($"History for {item.Value.PathProject} is {item.Value.nrCommits}"); ;
        //}
    }

    public (ClassesRefData, PublicClassRefData, AssemblyDataFromMSFT) AnalyzeDiagrams(string tempFolder)
    {
        var xmlFiles = Directory.GetFiles(tempFolder, "*.xml");
        List<GenericMetricsAssembly> msftMetrics = new();
        foreach (var file in xmlFiles)
        {
            var data= GenericMetrics.CreateFromXML(file);
            if (data.Length > 0)
            {
                foreach (var metric in data)
                {
                    msftMetrics.Add((GenericMetricsAssembly)metric);
                }
            }
        }
        AssemblyDataFromMSFT assemblyMSFTData =new ([.. msftMetrics]);

        List<ExportAssembly> expAss = new ();
        Dictionary<string,ExportPublicClass[]> expPublicClasses = new ();
        var files = Directory.GetFiles(tempFolder, "*.json");
        foreach (var fileJson in files)
        {

            var json = File.ReadAllText(fileJson);
            if (fileJson.EndsWith("_public_csproj.json"))
            {
                string nameCsproj = Path.GetFileNameWithoutExtension(fileJson);
                nameCsproj = nameCsproj.Replace("_public_csproj", "");
                ExportPublicClass[]? ex = JsonSerializer.Deserialize<ExportPublicClass[]>(json);
                if(ex!=null)expPublicClasses.Add(nameCsproj, ex);
                continue;
            }
            //if (fileJson.Contains("_rel_")) 
            {
                ExportAssembly? ex = JsonSerializer.Deserialize<ExportAssembly>(json);
                if (ex == null)
                    continue;
                expAss.Add(ex);
                continue;
            }

        }
        PublicClassRefData publicClassRefData = new();
        publicClassRefData.data = expPublicClasses;

        publicClassRefData.PublicMethod_MostLinesOfCode = expPublicClasses
            .SelectMany(it => it.Value)
            .SelectMany(it => it.PublicMethods)
            .Select(it => new NamePerCount(it.MethodName, it.LinesOfCode))
            .OrderByDescending(it => it.Count)
            .ToArray();

        publicClassRefData.PublicClass_MostLinesOfCode = expPublicClasses
            .Values
            .SelectMany(it => it)
            .Select(it=>new NamePerCount(it.Name,it.LinesOfCode))
            .OrderByDescending(it => it.Count)
            .ToArray();

        publicClassRefData.Assemblies_MostLinesInPublicClass = expPublicClasses
            .Select(it => new NamePerCount(it.Key, it.Value.Sum(c => c.LinesOfCode)))
            .OrderByDescending(it => it.Count)
            .ToArray();

        publicClassRefData.Assemblies_PublicClasses= expPublicClasses
                .Select(it => new NamePerCount(it.Key, it.Value.Length))
                .OrderByDescending(it => it.Count)
                .ToArray();

        publicClassRefData.Assemblies_PublicMethods = expPublicClasses
            .Select(it => new NamePerCount(it.Key, it.Value.SelectMany(c => c.Name).Count()))
            .OrderByDescending(it => it.Count)
            .ToArray();

        publicClassRefData.Class_PublicMethods = expPublicClasses
            .SelectMany(it => it.Value)
            .Select(it => new NamePerCount(it.Name, it.PublicMethods.Length))
            .OrderByDescending(it => it.Count)
            .ToArray();
        ;
        var classRefs = expAss
            .SelectMany(it => it.ClassesWithExternalReferences)
            .Select(it => new NamePerCount(it.ClassName, 
                it.MethodsWithExternalReferences
                .SelectMany(m => m.References)
                .Count()
                ))
            .OrderByDescending(it => it.Count)
            .ToArray()
            ;

        var methExt = expAss
            .SelectMany(it => it.ClassesWithExternalReferences)
            .SelectMany(it => it.MethodsWithExternalReferences)
            .ToArray();
        var allExtReferences = methExt
            .SelectMany(it=>it.References)
            .ToArray();
        
        var maxRefAssembly = allExtReferences
            .GroupBy(it=>it.AssemblyName)
            .Select(it => new NamePerCount (it.Key, it.Count()))
            .OrderByDescending(it => it.Count)
            .Where(it => it.Count > 0)
            .ToArray();

        var maxRefMethods = allExtReferences
            .GroupBy(it => it.FullName)
            .Select(it => new NamePerCount(it.Key, it.Count()))
            .OrderByDescending(it => it.Count)
            .Where(it => it.Count > 0)
            .Select(it =>
            {
                var dot= it.Name.LastIndexOf('.');
                if(dot<0)
                    return it;
                return new NamePerCount(it.Name.Substring(dot+1), it.Count);
            })
            .ToArray();

        var methodsWithRefs = methExt
            .Select(it => new NamePerCount(it.MethodName, it.References.Length))
            .OrderByDescending(it => it.Count)
            .Where(it => it.Count > 0)
            .ToArray()
            ;


        ClassesRefData ret = new();
        ret.MethodsReferences = maxRefMethods;
        ret.classRefs = classRefs;
        ret.AssembliesReferences = maxRefAssembly;
        ret.MethodWithMostReferences = methodsWithRefs;
        return (ret,publicClassRefData,assemblyMSFTData);
    }

    protected async Task<string?> GenerateDocsForClasses(string?[] projects, string folderResults)
    {

        var fldTemp = folderResults + "_Temp";
        if (!Directory.Exists(fldTemp))
            Directory.CreateDirectory(fldTemp);
        var temp = Path.GetTempPath();
        var pathZip = Path.Combine(temp, "metrics" + DateTime.Now.ToString("yyyyMMdd"));
        await ZipBigFiles.SaveToFile(pathZip,EmbeddedResource.metrics_exe_zip);
        foreach (var proj in projects)
        {
            if (string.IsNullOrWhiteSpace(proj))
                continue;
            Console.WriteLine($"Calculating code metrics for {proj}");
            string nameProj = Path.GetFileNameWithoutExtension(proj);
            string outputFile = Path.Combine(fldTemp, nameProj + ".xml");
            ProcessStartInfo startInfo = new()
            {
                FileName =Path.Combine(pathZip, "Metrics.exe"),
                WorkingDirectory = pathZip,
                Arguments = $"/p:{proj} /out:{outputFile}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Console.WriteLine($"executing in folder {startInfo.WorkingDirectory} : {startInfo.FileName} with args {startInfo.Arguments}");
            using (Process process = new Process())
            {
                try
                {
                    process.StartInfo = startInfo;
                    process.Start();
                    await process.WaitForExitAsync();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(error))
                        throw new ArgumentException($"error for {proj} metrics {error}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

            }
        }
        return fldTemp;
            //TODO: relation metrics,  metrics.exe
            //string fldTemp = string.Empty;
            //try
            //{
            //    var folder = Path.GetDirectoryName(fullPathToSolution)??"";
            //    fldTemp = folderResults + "_Temp";
            //    if (!Directory.Exists(fldTemp))
            //        Directory.CreateDirectory(fldTemp);
            //    RscgExportDataDiagram pwsh = new("2024.904.427", fldTemp);
            //    var code = pwsh.GenerateCode();
            //    var file = Path.Combine(folder, "ExportDiagram.ps1");
            //    File.WriteAllText(file, code);
            //    Console.WriteLine("Please wait - generate diagram classes");
            //    ProcessStartInfo startInfo = new ()
            //    {
            //        FileName = "powershell.exe",
            //        WorkingDirectory = folder,
            //        Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{file}\"",
            //        RedirectStandardOutput = true,
            //        RedirectStandardError = true,
            //        UseShellExecute = false,
            //        CreateNoWindow = true
            //    };

            //    using (Process process = new Process())
            //    {
            //        process.StartInfo = startInfo;
            //        process.Start();

            //        string output = process.StandardOutput.ReadToEnd();
            //        string error = process.StandardError.ReadToEnd();

            //        process.WaitForExit();

            //        if (process.ExitCode != 0)
            //        {
            //            Console.WriteLine($"PowerShell Error: {error}");
            //        }
            //        else
            //        {
            //            Console.WriteLine(output);
            //        }
            //        return fldTemp;

//        }
//}
        //catch (Exception ex)
        //{
        //    Console.WriteLine("Exception!! " + ex.Message);
        //    Console.WriteLine("Exception!! " + ex.StackTrace);
        //    return null;
        //}
        
    }
}
