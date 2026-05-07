using System.IO.Abstractions;
using System.IO.Compression;

namespace NPA.BigResources;

public class ZipBigFiles
{
    //TODO : should contain files for Windows, Linux, MacOS
    public static async Task SaveToFile(string folderPath, EmbeddedResource embeddedResource, IFileSystem? fileSystem = null)
    {
        fileSystem ??= new FileSystem();

        if(fileSystem.Directory.Exists(folderPath) && fileSystem.Directory.EnumerateFiles(folderPath).Any())
        {
            return;
        }
        if(!fileSystem.Directory.Exists(folderPath))
        {
            fileSystem.Directory.CreateDirectory(folderPath);
        }
        using var stream = EmbeddedResources.GetStream(embeddedResource);
        
        await ZipFile.ExtractToDirectoryAsync(stream, folderPath);
        
        return ;
    }
}
