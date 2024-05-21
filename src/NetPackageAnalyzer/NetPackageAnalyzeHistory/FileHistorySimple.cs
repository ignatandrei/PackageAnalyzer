namespace NetPackageAnalyzeHistory;

public class FileHistorySimple
{
    public readonly string nameFile;
    public int numberCommits { get; private set; }
    public FileHistorySimple(string nameFile)
    {
        this.nameFile = nameFile;
    }
    public void Initialize()
    {
        ArgumentNullException.ThrowIfNull(nameFile);
        var folder=Path.GetDirectoryName(nameFile);
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
            Arguments = "log --pretty=oneline -n 100000  -- " + nameFile
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
        var nrLines = output.Split('\r', '\n') ?? [] ;
        numberCommits = nrLines
            .Where(it=>!string.IsNullOrWhiteSpace(it) )
            .ToArray()
            .Length;
    }
}
