using System.Diagnostics;
using System.Text.Json;

namespace NPA.ProcessRunner;

public sealed record ProcessExecutionResult(int ExitCode, string StandardOutput, string StandardError);

public sealed record ProcessToSerialize(string file,string arguments, string workingDirectory)
{
    public static ProcessToSerialize FromProcessStartInfo(ProcessStartInfo psi) => new(psi.FileName, psi.Arguments, psi.WorkingDirectory);
    public ProcessStartInfo ToProcessStartInfo()
    {
        return new ProcessStartInfo
        {
            FileName = file,
            Arguments = arguments,
            WorkingDirectory = workingDirectory
        };
    }
}

public sealed class ProcessExecutionResultToSerialize
{
    public ProcessExecutionResultToSerialize()
    {

    }
    public ProcessExecutionResultToSerialize(ProcessStartInfo psi, ProcessExecutionResult result)
    {
        this.pso = ProcessToSerialize.FromProcessStartInfo(psi);
        this.result = result;
    }
    public ProcessToSerialize? pso { get; set; }
    public ProcessExecutionResult? result { get; set; }

    public string Serialize()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
    public static ProcessExecutionResultToSerialize Deserialize(string json)
    {
        return JsonSerializer.Deserialize<ProcessExecutionResultToSerialize>(json) ?? throw new InvalidOperationException("Deserialization failed.");
    }

};