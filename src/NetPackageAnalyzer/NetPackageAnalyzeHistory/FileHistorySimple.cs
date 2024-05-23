namespace NetPackageAnalyzeHistory;

public class FileFolderHistorySimple
{
    public readonly string nameFile;
    public int numberCommitsFile { get; private set; }
    public int? numberCommitsFolder { get; private set; }
    public FileFolderHistorySimple(string nameFile)
    {
        this.nameFile = nameFile;
    }
    private int NrCommits(string folder,string what)
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
            Arguments = "log --pretty=oneline -n 100000  -- " + what
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
        return  nrLines
            .Where(it => !string.IsNullOrWhiteSpace(it))
            .ToArray()
            .Length;

    }
    public void Initialize(bool AddHistoryForFolder)
    {
        ArgumentNullException.ThrowIfNull(nameFile);
        var folder=Path.GetDirectoryName(nameFile);
        ArgumentNullException.ThrowIfNull(folder);
        numberCommitsFile = NrCommits(folder, nameFile);
        if(AddHistoryForFolder)
            numberCommitsFolder = NrCommits(folder, ".");

    }
}
