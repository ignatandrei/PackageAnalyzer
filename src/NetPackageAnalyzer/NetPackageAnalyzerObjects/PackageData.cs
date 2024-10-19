namespace NetPackageAnalyzerObjects;
public partial record PackageData(string packageVersionId): PackageGatherInfo(packageVersionId)
{
    public bool IsTest()
    {
        if (packageVersionId.ToLowerInvariant().StartsWith("microsoft.test"))
            return true;

        if (packageVersionId.ToLowerInvariant().StartsWith("nunit"))
            return true;
        if (packageVersionId.ToLowerInvariant().StartsWith("xunit"))
            return true;
        if (packageVersionId.ToLowerInvariant().StartsWith("microsoft.net.test"))
            return true;

        return false;
    }
    public Dictionary<string, HashSet<ProjectData>> VersionsPerProject { get; set; } = new();
    public TypePackageData typePackageData()
    {
        if (VersionsPerProject.Count < 2) return TypePackageData.OneVersion;
        return (MajorVersionDiffer()? TypePackageData.MultipleVersionMajorDiff: TypePackageData.MultipleVersionNotMajorDiff);
    }
    public string[] VersionsForProject(ProjectData projectData)
    {
        List<string> versions = new ();
        var relPath = projectData.RelativePath();
        foreach (var item in VersionsPerProject)
        {
            if (item.Value.Any(it=>it.RelativePath()==relPath))
            {
                versions.Add(item.Key);
            }
        }
        return versions.Order().ToArray();
    }
    public bool MajorVersionDiffer()
    {
        if(VersionsPerProject.Keys.Count < 2)return false;
        var vers=VersionsPerProject
            .Select(it=>it.Key)
            .Select(it =>
            {
                var indexDot = it.IndexOf(".");
                if (indexDot < 0) return it;
                return it.Substring(0, indexDot);
            })
            .ToHashSet();
        return (vers.Count >1) ;

    }
    public bool HasProblems() 
    {
        return VersionsPerProjectWithProblems.Count > 0;
    }
    public Dictionary<string, List<ProjectData>> VersionsPerProjectWithProblems { get; set; } = new();
    
    public NamePerCount[] Licenses()
    {
        var nugetInfo = new NugetInfoData(packageVersionId);
        Dictionary<string,int> Licences = new();
        foreach (var item in VersionsPerProject)
        {
            var res = nugetInfo.GetNugetInfoLicence(item.Key);
            string nameLic = "License not found";
            if (res.TryGetLicenseFound(out var lic))
            {
                nameLic=lic.license;
            }
            if (Licences.ContainsKey(nameLic))
            {
                Licences[nameLic]++;
            }
            else
            {
                Licences.Add(nameLic, 1);
            }
        }
        return Licences.Select(it=>
        {
            var ret= new NamePerCount(it.Key, it.Value);
            ret.AdditionalData = packageVersionId;
            return ret;
        })
        .ToArray();
    }
}
