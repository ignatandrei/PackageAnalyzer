using NPA.ProcessRunner;

namespace AnalyzeMerge;
class ProcessOutput
{
    private readonly IProcessRunner processRunner;

    public ProcessOutput(IProcessRunner? processRunner = null)
    {
        this.processRunner = processRunner ?? new SystemProcessRunner();
    }

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

        var result = processRunner.Run(startInfo);
        string output = result.StandardOutput;
        string errorOutput = result.StandardError;
        if (errorOutput.Length > 0)
        {
            throw new ArgumentException(errorOutput);
        }
        return output;
    }
}

