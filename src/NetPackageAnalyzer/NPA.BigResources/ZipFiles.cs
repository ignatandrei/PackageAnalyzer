using System.IO.Compression;

namespace NPA.BigResources;

public class ZipBigFiles
{
    //TODO : should contain files for Windows, Linux, MacOS
    public static async Task SaveToFile(string folderPath, EmbeddedResource embeddedResource)
    {
        if(Directory.Exists(folderPath) && Directory.EnumerateFiles(folderPath).Any())
        {
            return;
        }
        if(!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        using var stream = EmbeddedResources.GetStream(embeddedResource);
        
        await ZipFile.ExtractToDirectoryAsync(stream, folderPath);
        
        return ;
    }
}
