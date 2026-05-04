using System.Diagnostics;

namespace NPA.ProcessRunner;

public interface IProcessRunner
{
    Process? Start(ProcessStartInfo startInfo);
    ProcessExecutionResult Run(ProcessStartInfo startInfo);
    Task<ProcessExecutionResult> RunAsync(ProcessStartInfo startInfo, CancellationToken cancellationToken = default);
}
