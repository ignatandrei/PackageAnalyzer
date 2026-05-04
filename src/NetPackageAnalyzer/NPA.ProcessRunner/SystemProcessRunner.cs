using System.Diagnostics;

namespace NPA.ProcessRunner;

 
public sealed class SystemProcessRunner : IProcessRunner
{
    //static int  nr=0;
    public Process? Start(ProcessStartInfo startInfo) => Process.Start(startInfo);

    public ProcessExecutionResult Run(ProcessStartInfo startInfo) =>
        RunAsync(startInfo).GetAwaiter().GetResult();

    public async Task<ProcessExecutionResult> RunAsync(
        ProcessStartInfo startInfo,
        CancellationToken cancellationToken = default)
    {
        using var process = new Process
        {
            StartInfo = startInfo
        };

        process.Start();

        var outputTask = startInfo.RedirectStandardOutput
            ? process.StandardOutput.ReadToEndAsync(cancellationToken)
            : Task.FromResult(string.Empty);
        var errorTask = startInfo.RedirectStandardError
            ? process.StandardError.ReadToEndAsync(cancellationToken)
            : Task.FromResult(string.Empty);

        await process.WaitForExitAsync(cancellationToken);
        var output = await outputTask;
        var error = await errorTask;
        var ret= new ProcessExecutionResult(
            process.ExitCode,
            output,
            error);
        //ProcessExecutionResultToSerialize p = new ProcessExecutionResultToSerialize(startInfo, ret);
        //string foldedOutput = @"D:\eu\GitHub\PackageAnalyzer\src\NetPackageAnalyzer\NetPackageAnalyzerConsole.Tests\eshopJSON";
        //Interlocked.Increment(ref nr);
        //await File.WriteAllTextAsync(Path.Combine(foldedOutput, $"output_{nr}.json"), p.Serialize());
        return ret;
    }
}
