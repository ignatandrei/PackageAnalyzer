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
    public Dictionary<string, ExportPublicClass[]> data=[];
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
public record NamePerCountArray(NamePerCount[]? NamePerCounts,bool Descending)
{ 
    public Statistics<long> Data()
    {
        return new Statistics<long>(NamePerCounts?.Select(it => it.Count).ToArray() ?? []);
    }
    public NamePerCount? First1()
    {
        if (Descending)
            return Max1();
        else
            return Min1();
    }
    public NamePerCount[] DataOrdered()
    {
        if(Descending)
            return OrderedDesc();
        else
            return OrderedAsc();
    }
    private NamePerCount? Max1()
    {
        if ((NamePerCounts?.Length ?? 0) == 0)
            return null;
        
        ArgumentNullException.ThrowIfNull(NamePerCounts);

        return this.NamePerCounts
            .OrderByDescending(it => it.Count)
            .FirstOrDefault();
    }
    private NamePerCount? Min1()
    {
        if ((NamePerCounts?.Length ?? 0) == 0)
            return null;

        ArgumentNullException.ThrowIfNull(NamePerCounts);

        return this.NamePerCounts
            .OrderBy(it => it.Count)
            .FirstOrDefault();
    }
    private NamePerCount[] OrderedDesc()
    {
        if ((NamePerCounts?.Length ?? 0) == 0)
            return [];

        ArgumentNullException.ThrowIfNull(NamePerCounts);

        return this.NamePerCounts
            .OrderByDescending(it => it.Count)
            .ThenBy(it => it.Name)
            .ToArray();
    }
    private NamePerCount[] OrderedAsc()
    {
        if ((NamePerCounts?.Length ?? 0) == 0)
            return [];

        ArgumentNullException.ThrowIfNull(NamePerCounts);

        return this.NamePerCounts
            .OrderBy(it => it.Count)
            .ThenBy(it => it.Name)
            .ToArray();
    }
}