namespace DotnetWhyParserObjects;

public enum TypeDependencyNode
{
    None = 0,
    Package,
    Project
}
[DebuggerDisplay("{ToString()}")]
public class DependencyNode
{
    public TypeDependencyNode typeDependencyNode { get; set; } = TypeDependencyNode.None; 
    public List<DependencyNode> AllChildren()
    {
        var ret= new List<DependencyNode>();
        ret.Add(this);
        var dep = this.Dependency;
        while(dep != null)
        {
            ret.Add(dep);
            dep = dep.Dependency;
        }
        return ret; 
    }
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public DependencyNode? Dependency { get; set; } 
    public bool HasReachedPackage()
    {
        if (IsPackage()) return true;
        if(Dependency == null) return false;
        return Dependency!.HasReachedPackage();
    }
    public int FindLineNumberRecursive()
    {
        var lineNr = this.LineNumberForPackage;
        if (lineNr > 0) return lineNr;
        if(Dependency == null) return 0;
        return this.Dependency!.FindLineNumberRecursive();
    }


    public int LineNumberForPackage { get; set; } = 0;
    public bool IsPackage()=> LineNumberForPackage >0;
    
    public string MyName
    {
        get
        {
            return this.typeDependencyNode switch {
                TypeDependencyNode.Package => this.Name + "," + this.Version,
                TypeDependencyNode.Project =>this.Name ,
                _ => throw new NotImplementedException()
            };
        }
    }
    public override string ToString()
    {
        return $"{Name} ({Version})";
    }
}
