using AnalyzeMerge;
public enum WhatToGenerate
{
    Default = 0,
    Docusaurus = 1,
}
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

        var generateData = new Option<WhatToGenerate>
        (name: "--whatGenerate",
        description: "what to generate - 0 = markdown, 1= docusaurus",
        getDefaultValue: () => 0);
        folderGenerate.AddAlias("-wg");

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
        cmdGenerate.SetHandler(async (string folder, string where,  WhatToGenerate what) =>
        {
            
            WriteLine($"analyzing {folder}");
            var g = new GenerateFiles();
            if (!await g.GenerateData(folder))
            {
                Console.WriteLine("not capable to generate data");
                return;
            }
            await g.GenerateNow(folder, where);


        }, folderToHaveSln,folderGenerate, generateData);

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
            //args= new[] { "generateFiles", 
            //    "--folder", @"D:\gth\PackageAnalyzer\src\NetPackageAnalyzer\",
            //    "--where", @"D:\gth\PackageAnalyzer\src\documentation\docs\Analysis",
            //};
            
        }
        WriteLine("args:" + string.Join(" ",args));
        await rootCommand.InvokeAsync(args); 
        return 0;
    }
}