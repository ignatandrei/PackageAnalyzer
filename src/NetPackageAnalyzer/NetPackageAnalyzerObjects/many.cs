namespace NetPackageAnalyzerObjects;
public class ExportPublicClass
{
    public string Name { get; set; } = "";
    public MethodPublic[] PublicMethods { get; set; } = [];
    public long LinesOfCode { get; set; } = 0;

}
public class MethodPublic
{
    public string MethodName { get; set; } = "";
    public long LinesOfCode { get; set; } = 0;
}
public class ClassesRefData
{
    public NamePerCount[]? AssembliesReferences { get; set; }
    public NamePerCount[]? MethodsReferences { get; set; }
    public NamePerCount[]? MethodWithMostReferences { get; set; }
    public NamePerCount[]? classRefs { get;  set; }
}
public class PublicClassRefData
{
    public Dictionary<string, ExportPublicClass[]> data;
    public NamePerCount[]? Assemblies_PublicClasses { get; set; }
    public NamePerCount[]? Assemblies_PublicMethods { get; set; }
    public NamePerCount[]? Class_PublicMethods { get; set; }

    public NamePerCount[]? PublicMethod_MostLinesOfCode { get; set; }
    public NamePerCount[]? Assemblies_MostLinesInPublicClass { get; set; }

    public NamePerCount[]? PublicClass_MostLinesOfCode { get; set; }

    public NamePerCount? PublicMethod_MostLinesOfCode_Most()
    {
        return PublicMethod_MostLinesOfCode?.OrderByDescending(x => x.Count).FirstOrDefault();
    }

}

public record NamePerCount(string Name, long Count)
{
    public string AdditionalData { get; set; } = Name;

}