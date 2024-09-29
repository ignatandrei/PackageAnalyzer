
using Microsoft.Extensions.Logging;
using NetPackageAnalyzerExportHTML;
using System.Runtime.InteropServices;

namespace NetPackageAnalyzerConsole;
internal class RealMainExecuting
{
    static Option<bool> verbose = new("--verbose", "Show verbose output");
    static Option<string> folderToHaveSln = new
            (name: "--folder",
            description: "folder where find the solution .sln",
            getDefaultValue: () => Environment.CurrentDirectory);

    static RealMainExecuting()
    {
        folderToHaveSln.AddAlias("-f");
        verbose.AddAlias("-v");
    }
    private static Command AddGenerateFiles()
    {
        Command cmdGenerate = new("generateFiles", "Generate files for documentation");
        cmdGenerate.AddAlias("gf");

        var folderGenerate = new Option<string>
        (name: "--where",
        description: "where generated files"
        //getDefaultValue: () => Path.Combine(Environment.CurrentDirectory, "Analysis")
        );
        folderGenerate.AddAlias("-w");

        cmdGenerate.AddOption(folderGenerate);

        var generateData = new Option<WhatToGenerate>
        (name: "--whatGenerate",
        description: "what to generate - 1= docusaurus , 2=summary"
        //,getDefaultValue: () => WhatToGenerate.Docusaurus
        );


        generateData.AddAlias("-wg");
        cmdGenerate.AddOption(generateData);

        var runProduct = new Option<bool>
        (name: "--runProduct",
        description: "run product after",
        getDefaultValue: () => false);

        runProduct.AddAlias("-rp");


        cmdGenerate.AddOption(runProduct);
        cmdGenerate.SetHandler(GenerateHandler,
            folderToHaveSln,
            folderGenerate,
            generateData,
            verbose,
            runProduct);

        return cmdGenerate;

    }
    private static Command AddGenerateConsole()
    {

        Command cmdGenerate = new("showConsole", "Show data in Console");
        cmdGenerate.AddAlias("sc");
        var generateData = new Option<EConsoleToGenerate>
        (name: "--whatGenerate",
        description: "what to generate",
        getDefaultValue: () => EConsoleToGenerate.MajorVersionDiffer);
        cmdGenerate.AddOption(generateData);
        cmdGenerate.SetHandler(GenerateHandlerMajorDiff,
            verbose,
            folderToHaveSln
            );

        return cmdGenerate;
    }

    private static async Task GenerateHandlerMajorDiff(bool verbose, string folder)
    {
        //DisplayData.Verbose = verbose;
        //if (verbose)
        //{
        //    Console.WriteLine("Please see verbose file at " + DisplayData.VerboseFile());
        //}
        var fs = new FileSystem();
        GenerateData? g = new(fs);
        bool b = await g.GenerateDataForSln(folder);
        if (!b)
        {
            Console.WriteLine("not capable to generate data");
            return;
        }
        WriteToConsole writeToConsole = new(g);
        writeToConsole.WriteMajorDiffers();
        return;
    }

    [InterceptMethod(WhatToIntercept.Timing)]
    public static async Task<int> RealMain(string[] args)
    {
        GlobalsForGenerating.Version = ThisAssembly.Info.Version.ToString();
        WriteLine("Version:" + ThisAssembly.Info.Version.ToString());
        RootCommand rootCommand = new();
        rootCommand.AddGlobalOption(verbose);
        rootCommand.AddGlobalOption(folderToHaveSln);



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

        rootCommand.Add(AddGenerateFiles());
        rootCommand.Add(AddGenerateConsole());
        //rootCommand.Add(cmdAnalyzeBranch);
        if (args.Length == 0)
        {
            args = ["-h"];

        }
        WriteLine("args:" + string.Join(" ", args));
        await rootCommand.InvokeAsync(args);
        return 0;
    }
    private static async Task GenerateHandler(string folder, string where, WhatToGenerate what, bool verbose, bool runProduct)
    {
        try
        {
            //DisplayData.Verbose = verbose;
            //if (verbose)
            //{
            //    Console.WriteLine("Please see verbose file at " + DisplayData.VerboseFile());
            //}
            await RealGenerateHandler(folder, where, what);
            if (runProduct)
            {
                WriteLine("running product");
                var p = new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = $"/c npm i && npm run start",
                    WorkingDirectory = where,
                    UseShellExecute = false,
                    CreateNoWindow = false
                };
                Process.Start(p);
            }
        }
        catch (Exception ex)
        {
            WriteLine("Exception!! " + ex.Message);
            WriteLine("Exception!! " + ex.StackTrace);

        }
    }
    private static void LaunchBrowser(string url)
    {
        // Windows
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start("cmd", new[] { "/C", "start", url });
            return;
        }

        // Linux
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
            return;
        }

        // OSX
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
            return;
        }

    }

    
    private static async Task RealGenerateHandler(string folder, string where, WhatToGenerate what)
    {

        where = string.IsNullOrWhiteSpace(where) ? Path.Combine(folder, "Analysis") : where;

        WriteLine($"analyzing {folder}");
        GenerateFiles? g = null;
        switch (what)
        {
            case WhatToGenerate.Docusaurus:
                g = new GenerateFilesDocusaurus(new FileSystem());
                break;
            case WhatToGenerate.HtmlSummary:
                g = new GenerateHTML(new FileSystem());
                break;
            default:
                throw new NotImplementedException($"what={what}");
        }
        GenerateData tempWIAD = g as GenerateData;
        bool b = await tempWIAD.GenerateDataForSln(folder);
        if (!b)
        {
            Console.WriteLine("not capable to generate data");
            return;
        }
        tempWIAD.AddHistoryCsproj(); 
        
        var data= await g.GenerateNow(folder, where);
        switch (what)
        {
            case WhatToGenerate.Docusaurus:
                WriteLine($"now npm i && npm run start in  {where}");
                break;
            case WhatToGenerate.HtmlSummary:
                WriteLine($"start {data}");
                LaunchBrowser(data);
                break;
            default:
                throw new NotImplementedException($"what={what}");
        }


    }
}
