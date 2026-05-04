using NPA.ProcessRunner;

namespace NetPackageAnalyzerObjects;
public partial class ProcessOutput
{
    private readonly IProcessRunner processRunner;

    public ProcessOutput(IProcessRunner? processRunner = null)
    {
        this.processRunner = processRunner ?? new SystemProcessRunner();
    }

    public string ListSDKS(string folder)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "dotnet.exe",
            WorkingDirectory = folder,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = $"--list-sdks"
        };

        var result = processRunner.Run(startInfo);
        string output = result.StandardOutput;
        string errorOutput = result.StandardError;
        if (errorOutput.Length > 0)
        {
            return errorOutput;
        }
        return output;
    }
    public bool Build(string folder)
    {

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "dotnet.exe",
            WorkingDirectory = folder,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = $"build"
        };

        var result = processRunner.Run(startInfo);
        string output = result.StandardOutput;
        string errorOutput = result.StandardError;
        Console.WriteLine("output of build: ---" );
        Console.WriteLine(output);
        Console.WriteLine("errorOutput of build: ---");
        Console.WriteLine(errorOutput);
        Console.WriteLine("-----");    
        if (errorOutput.Length > 0)
        {
            return false;
        }
        return true;
    }
    public string OutputDotnetReference(string pathToCsproj)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "dotnet.exe",
            WorkingDirectory = Path.GetDirectoryName(pathToCsproj),
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = $"list \"{pathToCsproj}\" reference "
        };

        var result = processRunner.Run(startInfo);
        string output = result.StandardOutput;
        string errorOutput = result.StandardError;
        if (errorOutput.Length > 0)
        {
            throw new Exception(errorOutput);
        }
        return output;
    }
    public string[] ListOfProjects(string slnFile)
    {
        var folder = Path.GetDirectoryName(slnFile);
        var nameFile = Path.GetFileName(slnFile);
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "dotnet.exe",
            WorkingDirectory = folder,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = $"solution {nameFile} list" 
        };
        Console.WriteLine("analyzing output of dotnet " + startInfo.Arguments + " in folder " + folder);
        var result = processRunner.Run(startInfo);
        string output = result.StandardOutput;
        string errorOutput = result.StandardError;

        if (errorOutput.Length > 0)
        {
            throw new Exception(errorOutput);
        }
        return output.Split('\n',StringSplitOptions.RemoveEmptyEntries)
            .Select(it=>it.Replace('\r',' '))
            .Where(it=>!string.IsNullOrWhiteSpace(it))
            .Skip(2) // Projects: ---- 
            .Select(it=>Path.Combine(folder!,it))            
            .ToArray();

    }
    public string OutputDotnetPackage(string folder, PackageOptions packageOptions)
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
        Console.WriteLine("analyzing output of dotnet " + startInfo.Arguments +" in folder " + folder);
        var result = processRunner.Run(startInfo);
        string output = result.StandardOutput;
        string errorOutput = result.StandardError;
        if (errorOutput.Length > 0)
        {
            throw new Exception(errorOutput);
        }
        if (output.StartsWith("error:"))
        {
            throw new ExceptionReadingPackages("Error reading packages" + output);
        }
        return output;
    }
    public string OutputWhy(string pathToSln, string packageId)
    {
        string folder = Path.GetDirectoryName(pathToSln)??"";
        string nameSln = Path.GetFileName(pathToSln);
        string arg = $"nuget why {nameSln} {packageId}";
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "dotnet.exe",
            WorkingDirectory = folder,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = arg
        };
        Console.WriteLine("analyzing output of dotnet " + startInfo.Arguments + " in folder " + folder);
        var result = processRunner.Run(startInfo);
        string output = result.StandardOutput;
        string errorOutput = result.StandardError;
        if (errorOutput.Length > 0)
        {
            throw new Exception(errorOutput);
        }
        return output;
    }

}


