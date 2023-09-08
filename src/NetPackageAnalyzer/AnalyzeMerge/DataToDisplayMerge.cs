namespace AnalyzeMerge;
public record DataToDisplayMerge(string nameBranch, string rootFolder, FileData[] files)
{
    public AnalyzeCsproj? analyzeCsproj;
    public AnalyzeNode? analyzeNode;
    public Dictionary<string, FileData[]> AfterExtension { get; internal set; } = new();
    public Dictionary<FileDataEnum, FileData[]> AfterStatus { get; internal set; } = new();
    public Dictionary<string, FileData[]> AfterFolder { get; internal set; } = new();
    public void Analyze()
    {
        analyzeCsproj = new AnalyzeCsproj(rootFolder, files);
        analyzeCsproj.Analyze();
        analyzeNode = new AnalyzeNode(rootFolder, files);
        analyzeNode.Analyze();

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
        AfterFolder = files 
            .Select(it=>new KeyValuePair<string,FileData>(it.relFolderPath(),it))
            .GroupBy(it=> it.Key)
            .ToDictionary(it=>it.Key,it=>it.Select(it => it.Value).ToArray())
            ;
    }

  
}
