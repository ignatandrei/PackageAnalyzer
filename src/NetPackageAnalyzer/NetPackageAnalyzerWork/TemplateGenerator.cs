namespace NetPackageAnalyzerWork;

public class TemplateGenerator
{
    public async Task<string> Generate_DisplayMoreThan1Version(DisplayDataMoreThan1Version model)
    {
        var rz = new DisplayMoreThan1Version(model);
        return await rz.RenderAsync();

    }
    public async Task<string> Generate_MermaidVisualizerMajorDiffer(DisplayDataMoreThan1Version model)
    {
        var rz = new MermaidVisualizerMajorDiffer(model);
        return await rz.RenderAsync();

    }

}
