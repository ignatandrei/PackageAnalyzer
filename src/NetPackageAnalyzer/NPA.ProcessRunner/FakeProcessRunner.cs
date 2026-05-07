using System.Diagnostics;
using System.IO.Abstractions;

namespace NPA.ProcessRunner;

public sealed class FakeProcessRunner : IProcessRunner
{
    private readonly IFileSystem fileSystem;
    private readonly Queue<ProcessExecutionResult> runResults = new();
    Dictionary<ProcessToSerialize, ProcessExecutionResultToSerialize> results = new();

    public FakeProcessRunner(IFileSystem? fileSystem = null)
    {
        this.fileSystem = fileSystem ?? new FileSystem();
    }

    public void DeserializeFromFolder(string folder)
    {
        results.Clear();
        foreach (var item in fileSystem.Directory.GetFiles(folder, "*.json"))
        {
            var content = fileSystem.File.ReadAllText(item);
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
            var existing = results
                .Where(it=>Path.GetFileName(it.Key.file) == Path.GetFileName(p.file) && it.Key.arguments == p.arguments)
                .ToArray();
            if (existing.Length == 0)
            {
                throw new ArgumentException("Cannot find output for " + p);
            }
            else
            {
                if(existing.Length == 1)return existing[0].Value.result ?? throw new InvalidOperationException("Deserialized object missing ProcessExecutionResult");
                throw new ArgumentException("Cannot find output for " + p);

            }
        }
    }

    public Task<ProcessExecutionResult> RunAsync(ProcessStartInfo startInfo, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Run(startInfo));
    }

    
}
