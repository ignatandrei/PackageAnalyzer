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
            args= new[] { "-h" };
            //args= new[] { "generateFiles", "--folder", @"D:\gth\PackageAnalyzer\src\NetPackageAnalyzer\" };
            
        }
        WriteLine("args:" + string.Join(" ",args));
        await rootCommand.InvokeAsync(args); 
        return 0;
    }
}