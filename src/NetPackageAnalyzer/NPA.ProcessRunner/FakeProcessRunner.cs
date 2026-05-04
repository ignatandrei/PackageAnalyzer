using System.Diagnostics;

namespace NPA.ProcessRunner;

public sealed class FakeProcessRunner : IProcessRunner
{
    private readonly Queue<ProcessExecutionResult> runResults = new();
    Dictionary<ProcessToSerialize, ProcessExecutionResultToSerialize> results = new();

    public void DeserializeFromFolder(string folder)
    {
        results.Clear();
        foreach (var item in Directory.GetFiles(folder, "*.json"))
        {
            var content = File.ReadAllText(item);
            var deserialized = ProcessExecutionResultToSerialize.Deserialize(content);
            var pso = deserialized.pso ?? throw new InvalidOperationException("Deserialized object missing ProcessStartInfo");
            if (results.ContainsKey(pso))
            {
                if (results[pso].result != deserialized.result)
                {
                    if(pso.file.StartsWith("dotnet") && pso.arguments.StartsWith("build"))
                    {
                        // Allow different build results for the same ProcessStartInfo, since they can be non-deterministic
                        continue;
                    }
                    throw new InvalidOperationException("Conflicting results for the same ProcessStartInfo");
                }
                continue;
            }
            results[deserialized.pso ?? throw new InvalidOperationException("Deserialized object missing ProcessStartInfo")] = deserialized;
        }
    }
    public Process? Start(ProcessStartInfo startInfo)
    {        
        return null;
    }

    public ProcessExecutionResult Run(ProcessStartInfo psi)
    {
        var p = ProcessToSerialize.FromProcessStartInfo(psi);
        if(results.TryGetValue(p, out var result))
        {
            return result.result ?? throw new InvalidOperationException("Deserialized object missing ProcessExecutionResult");
        }
        else
        {
            throw new ArgumentException("Cannot find output for "+ p);
        }
    }

    public Task<ProcessExecutionResult> RunAsync(ProcessStartInfo startInfo, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Run(startInfo));
    }

    
}
