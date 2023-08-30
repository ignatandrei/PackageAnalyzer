namespace NetPackageAnalyzerConsole;

public record DisplayDataMoreThan1Version(Dictionary<string, PackageData> IDPackageWithProjects, string folderName)
{
    public string Version = ThisAssembly.Info.Version;
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
