namespace NetPackageAnalyzerConsole;

public record DisplayDataMoreThan1Version(Dictionary<string, PackageData> IDPackageWithProjects, string folderName)
{
    public KeyValuePair<string,PackageData>[] Sorted()
    {
        return IDPackageWithProjects.OrderBy(it=>it.Key).ToArray();
    }
    public int NrPackages() { return IDPackageWithProjects.Count; }
    public string[] KeysPackageMultiple()
    {
        return IDPackageWithProjects.Where(it => it.Value.VersionsPerProject.Count > 1)
        .Select(it=>it.Key)
        .OrderBy(it => it).ToArray();
    }
    public string[] KeysPackageMultipleMajorDiffers()
    {
        return IDPackageWithProjects
            .Where(it => it.Value.VersionsPerProject.Count > 1)
            .Where(it=>it.Value.MajorVersionDiffer())
            .Select(it => it.Key)
            .OrderBy(it => it)
            .ToArray();
    }
    public int MajorVersionDiffers()
    {
        return IDPackageWithProjects.Count(it => it.Value.MajorVersionDiffer());
    }
}
public enum TypePackageData
{
    None=0,
    OneVersion=1,
    MultipleVersionNotMajorDiff=2,
    MultipleVersionMajorDiff = 3,
}
public record PackageData(string packageVersionId)
{
    public Dictionary<string, List<ProjectData>> VersionsPerProject { get; set; } = new();
    public TypePackageData typePackageData()
    {
        if (VersionsPerProject.Count < 2) return TypePackageData.OneVersion;
        return (MajorVersionDiffer()? TypePackageData.MultipleVersionMajorDiff: TypePackageData.MultipleVersionNotMajorDiff);
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
}

public record ProjectData(string PathProject, string folderSolution)
{
    public List<ProjectData> ProjectsReferences { get; set; }=new();
    public string NameCSproj()
    {
        var indexDot=PathProject.LastIndexOf(".");
        var remains=PathProject.Substring(0, indexDot);
        var index1=remains.LastIndexOf("/");
        var index2=remains.LastIndexOf("\\");
        var index = Math.Max(index1, index2)+1;
        return remains.Substring(index);
    }
    public string RelativePath()
    {
        var path1 = PathProject.Replace("/", "").Replace("\\", "");
        var path2= folderSolution.Replace("/", "").Replace("\\", "");
        if (path1.StartsWith(path2))
            return PathProject.Substring(folderSolution.Length);

        return PathProject;
    }
}