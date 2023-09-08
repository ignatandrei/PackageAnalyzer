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
}
