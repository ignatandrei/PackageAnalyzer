
namespace DotnetWhyParserObjects;

[DebuggerDisplay("{ProjectName} {TargetFramework} {HasDependency}")]
public class ProjectDependency
{
    private DependencyNode FromPackage(DependencyNode dependencyNode)
    {
        ArgumentNullException.ThrowIfNull(dependencyNode);
        if (dependencyNode!.typeDependencyNode != TypeDependencyNode.Package)
        {
            throw new InvalidOperationException("must be from package");
        }
        DependencyNode dn = new DependencyNode();
        dn.Name = this.ProjectName;
        dn.typeDependencyNode = TypeDependencyNode.Project;
        dn.Dependency= dependencyNode;
        return dn;
    }
    public DependencyNode[] SmallestProjectPathsToPackage()
    {
        List<DependencyNode> firstProjectThatPackageDependsTo = new();
        //a project can have a dep another project or package
        //the package can have JUST another package
        foreach(var graph in this.DependencyGraphs)
        {
            
            var currentNode = graph;
            while (currentNode != null)
            {
                
                if (currentNode.typeDependencyNode == TypeDependencyNode.None)
                    throw new InvalidOperationException("did you call ConsolidateProjectAndPackages?");

                if(currentNode.typeDependencyNode == TypeDependencyNode.Package)
                {
                    //already a package from the first time
                    firstProjectThatPackageDependsTo.Add(FromPackage(currentNode));
                    break;
                }
                
                if (currentNode.Dependency == null)
                    break;
                
                if (currentNode.Dependency.typeDependencyNode == TypeDependencyNode.Project)
                {
                    currentNode = currentNode.Dependency;
                    continue;
                }
                //the dependency is package,we found it
                firstProjectThatPackageDependsTo.Add(currentNode);
                break;
            }
        }
        return firstProjectThatPackageDependsTo
            .DistinctBy(it=>it.Name)
            .ToArray();
    }
    public string ProjectName { get; set; } = string.Empty;
    public string TargetFramework { get; set; } = string.Empty;
    public List<DependencyNode> DependencyGraphs { get; set; } = new();
    public DependencyNode[] AllChildren()
    {
        var ret = this.DependencyGraphs
                .SelectMany(it => it.AllChildren())
                .ToArray()
                ;

        return ret;
    
    }
    public string[] Names()
    {
        var ret= this.AllChildren()
                .Select(it => it.Name+","+it.Version)
                .Distinct()
                .ToList();
        ret.Add(this.ProjectName);
        return ret.ToArray();
    }
    public bool HasDependency { get; set; } = true;
}
