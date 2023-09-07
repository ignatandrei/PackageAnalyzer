namespace AnalyzeMerge;
class ProcessOutput
{
    public string ExecuteGit(string folder, GitData gitData)
    {
        var args = gitData switch
        {
            GitData.BranchName => "branch --show-current",
            GitData.Files => "diff --name-only HEAD",
            GitData.RelativeFolder => "rev-parse --show-prefix",
            _ => throw new ArgumentException("not found " + gitData),
        };

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            //TODO: make time sensitive comment
            //TODO: use where to find where git is
            FileName = "git.exe",
            WorkingDirectory = folder,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = args
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
        return output;
    }
}

