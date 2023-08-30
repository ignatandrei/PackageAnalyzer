namespace NetPackageAnalyzerWork;
public class ProjectsDict : Dictionary<string, ProjectData>
{
    public string Version = ThisAssembly.Info.Version;
    public ProjectsDict(Dictionary<string, ProjectData> data) : base(data)
    {

    }
    public void FindReferences()
    {
        ProcessOutput po = new();
        foreach (var project in this.Values)
        {
            var data = po.OutputDotnetReference(project.PathProject);
            data = data.Replace("\r\n", "\n");
            var lines = data.Split("\n");
            bool separator = false;
            var startFolder = Path.GetDirectoryName(project.PathProject)!;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("--"))
                {
                    separator = true;
                    continue;
                }
                if (!separator) continue;
                //from now start project references
                var csproj = lines[i].Trim();
                if (csproj.Length == 0) continue;
                var realCsproj = Path.Combine(startFolder, csproj);
                var nameCsproj = Path.GetFileNameWithoutExtension(csproj);
                var arr = this
                    .Where(it => it.Value.NameCSproj() == nameCsproj)
                    .ToArray();
                if (arr.Length == 0)
                {
                    throw new Exception("no csproj for " + csproj);
                }
                else if (arr.Length == 1)
                {
                    project.ProjectsReferences.Add(arr[0].Value);
                }
                else
                {
                    throw new Exception("multiple projects with same name, different folders");
                }

            }
        }
    }
}

