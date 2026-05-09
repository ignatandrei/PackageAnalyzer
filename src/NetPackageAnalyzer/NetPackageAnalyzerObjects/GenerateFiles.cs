using NPA.ProcessRunner;

namespace NetPackageAnalyzerObjects;
public abstract class GenerateFiles: GenerateData
{
    public GenerateFiles(IFileSystem system, IProcessRunner? processRunner = null):base(system, processRunner)
    {
        
    }
    public abstract Task<string> GenerateNow(string folder, string where);

}
