namespace NetPackageAnalyzerConsole;
public record PackageData(string packageVersionId)
{
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
}
