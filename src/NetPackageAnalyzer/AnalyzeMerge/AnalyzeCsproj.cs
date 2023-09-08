namespace AnalyzeMerge;
public class AnalyzeCsproj : AnalyzeForProject
{
    public AnalyzeCsproj(string rootFolder, FileData[] files):base(rootFolder, files, "*.csproj")
    {
        
    }
}
public class AnalyzeNode: AnalyzeForProject
{
    public AnalyzeNode(string rootFolder, FileData[] files) : base(rootFolder, files, "package.json")
    {

    }
}
public class AnalyzeForProject
{
    public ProjectWithFiles[] addedProj = new ProjectWithFiles[0];
    public ProjectWithFiles[] modifiedProj = new ProjectWithFiles[0];
    private FileData[] ProjFoundOnHdd = new FileData[0];
    public ProjectWithFiles[] modifiedFilesInside = new ProjectWithFiles[0];
    private readonly string rootFolder;
    private readonly FileData[] files;
    private readonly string search;

    public AnalyzeForProject(string rootFolder, FileData[] files,string search)
    {
        ProjFoundOnHdd = Directory
        .GetFiles(rootFolder, search, SearchOption.AllDirectories)
        .Select(it => new FileData(it.Substring(rootFolder.Length), FileDataEnum.NotInterested))
        .ToArray();
        this.rootFolder = rootFolder; 
        this.files = files;
        this.search = search;
    } 
    private int maxDiff(FileData file, string relPath)
    {
        var y = file.relFolderPath().Length;
        //can contain / at first and last
        if (Math.Abs(y - relPath.Length) < 2)
            return 0;

        return relPath.Substring(y).Length;

    }
    public void Analyze()
    {

        var AfterExtension = files
           .Select(it => new KeyValuePair<string, FileData>(Path.GetExtension(it.relPath), it))
           .GroupBy(it => it.Key)
           .ToDictionary(
            it => it.Key
            , it => it.Select(it => it.Value)
            .ToArray());
        var csprojs = AfterExtension
    .Where(it => it.Key == ".csproj")
    .SelectMany(it => it.Value)
    .ToArray();
        var relPathsCSProj = csprojs.Select(it => it.relPath).ToArray();
        addedProj = csprojs
            .Where(it => it.fileDataEnum == FileDataEnum.Added)
            .Select(it => new { csproj = it, files = files.Where(f => f.relPath.StartsWith(it.relFolderPath())).ToArray() })
            .Select(it => new ProjectWithFiles(it.csproj, it.files))
            .ToArray();
        modifiedProj = csprojs
            .Where(it => it.fileDataEnum == FileDataEnum.Modified)
            .Select(it => new { csproj = it, files = files.Where(f => f.relPath.StartsWith(it.relFolderPath())).ToArray() })
            .Select(it => new ProjectWithFiles(it.csproj, it.files))
            .ToArray();
        var csprojNoModified = ProjFoundOnHdd.Where(it => !relPathsCSProj.Contains(it.relPath)).ToArray();
        if (csprojNoModified.Length > 0)
        {
            Dictionary<FileData, List<FileData>> filesInCsproj = csprojNoModified.ToDictionary(it => it, it => new List<FileData>());
            foreach (FileData file in files)
            {
                var pathFile = file.relFolderPath();
                var proj = csprojNoModified
                    .Select(it => new { it, ok = it.FolderStartsWithThis(pathFile) })
                    .Where(it => it.ok)
                    .Select(it => new { it.it, nrRemains = maxDiff(it.it, pathFile) })
                    .MinBy(it => it.nrRemains)
                    ;
                if (proj != null)
                {
                    filesInCsproj[proj.it].Add(file);
                }
            }

            modifiedFilesInside = filesInCsproj
                .Where(it => it.Value.Count > 0)
            .Select(it => new ProjectWithFiles(it.Key, it.Value.ToArray()))
            .ToArray();



        }
    }
}
