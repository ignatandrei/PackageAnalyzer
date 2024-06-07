using NetPackageAnalyzeHistory;
using NetPackageAnalyzerObjects;

namespace NetPackageAnalyzerObjects;
public partial class ProjectsDict : Dictionary<string, ProjectData>
{
    public ProjectsDict(Dictionary<string, ProjectData> data) : base(data)
    {

    }
    public long MaxPackages
    {
        get
        {
            return this.Values.Select(it => it.Packages.Count).Max();
        }
    }
    public long MaxUpStreamReferences
    {
        get
        {
            return this.
                Values
                .Select(it => it.UpStreamProjectReferences.Count)
                .Max();
        }
    }

    public long MaxReferences
    {
        get
        {
            return this.Values.Select(it => it.ProjectsReferences.Count).Max();
        }
    }
    public ProjectData[] TestsProjects
    {
        get
        {
            return this.Values
                .Where(it => it.Packages.Any(it=>it.IsTest()))
                .ToArray();
                ;
        }
    }
    public ProjectData[] ProjectsNoTest
    {
        get
        {
            var tests = this.TestsProjects;
            return this.Values
                .Where(it => !tests.Contains(it))
                .ToArray();
        }
    }
    public IOrderedEnumerable<ProjectData> ProjectByName()
    {
        return this.Values
        .OrderBy(it => it.NameCSproj());

    }
    //public IOrderedEnumerable<ProjectData> ProjectByCommitsFile()
    //{

    //        return this.Values
    //            .OrderBy(it => it.nr());

    //}
    //public IOrderedEnumerable<ProjectData> ProjectByCommitsFolder()
    //{

    //    return this.Values
    //        .OrderBy(it => it.nrCommitsFolder);

    //}
    public ProjectData[] BuildingBlocks(int nrReferences)
    {
        var ret = this.ProjectsNoTest
            .Where(it=>it.ProjectsReferences.Count==nrReferences)
            .OrderBy(it => it.NameCSproj())
            .ToArray();    
        return ret;
    }
    public ProjectData[] RootProjects
    {
        get
        {
            List<ProjectData> result = new();
            var allRefs = this
                .SelectMany(it => it.Value.ProjectsReferences)
                .Select(it => it.RelativePath())
                .Distinct()
                .ToArray();
            if (allRefs.Length == 0)
            {
                return this.ProjectsNoTest
                    .OrderBy(it => it.NameCSproj()).ToArray();
            }

            var data =
                this.ProjectsNoTest
                    .Select(it => it)
                    .ToArray();

            var q = data!.ExceptBy(allRefs, it => it.RelativePath())
                .OrderBy(it => it.NameCSproj())
                    .ToArray();
            if (q == null)
            {
                return [];
            }
            return q;
        }
    }
    public ProjectData[] AlphabeticOrderedProjectsNoTests
    {
        get
        {
            return this.Values
                .Where(it=>it.IsTestProject()==false)
                .OrderBy(it => it.NameCSproj())
                .ToArray();
        }
    }
    public ProjectData[] AlphabeticOrderedProjects
    {
        get
        {
            return this.Values.OrderBy(it => it.NameCSproj()).ToArray();
        }
    }
    public void FindUpStreamReferences()
    {
        foreach (var project in this.Values)
        {
            foreach (var refProject in project.ProjectsReferences)
            {
                refProject.UpStreamProjectReferences.Add(project);
            }
        }
    }
    public void FindHistoryProjects()
    {
        foreach (var project in this.Values)
        {
            FileFolderHistorySimple fileHistorySimple = new(project.PathProject);
            fileHistorySimple.Initialize(true);
            project.nrCommitsFile = fileHistorySimple.numberCommitsFile??0;
            project.nrCommitsFolder = fileHistorySimple.numberCommitsFolder.GetValueOrDefault(-1);
            project.FirstCommitFile = fileHistorySimple.FirstCommitFile;
            project.FirstCommitFolder = fileHistorySimple.FirstCommitFolder;
            project.LastCommitFile = fileHistorySimple.LastCommitFile;
            project.LastCommitFolder = fileHistorySimple.LastCommitFolder;
            //Console.WriteLine("!done with " + project.PathProject + $"{project.LastCommitFolder} {project.LastCommitFile}");
        }
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
                    .Where(it => string.Equals( it.Value.NameCSproj() , nameCsproj,StringComparison.InvariantCultureIgnoreCase))
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

