namespace NetPackageAnalyzeHistory;
record History(int? nrCommits, DateTime? FirstCommit, DateTime? LastCommit);
public class FileFolderHistorySimple
{
    public readonly string nameFile;
    public int? numberCommitsFile { get; private set; }
    public int? numberCommitsFolder { get; private set; }
    public DateTime? LastCommitFile { get; private set; }
    public DateTime? FirstCommitFile { get; private set; }
    public DateTime? LastCommitFolder { get; private set; }
    public DateTime? FirstCommitFolder { get; private set; }
    public FileFolderHistorySimple(string nameFile)
    {
        this.nameFile = nameFile;
    }
    private History NrCommits(string folder,string what)
    {
        ProcessStartInfo startInfo = new()
        {
            //TODO: make time sensitive comment
            //TODO: use where to find where git is
            FileName = "git.exe",
            WorkingDirectory = folder,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            //Arguments = "log --pretty=oneline -n 100000  -- " + what,
            Arguments= "log --date=iso-strict --pretty=\"%ad\" -- "+ what
        };

        // Create and start the process
        Process process = new Process
        {
            StartInfo = startInfo
        };
        process.Start();

        // Read the output
        string output = process.StandardOutput.ReadToEnd();
        string errorOutput = process.StandardError.ReadToEnd();

        // Wait for the process to exit
        process.WaitForExit();
        if (errorOutput.Length > 0)
        {
            throw new ArgumentException(errorOutput);
        }
        var nrLines = output.Split('\r', '\n') ?? [];
        nrLines = nrLines.Where(it => !string.IsNullOrWhiteSpace(it)).ToArray();
        HashSet<DateTime> dates = new();
        foreach (var line in nrLines)
        {
            if(string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            if(line.Length < 10)
            {
                continue;
            }
            var str = line.Substring(0,10);
            if (DateTime.TryParseExact(str,"yyyy-MM-dd",null,System.Globalization.DateTimeStyles.None, out var date))
            {
                dates.Add(date);
            }
            
        }
        DateTime? LastCommit= null, FirstCommit=null;
        if(dates.Count > 0)
        {
            LastCommit = dates.Max();
            FirstCommit= dates.Min();
        }
        
        return  new History( nrLines.Length,FirstCommit, LastCommit);

    }
    public void Initialize(bool AddHistoryForFolder)
    {
        ArgumentNullException.ThrowIfNull(nameFile);
        var folder=Path.GetDirectoryName(nameFile);
        ArgumentNullException.ThrowIfNull(folder);
        (numberCommitsFile,FirstCommitFile,LastCommitFile) = NrCommits(folder, nameFile);
        if(AddHistoryForFolder)
            (numberCommitsFolder,FirstCommitFolder,LastCommitFolder) = NrCommits(folder, ".");
        
        //Console.WriteLine("done with " + nameFile + $"{LastCommitFolder != LastCommitFile}");
        
    }
}
