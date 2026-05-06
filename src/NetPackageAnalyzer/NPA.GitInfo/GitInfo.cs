using System.Diagnostics;
using NPA.ProcessRunner;

namespace NPA.GitInfo;

public class GitInfo
{
    public IProcessRunner ProcessRunner { get; set; } 

    private GitInfo(IProcessRunner? processRunner)
    {
        ProcessRunner = processRunner ?? new SystemProcessRunner();
    }
    public string Repository { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;

    public string Commit { get; set; }= string.Empty;

    static Dictionary<string,GitInfo> gitInfo=[];
    public static GitInfo Construct(string folder, IProcessRunner? processRunner = null)
    {
        
        processRunner = processRunner ?? new SystemProcessRunner();
        if(gitInfo.ContainsKey(folder))
        {
            return gitInfo[folder];
        }
        var ret = new GitInfo(processRunner);
        ret.Branch = ExecuteGit(folder, "branch --show-current", processRunner);
        ret.Repository = ExecuteGit(folder, "config --get remote.origin.url", processRunner);
        if(ret.Repository.EndsWith(".git"))
        {
            ret.Repository = ret.Repository.Substring(0, ret.Repository.Length - 4);
        }
        ret.Commit = ExecuteGit(folder, "rev-parse HEAD", processRunner);
        gitInfo.Add(folder, ret);
        return ret;
    }

    static string ExecuteGit(string folder, string args, IProcessRunner? processRunner)
    {
        processRunner ??= new SystemProcessRunner();

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
        var data = output
          .Split('\r', '\n')
            .Where(it => !string.IsNullOrWhiteSpace(it))
            .ToArray();
        if (data.Length != 1)
        {
            throw new ArgumentException("Expected 1 line, got " + output);
        }
        return data[0];
    }
}
