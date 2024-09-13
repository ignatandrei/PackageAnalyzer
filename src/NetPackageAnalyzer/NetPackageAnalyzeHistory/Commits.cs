
namespace NetPackageAnalyzeHistory;
public class Commit
{
    public static readonly Commit Empty = new Commit();
    private Commit()
    {
        this.date = DateTime.MinValue;
        this.sha = "";
        this.Files = [];
    }
    public Commit(DateTime date,string sha)
    {
        this.date = date;
        this.sha = sha;
        this.Files = [];
    }
    public readonly DateTime date;
    public readonly string sha;

    public List<string> Files { get; }

    public int CountFiles()
    {
        return Files.Count;
    }
}
public class CommitsData : List<Commit>
{
    //public Dictionary<string, int> CountFiles()
    //{
    //    var files = this.SelectMany(it => it.Files);
    //    return files.GroupBy(it => it).ToDictionary(it => it.Key, it => it.Count());

    //}
    public static CommitsData EmptySingleton = new CommitsData();
    public Dictionary<string,int> FilesWithMaxCommits(int? year, int take)
    {

        var files = this.Where(it => year == null || it.date.Year == year)
             .SelectMany(it => it.Files);
        return files.GroupBy(it => it)
            .OrderByDescending(it => it.Count())
            .Take(take)
            .ToDictionary(it => it.Key, it => it.Count());

    }

    //public Dictionary<string, int> CountFilesPerYear(int year)
    //{

    //    var files = this.Where(it => it.date.Year == year)
    //        .SelectMany(it => it.Files);
    //    return files.GroupBy(it => it)
    //        .ToDictionary(it => it.Key, it => it.Count());
    //}

}

public class FolderHistoryCommits
{
    private readonly string nameFolder;
    public CommitsData commit = new();
    public FolderHistoryCommits(string nameFolder)
    {
        this.nameFolder = nameFolder;
    }
    public void Initialize()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = "log --name-status --date=iso-strict --pretty=\"%ad %H\" .",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            WorkingDirectory = nameFolder
        };

        // Create and start the process
        Process process = new Process { StartInfo = startInfo };
        process.Start();

        // Read the output from the command
        string output = process.StandardOutput.ReadToEnd();

        // Wait for the process to finish
        process.WaitForExit();
        var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string format = "yyyy-MM-dd HH:mm:ss";
        
        Commit? lastCommit = null;
        foreach (var line in lines)
        {
            if(line.Length <format.Length)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    if(!line.StartsWith("A "))
                        Console.WriteLine("strange line:" + line);
                }
                continue;
            }
            var date = line
                .Substring(0, format.Length)
                .Replace("T", " ");
            if (DateTime.TryParseExact(date, format, null, System.Globalization.DateTimeStyles.None, out var realDate))
            {
                var commitHash = line.Split(' ')[1];
                lastCommit = new Commit(realDate,commitHash);
                commit.Add(lastCommit);

            }
            else
            {
                var file = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                var fullName = Path.Combine(nameFolder, file[1]);
                lastCommit!.Files.Add(fullName);
            }

        }
    }

}