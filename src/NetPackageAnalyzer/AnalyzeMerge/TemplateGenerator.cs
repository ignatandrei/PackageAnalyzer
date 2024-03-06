using AnalyzeMerge.Templates;

namespace AnalyzeMerge;

internal class TemplateGenerator
{
    public async Task<string> Generate_DisplayAllFiles(DataToDisplayMerge model)
    {
        var rz = new DisplayAllFiles(model);
        return await rz.RenderAsync();
    }
    public async Task<string> Generate_DisplayAllFilesMD(DataToDisplayMerge model)
    {
        var rz = new DisplayAllFilesMarkdown(model);
        return await rz.RenderAsync();
        
    }
}
