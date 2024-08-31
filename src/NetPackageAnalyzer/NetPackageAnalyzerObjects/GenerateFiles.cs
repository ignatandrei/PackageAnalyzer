namespace NetPackageAnalyzerObjects;
public abstract class GenerateFiles: GenerateData
{
    public GenerateFiles(IFileSystem system):base(system)
    {
        
    }
    public abstract Task<string> GenerateNow(string folder, string where);

}
