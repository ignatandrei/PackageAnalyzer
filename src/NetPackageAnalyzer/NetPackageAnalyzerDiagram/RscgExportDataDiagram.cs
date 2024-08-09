namespace NetPackageAnalyzerDiagram;
public record RscgExportDataDiagram(string version, string pathToGenerate)
{
    public string GenerateCode()
    {        
       var data = new Templates.RSCGExportDiagramPowershell(this);
       return data.Render();
    }
}
