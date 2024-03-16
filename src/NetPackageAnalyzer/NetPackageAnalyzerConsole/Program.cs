using LibGit2Sharp;
using RSCG_WhatIAmDoing_Common;

public class Program
{
    static async Task<int> Main(string[] args)
    {
        try
        {
            return await RealMain(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            WhatIAmDoingData.DisplayData.DisplayJustErrors();

            return 1;
        }
    }
    static async Task<int> RealMain(string[] args)
    {
        GlobalsForGenerating.Version = ThisAssembly.Info.Version.ToString();
        WriteLine("Version:"+ThisAssembly.Info.Version.ToString());        
        RootCommand rootCommand = new();
        Command cmdGenerate = new("generateFiles", "Generate files for documentation");
        cmdGenerate.AddAlias("gf");
        
        
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
        folderGenerate.AddAlias("-w");

        cmdGenerate.AddOption(folderGenerate);

        var generateData = new Option<WhatToGenerate>
        (name: "--whatGenerate",
        description: "what to generate - 1= docusaurus",
        getDefaultValue: () => WhatToGenerate.Docusaurus);

        generateData.AddAlias("-wg");

        cmdGenerate.AddOption(generateData);
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
        
        cmdGenerate.SetHandler(GenerateHandler, folderToHaveSln,folderGenerate, generateData);

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
            args = new[] { "generateFiles",
                "--folder", @"D:\gth\PackageAnalyzer\src\NetPackageAnalyzer\",
                //"--folder",@"D:\gth\PackageAnalyzer\src\documentation1\",
                "--where", @"D:\gth\PackageAnalyzer\src\documentation1\",
            };

        }
        WriteLine("args:" + string.Join(" ",args));
        await rootCommand.InvokeAsync(args); 
        return 0;
    }
    private static async Task GenerateHandler(string folder, string where, WhatToGenerate what)
    {
        try
        {
            await RealGenerateHandler(folder, where, what);
        }
        catch (Exception ex)
        {
            WriteLine("Exception!! "+ex.Message);
            
            

        }
    }
    private static async Task RealGenerateHandler(string folder, string where, WhatToGenerate what)
    {

        WriteLine($"analyzing {folder}");
        GenerateFiles? g = null;
        switch (what)
        {
            case WhatToGenerate.Docusaurus:
                g = new GenerateFilesDocusaurus(new FileSystem());
                break;
            default:
                throw new NotImplementedException($"what={what}");
        }
        if (!await g.GenerateData(folder))
        {
            Console.WriteLine("not capable to generate data");
            return;
        }
        await g.GenerateNow(folder, where);
   }
}