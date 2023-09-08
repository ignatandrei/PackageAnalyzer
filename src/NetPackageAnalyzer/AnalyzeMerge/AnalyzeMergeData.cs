namespace AnalyzeMerge;
public record CsprojWithFiles(FileData csproj, FileData[] files)
{

}    
public record DataToDisplayMerge(string nameBranch, FileData[] files)
{
    public Dictionary<string, FileData[]> AfterExtension { get; internal set; } = new();
    public Dictionary<FileDataEnum, FileData[]> AfterStatus { get; internal set; } = new();
    public CsprojWithFiles[] addedCsproj = new CsprojWithFiles[0];
    public CsprojWithFiles[] modifiedCsproj = new CsprojWithFiles[0];

    public void Analyze()
    {
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

    }
}
public class AnalyzeMergeData
{
    private readonly string folder;
    private string folderRoot;
    public AnalyzeMergeData(string folder)
    {
        this.folder = folder;
        folderRoot=folder;
    }

    public async Task GenerateNow()
    {
        await Task.Delay(1000);
        
        //var data=new DataToDisplayMerge(GetBranchName(), FileNames());
        var relPath = relFolder();
        folderRoot= folder;
        if (relPath?.Length> 1) {
            var length = folder.Length - relPath.Length;
            if(relPath.EndsWith("/")||relPath.EndsWith("\\"))
            {
                if(!(folderRoot.EndsWith("/")|| folderRoot.EndsWith("\\")))
                {
                    folderRoot+= "/" ;//does not matter , will be removed
                }
            }
            folderRoot = folderRoot.Substring(0, folder.Length - relPath.Length);
        }

        using (var repo = new Repository(folderRoot))
        {
            //Commit? debugCommit = null;
            Branch? mainBranch = null;
            //foreach (var item in repo.Commits)
            //{
            //    if (item.Sha == "fdeddfa037105ec84d8e92f1be9eb7ee00f63f20")
            //    {
            //        debugCommit= item;
            //        break;
            //    }
            //}
            //ArgumentNullException.ThrowIfNull(debugCommit);
            foreach (var branch in repo.Branches)
            {
                if (branch.FriendlyName == "main")
                {
                    mainBranch= branch;
                    break;
                }
                if(branch.FriendlyName == "master")
                {
                    mainBranch = branch;
                    break;
                }
            }
            ArgumentNullException.ThrowIfNull(mainBranch);
            //var mainBranch = repo.Branches["main"];
            //var name = GetBranchName();
            //var currentBranch1 = repo.Branches[name];
            var currentBranch1 = repo.Head;            
            var treeChanges = repo.Diff.Compare<TreeChanges>(mainBranch.Tip.Tree, currentBranch1.Tip.Tree);
            //treeChanges = repo.Diff.Compare<TreeChanges>(debugCommit.Tree, mainBranch.Tip.Tree);

            foreach (var change in treeChanges)
            {                                
                Console.WriteLine($"{change.Status}: {change.Path}");
            }
            var files = treeChanges.Select(it => new FileData(it.Path ,
                it.Status switch
                {
                    ChangeKind.Modified => FileDataEnum.Modified,
                    ChangeKind.Deleted => FileDataEnum.Deleted,
                    ChangeKind.Added => FileDataEnum.Added,
                    ChangeKind.Renamed=>FileDataEnum.Renamed,
                    _ => FileDataEnum.NotInterested
                })).ToArray();
            var data = new DataToDisplayMerge(currentBranch1.FriendlyName,files);
            data.Analyze();
            var g = new TemplateGenerator();
            var folderResults = Path.Combine(folderRoot, "changes");
            if(!Path.Exists(folderResults))
                Directory.CreateDirectory(folderResults);
            var file = Path.Combine(folderResults, $"AllFiles{currentBranch1.FriendlyName}.html");
            await File.WriteAllTextAsync(file, await g.Generate_DisplayAllFiles (data));
             
            //Console.WriteLine("=>"+name);
            //Branch? currentBranch = null;
            //foreach (var item in repo.Branches)
            //{
            //    if(item.FriendlyName == name)
            //    {
            //        currentBranch = item;
            //        break;
            //    }
            //}
            //var currentBranch = repo.Branches[name]; // Get the current branch (the one you are currently on)
            //var shaCommits = mainBranch.Commits.Select(it => it.Sha).ToArray();    

            //var currentBranch = repo.Head;
            //ArgumentNullException.ThrowIfNull(currentBranch);
            //Console.WriteLine("branch name"+currentBranch.FriendlyName);
            //var commits=currentBranch
            //    .Commits
            //    .Where(it=>!shaCommits.Contains(it.Sha) )
            //    .ToArray();
            //Console.WriteLine("commits " + commits.Length);
            //foreach(var commit in commits)
            //{
            //    var changes = repo.Diff.Compare<TreeChanges>(mainBranch.Tip.Tree, commit.Tree);
            //    foreach(var change in changes)
            //    {
            //        Console.WriteLine(commit.MessageShort +"=>"+ change.Status + " " + change.Path);
            //    }
            //}
            //var status = repo.RetrieveStatus();
            //foreach (var entry in status)
            //{
            //    if (entry.State == FileStatus.Ignored)
            //        continue;
            //    if (entry.State == FileStatus.Nonexistent)
            //        continue;
            //    if (entry.State == FileStatus.Unaltered)
            //        continue;
            //    if (entry.State == FileStatus.Unreadable)
            //        continue;
            //    if(entry.State == FileStatus.ModifiedInWorkdir && entry.FilePath.EndsWith(".json"))
            //    {
            //        string path = entry.FilePath;
            //        var file = currentBranch[path].Target as Blob ;
            //        var mainContent=file?.GetContentText();
            //        if (mainContent!= null)
            //        {                        
            //            var currentFile = Path.Combine(folderRoot, path);
            //            //Console.Write(file.GetContentText());
            //            var currentContent=(File.ReadAllText(currentFile));
            //            if (currentContent != null && mainContent != null)
            //            {

            //                var latestLines = currentContent.Split('\n', '\r');
            //                var headLines = mainContent.Split('\n', '\r');
            //                //Console.WriteLine("tst" + latestLines.Length + "--" + headLines.Length);
            //                for (int i = 0; i < Math.Min(latestLines.Length, headLines.Length); i++)
            //                {
            //                    if (latestLines[i] != headLines[i])
            //                    {
            //                        Console.WriteLine($"Line {i + 1} in file {path} was modified:");
            //                        Console.WriteLine($"Latest: {latestLines[i]}");
            //                        Console.WriteLine($"HEAD: {headLines[i]}");
            //                        Console.WriteLine();
            //                    }
            //                }
            //            }

            //        }

            //    }
            //    Console.WriteLine($"File: {entry.FilePath}, Status: {entry.State}");
            //}
            //Tree commitTree = repo.Head.Tip.Tree;
            //Tree parentCommitTree = repo.Head.Tip.Parents.Single().Tree;
            //TreeChanges changes = repo.Diff.Compare<TreeChanges>(parentCommitTree, commitTree);
            //Console.WriteLine(changes.Count);
            //foreach (var treeEntryChanges in changes)
            //{
            //    Console.WriteLine("Path:{0} +{1} -{2} ",
            //            treeEntryChanges.Path,
            //            treeEntryChanges.Mode,
            //            treeEntryChanges.Status
            //        );
            //}

        }
        //data.Analyze();
    }
    string? relFolder()
    {
        var p = new ProcessOutput();
        return p.ExecuteGit(folder, GitData.RelativeFolder)
            .Split('\r','\n')
            .Where(it=>!string.IsNullOrWhiteSpace(it))
            .FirstOrDefault();
    }

    //string[] FileNames() {
    //    return new string[1] { "doesnt work multiple commits same branch" };
    //    //var p = new ProcessOutput();
    //    //return p.ExecuteGit(folder, GitData.Files)
    //    //    .Split('\r', '\n')
    //    //    .ToArray();
    //}
    //string GetBranchName()
    //{
    //    var p = new ProcessOutput();
    //    return p.ExecuteGit(folder, GitData.BranchName)
    //        .Split('\r','\n')
    //        .Where(it=>!string.IsNullOrWhiteSpace(it))
    //        .Single();

    //}
}