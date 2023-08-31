public class Program
{
    static async Task<int> Main(string[] args)
    {
        
        args = new[]
        {
          @"generateFiles",
            "--folder",
            @"C:\gth\TILT\src\backend\Net7\NetTilt"

        };
        /*
        args = new[]
        {
          @"generateFiles",
            "--folder",
            @"C:\gth\PackageAnalyzer\src\NetPackageAnalyzer\"

        };
        */
        RootCommand rootCommand = new();
        Command cmdGenerate = new("generateFiles", "Generate files for documentation");

        rootCommand.Add(cmdGenerate);

        var folderToHaveSln = new Option<string>
            (name: "--folder",
            description: "folder where find the solution .sln",
            getDefaultValue: () => Environment.CurrentDirectory);
        folderToHaveSln.AddAlias("-f");

        cmdGenerate.AddOption(folderToHaveSln);

        cmdGenerate.SetHandler(async (folder) =>
        {
            WriteLine($"analyzing {folder}");
            var g = new GenerateFiles();
            if (await g.GenerateData(folder))
            {
                await g.GenerateNow(folder);
            }
            else
            {
                Console.WriteLine("not capable to generate data");
            }

        }, folderToHaveSln);
        
        await rootCommand.InvokeAsync(args);
        
        return 0;

    }
}