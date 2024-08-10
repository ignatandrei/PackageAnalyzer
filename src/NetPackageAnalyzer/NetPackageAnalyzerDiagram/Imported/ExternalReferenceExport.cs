namespace RSCG_ExportDiagram;

public class ExternalReferenceExport
{
    public string Name { get; set; } = "";
    public string FullName { get; set; } = "";
    public string TypeName { get; set; } = "";
    public string AssemblyName { get; set; } = "";

    public string FullClassName()
    {
        return AssemblyName + "." + TypeName;
    }
}

