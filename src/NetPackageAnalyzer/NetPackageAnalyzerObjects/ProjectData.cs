﻿namespace NetPackageAnalyzerObjects;

public partial record ProjectData(string PathProject, string folderSolution)
{
    public HistoryPerYear? AllHistoryFile { get; set; }
    public HistoryPerYear? AllHistoryFolder { get; set; }
    public CommitsData CommitsData { get; internal set; } = CommitsData.EmptySingleton;

    public History AllHistoryFileYear(int year)
    {
        return AllHistoryFile?.HistoryYear(year)??History.Empty;
    }
    public History AllHistoryFolderYear(int year)
    {

        return AllHistoryFolder?.HistoryYear(year) ?? History.Empty;
    }
    public List<ProjectData> ProjectsReferences { get; set; }=new();

    public List<ProjectData> UpStreamProjectReferences { get; set; } = new();

    public int FindReferenceRec(ProjectData project)
    {
        return FindRefRecursive(ProjectsReferences.ToArray(), project, 1);
    }
    public int TotalReferences()
    {
        return TotalReferences(ProjectsReferences.ToArray());
    }
    public int TotalReferences(ProjectData[] projects)
    {
        int total = 0;
        foreach (var item in projects)
        {
            total++;
            total += TotalReferences(item.ProjectsReferences.ToArray());
        }
        return total;
    }
    private int FindRefRecursive(ProjectData[] upRefs,ProjectData child,int nr)
    {
        foreach (var item in upRefs)
        {
            if (item == child)
                return nr;
       }
        List<int> lst= [0];
        foreach (var item in upRefs)
        {
            var res = FindRefRecursive(item.ProjectsReferences.ToArray(), child, nr + 1);
            lst.Add(res);
        }
        //just the first one
        if (lst.Count ==1) return 0;
        lst.Remove(0);
        return lst.Order().First();
    }

    public List<PackageData> Packages { get; set; }=new();

    public Dictionary<string, NamePerCount[]> LicNames()
    {
        Dictionary<string, NamePerCount[]> ret = new();
        foreach (var item in Packages)
        {
            var res = item.Licenses();
            foreach (var lic in res)
            {
                if (ret.ContainsKey(lic.Name))
                    ret[lic.Name] = ret[lic.Name].Append(lic).ToArray();
                else
                    ret[lic.Name] = new NamePerCount[] { lic };
            }
        }
        return ret;
    }
    public NamePerCount[] NamePerCountLicences(string nameLicence)
    {
        Dictionary<string,long>  ret = new();
        foreach (var item in Packages)
        {
            var res = item.Licenses();
            foreach (var lic in res)
            {
                if (lic.Name != nameLicence)
                    continue;
                if(ret.ContainsKey(lic.AdditionalData))
                    ret[lic.AdditionalData] += lic.Count;
                else
                    ret[lic.AdditionalData] = lic.Count;
            }
        }
        return ret.Select(it=>new NamePerCount(it.Key,it.Value)).ToArray();
    }
    public NamePerCount[] Licenses()
    {
        Dictionary<string, long> Licences = new();
        foreach (var item in Packages)
        {
            var res = item.Licenses();
            foreach (var lic in res)
            {
                if(Licences.ContainsKey(lic.Name))
                    Licences[lic.Name] += lic.Count;
                else
                    Licences[lic.Name] = lic.Count;


            }
        }
        return Licences.Select(it => new NamePerCount(it.Key, it.Value))
            .OrderBy(it=>it.Name)
            .ToArray();
    }
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