using System.Diagnostics;

namespace NPA.GitInfo;

public class GitInfo
{
    private GitInfo()
    {
    }
    public string Repository { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;

    public string Commit { get; set; }= string.Empty;

    static Dictionary<string,GitInfo> gitInfo=[];
    public static GitInfo Construct(string folder)
    {
        
        if(gitInfo.ContainsKey(folder))
        {
            return gitInfo[folder];
        }
        var ret = new GitInfo();
        ret.Branch = ExecuteGit(folder, "branch --show-current");
        ret.Repository = ExecuteGit(folder, "config --get remote.origin.url");
        if(ret.Repository.EndsWith(".git"))
        {
            ret.Repository = ret.Repository.Substring(0, ret.Repository.Length - 4);
        }
        ret.Commit = ExecuteGit(folder, "rev-parse HEAD");
        gitInfo.Add(folder, ret);
        return ret;
    }

    static string ExecuteGit(string folder, string args)
    {
        

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
