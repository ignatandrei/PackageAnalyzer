﻿namespace NetPackageAnalyzerDocusaurus;
public class TemplateGenerator
{
    public async Task<string> Generate_DisplayAllVersions(DisplayDataMoreThan1Version? model)
    {
        if(model == null)
            return string.Empty;

        var rz = new DisplayAllVersions(model);
        return await rz.RenderAsync();
    }
    public async Task<string?> Generate_OutDeprMarkdown(PackageWithVersion[] model)
    {
        var rz = new DeprecatedOutdated(model);
        return await rz.RenderAsync();

    }
    public async Task<string?> Generate_DisplayAllVersionsMarkdown(DisplayDataMoreThan1Version? model)
    {
        if(model == null)
            return string.Empty;

        var rz = new DisplayAllVersionsForMarkdown(model);
        return await rz.RenderAsync();

    }
    //public async Task<string?> Generate_DisplayAllVersionsWithProblemsMarkdown(DisplayDataMoreThan1Version model)
    //{
    //    var rz = new DisplayAllVersionsProblemsForMarkdown(model);
    //    return await rz.RenderAsync();

    //}
    public async Task<string> Generate_MermaidVisualizerMajorDiffer(DisplayDataMoreThan1Version? model)
    {
        if (model == null)
            return string.Empty;
        var rz = new MermaidVisualizerMajorDiffer(model);
        return await rz.RenderAsync();
    }
    public async Task<string> Generate_SolutionRelations(ProjectsDict model)
    {
        var rz = new SolutionRelations(model);
        return await rz.RenderAsync();
    }

    internal async Task<string?> Generate_ProjectPackages(ProjectData model)
    {
        var rz= new ProjectPackages(model);
        return await rz.RenderAsync();
    }
    internal async Task<string?> Generate_ProjectCommits(ProjectData model)
    {
        var rz = new ProjectCommits(model);
        return await rz.RenderAsync();
    }
    internal async Task<string?> Generate_BuildingBlocks(ProjectsDict model)
    {
        var rz = new BuildingBlocks(model);
        return await rz.RenderAsync();
    }
    internal async Task<string?> Generate_Commits(ProjectsDict model)
    {
        var rz = new Commits(model);
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
    internal async Task<string?> Generate_BlogPost(
        InfoSolution model, ProjectsDict projectsDict
        , DisplayDataMoreThan1Version? displayDataMoreThan1Version
        , ClassesRefData? refSummary,
        PublicClassRefData? publicClassRefData)
    { 
        if(displayDataMoreThan1Version == null || refSummary == null || publicClassRefData == null)
            return string.Empty;

        var modelData = Tuple.Create(model, projectsDict, displayDataMoreThan1Version, refSummary, publicClassRefData);
        var rz = new BlogPost(modelData);
        return await rz.RenderAsync();
    }
    internal async Task<string?> Generate_SolutionIntroduction(InfoSolution model)
    {
        var rz = new SolutionResume(model);
        return await rz.RenderAsync();
    }
    internal async Task<string?> Generate_ProjectRelations(ProjectData projData)
    {
        var rz=new ProjectRelations(projData);
        return await rz.RenderAsync();
    }

    internal async Task<string?> Generate_ReferencesSummaryProjects(ClassesRefData refSummary)
    {
        var rz=new ReferencesSummaryProjects(refSummary);
        return await rz.RenderAsync();
    }
    internal async Task<string?> Generate_PublicClasses(PublicClassRefData refPublic)
    {
        var rz = new ReferencesPublicClasses(refPublic);
        return await rz.RenderAsync();
    }
}