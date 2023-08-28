namespace NetPackageAnalyzerConsole;
public enum PackageOptions
{
    None = 0,
    Outdated = 1,
    Deprecated = 2,
    Include_Transitive = 3
}
public class ProcessOutput
{
    public string OutputDotnet(string folder, PackageOptions packageOptions)
    {
        string arg = packageOptions.ToString().Replace("_", "-").ToLower();
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "dotnet.exe",
            WorkingDirectory = folder,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = $"list package --{arg} --format json"
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
            throw new Exception(errorOutput);
        }
        return output;
    }
}


