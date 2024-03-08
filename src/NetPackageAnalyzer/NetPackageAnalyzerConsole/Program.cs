using AnalyzeMerge;

public class Program
{
    static async Task<int> Main(string[] args)
    { 
        
        WriteLine("Version:"+ThisAssembly.Info.Version.ToString());
        //args = new[]
        //{
        //  @"generateFiles",
        //    "--folder",
        //    @"C:\gth\TILT\src\backend\Net7\NetTilt"

        //};
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

        
        var folderToHaveSln = new Option<string>
            (name: "--folder",
            description: "folder where find the solution .sln",
            getDefaultValue: () => Environment.CurrentDirectory);
        folderToHaveSln.AddAlias("-f");

        cmdGenerate.AddOption(folderToHaveSln);

        var folderGenerate = new Option<string>
        (name: "--where",
        description: "where generated files",
        getDefaultValue: () => Path.Combine(Environment.CurrentDirectory,"Analysis"));
            folderToHaveSln.AddAlias("-w");

        cmdGenerate.AddOption(folderGenerate);
        //cmdGenerate.Handler = CommandHandler.Create<string,string>(async (folder,where) =>
        //{
        //    WriteLine($"analyzing {folder}");
        //    var g = new GenerateFiles();
        //    if (await g.GenerateData(folder))
        //    {
        //        await g.GenerateNow(folder,where);
        //    }
        //    else
        //    {
        //        Console.WriteLine("not capable to generate data");
        //    }

        //});
        cmdGenerate.SetHandler(async (string folder, string where) =>
        {
            
            WriteLine($"analyzing {folder}");
            var g = new GenerateFiles();
            if (await g.GenerateData(folder))
            {
                await g.GenerateNow(folder,where);
            }
            else
            {
                Console.WriteLine("not capable to generate data");
            }

        }, folderToHaveSln,folderGenerate);

        //Command cmdAnalyzeBranch = new("analyzeBranch", "Analyze branch");

        //var cmdAnalyzeBranchFolder = new Option<string>
        //    (name: "--folder",
        //    description: "folder where branch is ",
        //    getDefaultValue: () => Environment.CurrentDirectory);
         
        //cmdAnalyzeBranch.AddAlias("-f");
        //cmdAnalyzeBranch.AddOption(cmdAnalyzeBranchFolder);
        //cmdAnalyzeBranch.SetHandler(async (folder) =>
        //{          
        //    //folder = @"C:\gth\PackageAnalyzer\"; 
        //    var g = new AnalyzeMergeData(folder);
        //    await g.GenerateNow();


        //}, cmdAnalyzeBranchFolder);

        rootCommand.Add(cmdGenerate);
        //rootCommand.Add(cmdAnalyzeBranch);
        if(args.Length == 0)
        {
            args = ["-h"];
            args= new[] { "generateFiles", 
                "--folder", @"D:\gth\PackageAnalyzer\src\NetPackageAnalyzer\",
                "--where", @"D:\gth\PackageAnalyzer\docs\documentation\docs\Analysis",
            };
            
        }
        WriteLine("args:" + string.Join(" ",args));
        await rootCommand.InvokeAsync(args); 
        return 0;
    }
}