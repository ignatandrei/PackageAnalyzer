

namespace NetPackageAnalyzerDocusaurus;
public class TemplateGenerator
{
    public async Task<string> Generate_DisplayAllVersions(DisplayDataMoreThan1Version model)
    {
        var rz = new DisplayAllVersions(model);
        return await rz.RenderAsync();
    }

    public async Task<string?> Generate_DisplayAllVersionsMarkdown(DisplayDataMoreThan1Version model)
    {
        var rz = new DisplayAllVersionsForMarkdown(model);
        return await rz.RenderAsync();

    }
    //public async Task<string?> Generate_DisplayAllVersionsWithProblemsMarkdown(DisplayDataMoreThan1Version model)
    //{
    //    var rz = new DisplayAllVersionsProblemsForMarkdown(model);
    //    return await rz.RenderAsync();

    //}
    public async Task<string> Generate_MermaidVisualizerMajorDiffer(DisplayDataMoreThan1Version model)
    {
        var rz = new MermaidVisualizerMajorDiffer(model);
        return await rz.RenderAsync();
    }
    public async Task<string> Generate_ProjectsRelations(ProjectsDict model)
    {
        var rz = new ProjectsRelations(model);
        return await rz.RenderAsync();
    }

    internal async Task<string?> Generate_ProjectPackages(ProjectData model)
    {
        var rz= new ProjectPackages(model);
        return await rz.RenderAsync();
    }
    internal async Task<string?> Generate_BuildingBlocks(ProjectsDict model)
    {
        var rz = new BuildingBlocks(model);
        return await rz.RenderAsync();
    }
    internal async Task<string?> Generate_RootProjects(ProjectsDict model)
    {
        var rz = new ProjectRoot(model);
        return await rz.RenderAsync();
    }
    internal async Task<string?> Generate_TestProjects(ProjectsDict model)
    {
        var rz = new TestProjects(model);
        return await rz.RenderAsync();
    }

    internal async Task<string?> Generate_ProjectRelations(ProjectData projData)
    {
        var rz=new ProjectRelations(projData);
        return await rz.RenderAsync();
    }
}