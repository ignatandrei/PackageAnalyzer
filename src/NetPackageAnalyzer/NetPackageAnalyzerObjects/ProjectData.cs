﻿namespace NetPackageAnalyzerObjects;

public partial record ProjectData(string PathProject, string folderSolution)
{
    public int nrCommitsFile { get; set; }
    public int nrCommitsFolder { get; set; }
    public DateTime? LastCommit { get; set; }
    public DateTime? FirstCommit { get; set; }

    public TimeSpan? DiffCommits
    {
        get{
            
            if(LastCommit == null || FirstCommit == null)
                return null;

            return LastCommit - FirstCommit;
        }
    }
    public int? CommitsPerMonth
    {

        get
        {
            if (LastCommit == null || FirstCommit == null)
                return null;

            var diff = DiffCommits;
            if (diff == null)
                return null;
            var nrMonths = (int) diff.Value.TotalDays / 30;
            if (nrMonths == 0)
                nrMonths++;
            return (int)( nrCommitsFile   / nrMonths);
        }
    }

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