using NetPackageAnalyzeHistory;

namespace NetPackageAnalyzerObjects;

public partial record ProjectData(string PathProject, string folderSolution)
{
    public History? AllHistoryFile { get; set; }
    public History? AllHistoryFolder { get; set; }

    public List<ProjectData> ProjectsReferences { get; set; }=new();

    public List<ProjectData> UpStreamProjectReferences { get; set; } = new();

    public List<PackageData> Packages { get; set; }=new();
    
    public ProjectData[] AlphabeticalProjectsReferences()
    { 
        
        return ProjectsReferences.OrderBy(p => p.NameCSproj()).ToArray();
        
    }
    public ProjectData[] AlphabeticalUpStreamProjectReferences
    {
        get
        {
            return UpStreamProjectReferences.OrderBy(p => p.NameCSproj()).ToArray();
        }
    }
    public PackageData[] AlphabeticalProjectPackages
    {
        get
        {
            return Packages.OrderBy(p => p.packageVersionId).ToArray();
        }
    }

    public bool IsTestProject()
    {
        return Packages.Any(it => it.IsTest());
    }
    public string NameCSproj()
    {
        var indexDot=PathProject.LastIndexOf(".");
        var remains=PathProject.Substring(0, indexDot);
        var index1=remains.LastIndexOf("/");
        var index2=remains.LastIndexOf("\\");
        var index = Math.Max(index1, index2)+1;
        return remains.Substring(index);
    }
    public string FullNameMermaid()
    {
        var ret= $"{NameCSproj()}[{RelativePath()}]"; 
        ret=ret.Replace(@"[\","[");
        ret=ret.Replace(@"[/","[");
        return ret;
    }
    public string RelativePath()
    {
        return Path.GetRelativePath(folderSolution, PathProject);
        //var path1 = PathProject.Replace("/", "").Replace("\\", "");
        //var path2= folderSolution.Replace("/", "").Replace("\\", "");
        //if (path1.StartsWith(path2))
        //    return PathProject.Substring(folderSolution.Length);

        //return PathProject;
    }
}