using System.Linq;

namespace AnalyzeMerge;

public record DataToDisplayMerge(string nameBranch, string rootFolder,FileData[] files)
{
    
    public Dictionary<string, FileData[]> AfterExtension { get; internal set; } = new();
    public Dictionary<FileDataEnum, FileData[]> AfterStatus { get; internal set; } = new();
    public CsprojWithFiles[] addedCsproj = new CsprojWithFiles[0];
    public CsprojWithFiles[] modifiedCsproj = new CsprojWithFiles[0];
    private FileData[] csprojFoundOnHdd = new FileData[0];
    public CsprojWithFiles[] modifiedFilesInside = new CsprojWithFiles[0];
    public void Analyze() 
    {
        csprojFoundOnHdd = Directory
        .GetFiles(rootFolder, "*.csproj", SearchOption.AllDirectories)
        .Select(it => new FileData(it.Substring(rootFolder.Length), FileDataEnum.NotInterested))
        .ToArray();

        AfterExtension = files
           .Select(it => new KeyValuePair<string, FileData>(Path.GetExtension(it.relPath), it))
           .GroupBy(it => it.Key)
           .ToDictionary(
            it => it.Key
            , it => it.Select(it => it.Value)
            .ToArray());
        AfterStatus = files
           .Select(it => new KeyValuePair<FileDataEnum, FileData>(it.fileDataEnum, it))
           .GroupBy(it => it.Key)
           .ToDictionary(
            it => it.Key
            , it => it.Select(it => it.Value)
            .ToArray());
        var csprojs = AfterExtension
            .Where(it => it.Key == ".csproj")
            .SelectMany(it => it.Value)
            .ToArray();
        var relPathsCSProj = csprojs.Select(it=>it.relPath).ToArray();
        addedCsproj = csprojs
            .Where(it=>it.fileDataEnum == FileDataEnum.Added)
            .Select(it=>new {csproj=it,files= files.Where(f=>f.relPath.StartsWith(it.relFolderPath())).ToArray()})
            .Select(it=> new CsprojWithFiles(it.csproj,it.files))
            .ToArray();
        modifiedCsproj = csprojs
            .Where(it => it.fileDataEnum == FileDataEnum.Modified)
            .Select(it => new { csproj = it, files = files.Where(f => f.relPath.StartsWith(it.relFolderPath())).ToArray() })
            .Select(it => new CsprojWithFiles(it.csproj, it.files))
            .ToArray();
        var csprojNoModified = csprojFoundOnHdd.Where(it => !relPathsCSProj.Contains(it.relPath)).ToArray(); 
        if(csprojNoModified.Length> 0)
        {
            Dictionary<FileData,List<FileData>> filesInCsproj = csprojNoModified.ToDictionary(it=>it,it=> new List<FileData>());
            foreach (FileData file in files)
            {
                var pathFile = file.relFolderPath();
                var proj = csprojNoModified
                    .Select(it => new { it, ok = it.FolderStartsWithThis(pathFile) })
                    .Where(it => it.ok)
                    .Select(it =>new { it.it, nrRemains = maxDiff(it.it, pathFile) })
                    .MinBy(it=>it.nrRemains)                    
                    ;
                if(proj != null)
                {
                    filesInCsproj[proj.it].Add(file);
                }
            }
            
            modifiedFilesInside = filesInCsproj
                .Where(it=>it.Value.Count>0)
            .Select(it => new CsprojWithFiles(it.Key, it.Value.ToArray()))
            .ToArray();


        }
    }
    private int maxDiff(FileData file, string relPath)
    {
        var y = file.relFolderPath().Length;
        //can contain / at first and last
        if (Math.Abs(y - relPath.Length) < 2)
            return 0;

        return relPath.Substring(y).Length;

    }
}
