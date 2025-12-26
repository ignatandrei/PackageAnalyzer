
namespace DotnetWhyParserObjects;

public class DotnetWhyOutput
{
    public string TargetPackage { get; set; } = string.Empty;
    public List<ProjectDependency> Projects { get; set; } = new();
    public string[] ProjectNames()
    {
        var ret= Projects.Select(it=>it.ProjectName).Distinct().ToArray();
        return ret;
    }
    public Tuple<string,string>[] ProjectNamesWithVersionPackage()
    {
        List<Tuple<string, string>> ret = new();
        foreach (var it in SmallestProjectPathsToPackage())
        {
            var node = it;
            string version = "";
            while (node != null)
            {
                version = node.Version;
                node = node.Dependency;
            }
            ret.Add(Tuple.Create(it.MyName, version));
        }
        return ret.ToArray();
    }
    public Dictionary<string, HashSet<string>> GetVersionsByProject()
    {
        var result = new Dictionary<string, HashSet<string>>();
        
        foreach (var project in Projects)
        {
            if (!project.HasDependency)
                continue;
                
            var versions = new HashSet<string>();
            foreach (var graph in project.DependencyGraphs)
            {
                CollectVersions(graph, versions);
            }
            
            if (versions.Count > 0)
            {
                result[project.ProjectName] = versions;
            }
        }
        
        return result;
    }
    
    private void CollectVersions(DependencyNode? node, HashSet<string> versions)
    {
        if (node == null) return;
        if (node.Name == TargetPackage && !string.IsNullOrEmpty(node.Version))
        {
            versions.Add(node.Version);
        }
        CollectVersions(node.Dependency, versions);
        
    }
    
    public DependencyNode[] AllChildren()
    {
        return Projects
            .SelectMany(it=>it.AllChildren())
            .ToArray();
    }
    public DependencyNode[] SmallestProjectPathsToPackage()
    {
        List<DependencyNode> small = new();
        foreach(var project in Projects)
        {
            if (!project.HasDependency) continue;
            var packages = new HashSet<string>();
            small.AddRange(project.SmallestProjectPathsToPackage());
        }
        return small
            .DistinctBy(it => it.Name)
            .ToArray();
    }
    internal void ConsolidateProjectAndPackages()
    {
        var namesProjects = this.Projects.Select(it => it.ProjectName).Distinct().ToArray();
        foreach(var prj in this.Projects)
        {
            if(!prj.HasDependency) continue;

            foreach (var graph in prj.DependencyGraphs)
            {
                Verify(graph, namesProjects);
            }
        }
    }

    private void Verify(DependencyNode? graph, string[] namesProjects)
    {
        if(graph == null) return;
        //already parsed
        if(graph.typeDependencyNode!= TypeDependencyNode.None) return;
        var name= graph.Name;
        if (namesProjects.Contains(name))
        {
            graph.typeDependencyNode = TypeDependencyNode.Project;
        }
        else
        {
            graph.typeDependencyNode= TypeDependencyNode.Package;
        }
        Verify(graph.Dependency, namesProjects);
    }
}
