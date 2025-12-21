namespace NetPackageAnalyzerDiagram;
public record RscgExportDataDiagram(string version, string pathToGenerate)
{
    public  async Task< string> GenerateCode()
    {        
       var data = new Templates.RSCGExportDiagramPowershell(this);
       return await data.RenderAsync();
    }
}
