
namespace NetPackageAnalyzerObjects;
public class ProcessOutput
{
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

        // Create and start the process
        Process process = new Process
        {
            StartInfo = startInfo
        };
        process.Start();

  

        // Wait for the process to exit
        process.WaitForExit();
        // Read the output
        string output = process.StandardOutput.ReadToEnd();
        string errorOutput = process.StandardError.ReadToEnd();
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
        // Create and start the process
        Process process = new Process
        {
            StartInfo = startInfo
        };
        process.Start();

        
        // Wait for the process to exit
        process.WaitForExit();
        // Read the output
        string output = process.StandardOutput.ReadToEnd();
        string errorOutput = process.StandardError.ReadToEnd();

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


