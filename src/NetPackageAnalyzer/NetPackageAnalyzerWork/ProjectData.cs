﻿namespace NetPackageAnalyzerConsole;

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