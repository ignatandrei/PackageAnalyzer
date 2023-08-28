using NetPackageAnalyzerWork;

var folder=Environment.CurrentDirectory;
folder = @"C:\gth\TILT\src\backend\Net7\NetTilt";
WriteLine($"Start analyzing {folder}");
var p = new ProcessOutput();
string text;

text= p.OutputDotnet(folder, PackageOptions.Outdated);

var outdatedPackages = JsonSerializer.Deserialize<OutDated.outdatedV1_gen_json>(text);


text = p.OutputDotnet(folder, PackageOptions.Deprecated);

var deprecatedPackages = JsonSerializer.Deserialize<Deprecated.deprecatedV1_gen_json>(text);

text = p.OutputDotnet(folder, PackageOptions.Include_Transitive);
var allPackages = JsonSerializer.Deserialize<All.includeV1_gen_json>(text);
IOperations[] operations = new IOperations?[3]
{
    outdatedPackages,
    deprecatedPackages,
    allPackages
}
.Where(it=> it != null)
.Select(it=> it!)
.ToArray();

List<string> arrDataProjectsPath = new();
List<string> arrData=new();

foreach (var operation in operations)
{
    operation.ClearWrongData();
    arrDataProjectsPath.AddRange(operation.ProjectsPath());
    arrData.AddRange(operation.TopLevelPackagesIDs());
}

if(arrDataProjectsPath.Count == 0)
{
    WriteLine($"No projects in folder {folder}");
    return;
}
var projectsDict= arrDataProjectsPath.Distinct().ToDictionary(it=>it, it=>new ProjectData(it,folder));
WriteLine($"Number projects : {projectsDict.Count}");
//adding transitive packages
if (allPackages?.Frameworks()?.Length > 0)
{
    var newData = allPackages.TransitivePackagesIDs();
    if (newData?.Length > 0)
        arrData.AddRange(newData!);

}

var packagedDict = arrData.Distinct()
    .ToDictionary(it => it, it => new PackageData(it));


WriteLine($"Number references : {packagedDict.Count}");

var allProjectPathWithVersion = operations
    .SelectMany(it => it.PerProjectPathWithVersion())
    .ToArray();
if(allPackages != null)
    allProjectPathWithVersion= allProjectPathWithVersion
        .Union(allPackages.PerProjectPathWithVersionTransitive())
        .ToArray();

foreach (var pathPackage in allProjectPathWithVersion)
{
    var projData = projectsDict[pathPackage.Key];
    foreach (var package in pathPackage.Value)
    {
            var vers = packagedDict[package.PackageId].VersionsPerProject;
            if(!vers.ContainsKey(package.RequestedVersion))
                vers.Add(package.RequestedVersion, new ());
            vers[package.RequestedVersion].Add(projData);
    }
}

var folderResults = Path.Combine(folder, "Analysis");
if(!Directory.Exists(folderResults))
    Directory.CreateDirectory(folderResults);
DisplayDataMoreThan1Version model = new(packagedDict, folder);

TemplateGenerator generator=new();

var file = Path.Combine(folderResults, "DisplayMoreThan1Version.html");
await File.WriteAllTextAsync(file, await generator.Generate_DisplayMoreThan1Version(model));

file = Path.Combine(folderResults, $"MermaidVisualizerMajorDiffer.md");
await File.WriteAllTextAsync(file, await generator.Generate_MermaidVisualizerMajorDiffer(model));

