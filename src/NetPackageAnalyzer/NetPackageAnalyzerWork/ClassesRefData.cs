
namespace NetPackageAnalyzerDocusaurus;
internal class ClassesRefData
{
    public NamePerCount[]? AssembliesReferences { get; set; }
    public NamePerCount[]? MethodsReferences { get; set; }
    public NamePerCount[]? MethodWithMostReferences { get; set; }
    public NamePerCount[]? classRefs { get; internal set; }
}
internal class PublicClassRefData
{
    public Dictionary<string, ExportPublicClass[]> data;
    public NamePerCount[]? Assemblies_PublicClasses { get; set; }
    public NamePerCount[]? Assemblies_PublicMethods { get; set; }
    public NamePerCount[]? Class_PublicMethods { get; set; }
}