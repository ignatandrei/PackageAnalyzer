using LibGit2Sharp;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace AnalyzeMerge;
public record DataToDisplayMerge(string nameBranch,string[] files )
{
    public Dictionary<string, string[]> AfterExtension { get; internal set; } = new();
    public void Analyze()
    {
        AfterExtension = files
           .Select(it => new KeyValuePair<string, string>(Path.GetExtension(it), it))
           .GroupBy(it => it.Key)
           .ToDictionary(
            it => it.Key
            , it => it.Select(it => it.Value)
            .ToArray());

    }
}
public class AnalyzeMergeData
{
    private readonly string folder;
    private string folderRoot;
    public AnalyzeMergeData(string folder)
    {
        this.folder = folder;
    }

    public async Task GenerateNow()
    {
        await Task.Delay(1000);
        
        var data=new DataToDisplayMerge(GetBranchName(), FileNames());
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
            //Console.WriteLine($"AAA{folderNew}=>{relPath}");
            folderRoot = folderRoot.Substring(0, folder.Length - relPath.Length);
        }
        //Console.WriteLine($"{folder}=>{relPath}=>{folderNew}");

        using (var repo = new Repository(folderRoot))
        {
            //var name = GetBranchName();
            var mainBranch = repo.Branches["main"];
            var shaCommits = mainBranch.Commits.Select(it => it.Sha).ToArray();    
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
            var currentBranch = repo.Head;
            ArgumentNullException.ThrowIfNull(currentBranch);
            Console.WriteLine("branch name"+currentBranch.FriendlyName);
            var commits=currentBranch
                .Commits
                .Where(it=>!shaCommits.Contains(it.Sha) )
                .ToArray();
            Console.WriteLine("commits " + commits.Length);
            foreach(var commit in commits)
            {

                var changes = repo.Diff.Compare<TreeChanges>(mainBranch.Tip.Tree, commit.Tree);
                foreach(var change in changes)
                {
                    Console.WriteLine(commit.MessageShort +"=>"+ change.Status + " " + change.Path);
                }
            }
            var status = repo.RetrieveStatus();
            foreach (var entry in status)
            {
                if (entry.State == FileStatus.Ignored)
                    continue;
                if (entry.State == FileStatus.Nonexistent)
                    continue;
                if (entry.State == FileStatus.Unaltered)
                    continue;
                if (entry.State == FileStatus.Unreadable)
                    continue;
                if(entry.State == FileStatus.ModifiedInWorkdir && entry.FilePath.EndsWith(".json"))
                {
                    string path = entry.FilePath;
                    var file = currentBranch[path].Target as Blob ;
                    var mainContent=file?.GetContentText();
                    if (mainContent!= null)
                    {                        
                        var currentFile = Path.Combine(folderRoot, path);
                        //Console.Write(file.GetContentText());
                        var currentContent=(File.ReadAllText(currentFile));
                        if (currentContent != null && mainContent != null)
                        {

                            var latestLines = currentContent.Split('\n', '\r');
                            var headLines = mainContent.Split('\n', '\r');
                            //Console.WriteLine("tst" + latestLines.Length + "--" + headLines.Length);
                            for (int i = 0; i < Math.Min(latestLines.Length, headLines.Length); i++)
                            {
                                if (latestLines[i] != headLines[i])
                                {
                                    Console.WriteLine($"Line {i + 1} in file {path} was modified:");
                                    Console.WriteLine($"Latest: {latestLines[i]}");
                                    Console.WriteLine($"HEAD: {headLines[i]}");
                                    Console.WriteLine();
                                }
                            }
                        }

                    }

                }
                Console.WriteLine($"File: {entry.FilePath}, Status: {entry.State}");
            }
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
        data.Analyze();
    }
    string? relFolder()
    {
        var p = new ProcessOutput();
        return p.ExecuteGit(folder, GitData.RelativeFolder)
            .Split('\r','\n')
            .Where(it=>!string.IsNullOrWhiteSpace(it))
            .FirstOrDefault();
    }

    string[] FileNames() {
        return new string[1] { "doesnt work multiple commits same branch" };
        //var p = new ProcessOutput();
        //return p.ExecuteGit(folder, GitData.Files)
        //    .Split('\r', '\n')
        //    .ToArray();
    }
    string GetBranchName()
    {
        var p = new ProcessOutput();
        return p.ExecuteGit(folder, GitData.BranchName)
            .Split('\r','\n')
            .Where(it=>!string.IsNullOrWhiteSpace(it))
            .Single();

    }
}