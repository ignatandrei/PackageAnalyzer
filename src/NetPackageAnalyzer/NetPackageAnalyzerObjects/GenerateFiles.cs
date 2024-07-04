namespace NetPackageAnalyzerObjects;
public abstract class GenerateFiles: GenerateData
{
    public GenerateFiles(IFileSystem system):base(system)
    {
        
    }
    public abstract Task<int> GenerateNow(string folder, string where);

}
