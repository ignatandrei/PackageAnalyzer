
namespace NetPackageAnalyzerDocusaurus;
internal class ClassesRefData
{
    public NamePerCount[]? AssembliesReferences { get; set; }
    public NamePerCount[]? MethodsReferences { get; set; }
    public NamePerCount[] MethodWithMostReferences { get; set; }
    public NamePerCount[] classRefs { get; internal set; }
}
