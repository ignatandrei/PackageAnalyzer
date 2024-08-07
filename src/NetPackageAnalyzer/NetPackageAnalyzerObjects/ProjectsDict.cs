﻿using NetPackageAnalyzeHistory;
using NetPackageAnalyzerObjects;

namespace NetPackageAnalyzerObjects;
public partial class ProjectsDict : Dictionary<string, ProjectData>
{
    public ProjectsDict(Dictionary<string, ProjectData> data) : base(data)
    {

    }
    public long TotalCommits()
    {
        return this.Values
            .SelectMany(it => it.AllHistoryFolder!)
            .Sum(it=> (long)(it.Value?.nrCommits??0));
    }
    public int MedianCommits(int? year)
    {
        var data= this.Values.SelectMany(it => it.CommitsData!)
            .Where(it => year == null || it.date.Year == year)
            .SelectMany(it => it.Files)
            .GroupBy(it => it)
            .ToDictionary(it => it.Key, it => it.Count())
            .ToArray();
        if ((data?.Length??0) == 0) return 0;
        ArgumentNullException.ThrowIfNull(data);
        return Statistical<int>.Median(data.Select(it => it.Value).ToArray());

    }
    public KeyValuePair<string, int>[] FilesWithMaxCommits(int? year)
    {
        int take = 10;
        var data = this.Values.SelectMany(it => it.CommitsData!)
            .Where(it => year == null || it.date.Year == year)
            .SelectMany(it => it.Files)
            .GroupBy(it => it)
            .OrderByDescending(it => it.Count())
            .Take(take * 2)
            .ToDictionary(it => it.Key, it => it.Count())
            .ToArray();
        if(data.Length > take)
        {
            var nr = data[take].Value;
            data = data.Where(it => it.Value >= nr).ToArray();
        }

        return data;
    }
    public Dictionary<int, Commit> CommitsWithMaxFilesPerYear()
    {
        var data = this.Values
            .SelectMany(it => it.CommitsData!)
            .GroupBy(it => it.date.Year)
            .Select(it => new { year = it.Key, commit = it.ToArray() })
            .Where(it => it.commit.Length > 0)
            .ToDictionary(it => it.year, it => it.commit.MaxBy(it => it.CountFiles())!);
        return data;
    }
    public int MedianCommitsForFiles(int? year)
    {
        var data = this.Values
            .SelectMany(it => it.CommitsData??CommitsData.EmptySingleton)
            .Where(it => it != null)
            .Where(it => year == null || it.date.Year == year)
            .Select(it => it.CountFiles())
            .ToArray();
        var median=Statistical<int>.Median(data);
        return median;
    }
    public Commit[] MaxCommits(int? year)
    {
        int take = 10;
        var data = this.Values
            .SelectMany(it => it.CommitsData ?? CommitsData.EmptySingleton)
            .Where(it => it != null)
            .Where(it => year == null || it.date.Year == year)
            .OrderByDescending(it => it.CountFiles())
            .ToArray();
            ;

        if (data.Length < take+1)
            return data;

        var nrCommits = data[take].CountFiles();

        return data.Where(it=>it.CountFiles()>=nrCommits).ToArray();


    }
    public Dictionary<int, int> CommitsMedianFilesPerYear()
    {
        var data = this.Values
            .SelectMany(it => it.CommitsData!)
            .GroupBy(it => it.date.Year)
            .Select(it => new { 
                year = it.Key, 
                median = 
            Statistical<int>.Median( it.Select(it=>it.CountFiles()).ToArray())
            })            
            .ToDictionary(it => it.year, it => it.median)
            ;
        
        return data;
    }
    public Commit CommitsWithMaxFiles(int? year)
    {
        return this.Values
            .SelectMany(it => it.CommitsData!)
            .Where(it => year == null || it.date.Year == year.Value)
            .OrderByDescending(it => it.CountFiles())
            .Take(1)
            .First();
    }
    public Dictionary<int,long> CommitsPerYearFolder()
    {
        var data= this.Values
            .SelectMany(it => it.AllHistoryFolder!)
            .GroupBy(it => it.Key)
            .ToDictionary(it => it.Key, it => it.Sum(it => (long)(it.Value?.nrCommits ?? 0)))
            ;
        return data;

    }
    public Dictionary<int, long> CommitsPerYearFile()
    {
        var data = this.Values
            .SelectMany(it => it.AllHistoryFile!)
            .GroupBy(it => it.Key)
            .ToDictionary(it => it.Key, it => it.Sum(it => (long)(it.Value?.nrCommits ?? 0)))
            ;
        return data;

    }
    public long CommitsPerYearFolder(int year)
    {

       return this.Values
            .SelectMany(it => it.AllHistoryFolder!)
            .Where(it => it.Key == year)
            .Sum(it => (long)(it.Value?.nrCommits ?? 0));
    }
    public int MaxYearCommits()
    {
        return this.Values
            .Select(it => it.AllHistoryFolder)
            .Where(it => it != null)
            .Select(it => it!.MaxYear())
            .Max();

    }
    public int MinYearCommits()
    {
        return this.Values
            .Select(it => it.AllHistoryFolder)
            .Where(it => it != null)
            .Select(it => it!.MinYear())
            .Min();

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
            FolderHistoryCommits folderHistoryCommits = new(Path.GetDirectoryName(project.PathProject));
            folderHistoryCommits.Initialize();
            project.AllHistoryFile = fileHistorySimple.AllHistoryFile;
            project.AllHistoryFolder = fileHistorySimple.AllHistoryFolder;
            project.CommitsData = folderHistoryCommits.commit;
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

