namespace AnalyzeMerge;

public record FileData(string relPath, FileDataEnum fileDataEnum)
{
    public string relFolderPath()
    {
        var name=Name();
        return relPath.Substring(0, relPath.Length - name.Length);
    }
    public string Name()
    {
        return Path.GetFileName(relPath);
    }

    public bool ThisStartsWithFolder(string folderPath)
    {
        var myPath = relFolderPath().Replace("/", "\\");
        folderPath = folderPath.TrimStart('/', '\\');
        myPath = myPath.TrimStart('/', '\\');

        // Check if folderPath1 starts with folderPath2
        return myPath.StartsWith(folderPath, StringComparison.OrdinalIgnoreCase);
    }
    public bool FolderStartsWithThis(string folderPath)
    {
        var myPath = relFolderPath().Replace("/", "\\");
        myPath = myPath.TrimStart('/', '\\');
        folderPath = folderPath.TrimStart('/', '\\');
        folderPath= folderPath.Replace("/", "\\");



        // Check if folderPath1 starts with folderPath2
        return folderPath.StartsWith(myPath, StringComparison.OrdinalIgnoreCase);
    }
}
