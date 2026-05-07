

using static System.Runtime.InteropServices.JavaScript.JSType;
using NPA.ProcessRunner;

namespace NetPackageAnalyzerConsole;
internal class RealMainExecuting
{
    internal static IProcessRunner ProcessRunner { get; set; }=default!;
    static Option<bool> verbose = new("--verbose", "-v");
    static Option<string> folderToHaveSln = new
            (name: "--folder","-f"            
            );

    static RealMainExecuting()
    {
        folderToHaveSln.Description = "folder to have sln file";
        folderToHaveSln.DefaultValueFactory = (_) => Environment.CurrentDirectory;
        verbose.Description= "verbose output";
    }
    private static Command AddGenerateFiles()
    {
        Command cmdGenerate = new("generateFiles", "gf");
        cmdGenerate.Description= "Generate files for solution";

        cmdGenerate.Options.Add(folderToHaveSln);
        cmdGenerate.Options.Add(verbose);

        var folderGenerate = new Option<string>
        (name: "--where", "-w"
        
        //getDefaultValue: () => Path.Combine(Environment.CurrentDirectory, "Analysis")
        );
        folderGenerate.Description = "where generated files";

        cmdGenerate.Options.Add(folderGenerate);

        var generateData = new Option<WhatToGenerate>
        (name: "--whatGenerate", "-wg"
       
        );


        generateData.Description = "what to generate - 1= docusaurus , 2=summary";
        generateData.DefaultValueFactory = (_) => WhatToGenerate.Docusaurus;
        cmdGenerate.Options.Add(generateData);

        var runProduct = new Option<bool>
        (name: "--runProduct", "-rp");
        runProduct.Description= "run product after";
        

        runProduct.DefaultValueFactory=(_)=>false;


        cmdGenerate.Options.Add(runProduct);
        cmdGenerate.SetAction(pr =>
        {
            var folder = pr.GetValue(folderToHaveSln)!;
            var where = pr.GetValue(folderGenerate)!;
            var what = pr.GetValue(generateData);
            var verb = pr.GetValue(verbose);
            var runProd = pr.GetValue(runProduct);
            return GenerateHandler(folder, where, what, verb, runProd);
        });
        
        return cmdGenerate;

    }
    private static Command AddGenerateConsole()
    {

        Command cmdGenerate = new("showConsole", "Show data in Console");
        cmdGenerate.Aliases.Add("sc");
        var generateData = new Option<EConsoleToGenerate>
        (name: "--whatGenerate", "-wg");
        generateData.Description = "what to generate";
        generateData.DefaultValueFactory= (_) => EConsoleToGenerate.MajorVersionDiffer;
        cmdGenerate.Options.Add(generateData);
        cmdGenerate.SetAction(pr =>
        {
            var v = pr.GetValue(verbose);
            var f = pr.GetValue(folderToHaveSln)!;
            return GenerateHandlerMajorDiff(v,f);
        });
        //cmdGenerate.SetHandler(GenerateHandlerMajorDiff,
        //    verbose,
        //    folderToHaveSln
        //    );

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
        GenerateData? g = new(fs,ProcessRunner);
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
        ProcessRunner??= new SystemProcessRunner();
        GlobalsForGenerating.Version = ThisAssembly.Info.Version.ToString();
        GlobalsForGenerating.NameVersion = TheAssemblyInfo.GeneratedNameNice;
        WriteLine("Version:" + ThisAssembly.Info.Version.ToString());
        RootCommand rootCommand = new();
        rootCommand.Options.Add(verbose);
        rootCommand.Options.Add(folderToHaveSln);
        rootCommand.Description= @$"
Generate documetation for a solution
Please use the following commands in the folder with the sln file

    dotnet PackageAnalyzer generateFiles -wg HtmlSummary

See documentation at https://github.com/ignatandrei/PackageAnalyzer

";
        


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
        ParseResult parseResult = rootCommand.Parse(args);
        await parseResult.InvokeAsync();
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
            await RealGenerateHandler(folder, where, what, ProcessRunner);
            if (runProduct)
            {
                WriteLine("running product");
                ProcessRunner.Start(CreateRunProductStartInfo(where));
            }
        }
        catch (Exception ex)
        {
            WriteLine("Exception!! " + ex.Message);
            WriteLine("Exception!! " + ex.StackTrace);

        }
    }
    internal static ProcessStartInfo CreateRunProductStartInfo(string where) =>
        new()
        {
            FileName = "cmd",
            Arguments = "/c npm i && npm run start",
            WorkingDirectory = where,
            UseShellExecute = false,
            CreateNoWindow = false
        };
    internal static void LaunchBrowser(string url)
    {
        ProcessRunner.Start(CreateBrowserLaunchStartInfo(url));
    }
    internal static ProcessStartInfo CreateBrowserLaunchStartInfo(string url)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var startInfo = new ProcessStartInfo("cmd")
            {
                UseShellExecute = false
            };
            startInfo.ArgumentList.Add("/C");
            startInfo.ArgumentList.Add("start");
            startInfo.ArgumentList.Add(url);
            return startInfo;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            var startInfo = new ProcessStartInfo("xdg-open")
            {
                UseShellExecute = false
            };
            startInfo.ArgumentList.Add(url);
            return startInfo;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var startInfo = new ProcessStartInfo("open")
            {
                UseShellExecute = false
            };
            startInfo.ArgumentList.Add(url);
            return startInfo;
        }

        throw new PlatformNotSupportedException("Browser launch is not supported on this platform.");
    }

    
    private static async Task RealGenerateHandler(string folder, string where, WhatToGenerate what, IProcessRunner? processRunner = null)
    {
        processRunner = processRunner ?? new SystemProcessRunner();
        var fileSystem = new FileSystem();

        where = string.IsNullOrWhiteSpace(where) ? Path.Combine(folder, "Analysis") : where;
        try
        {
            var s = GitInfo.Construct(folder,processRunner);

            Console.WriteLine($"Repository:{s.Repository}");
            Console.WriteLine($"Branch:{s.Branch}");
            Console.WriteLine($"Commit:{s.Commit}");
            GlobalsForGenerating.gitInfo = s;
        }
        catch(Exception ex)
        {
            Console.WriteLine("Exception finding git " + ex.Message);
        }
        WriteLine($"analyzing {folder}");
        GenerateFiles? g = null;
        switch (what)
        {
            case WhatToGenerate.Docusaurus:
                g = new GenerateFilesDocusaurus(fileSystem);
                break;
            case WhatToGenerate.HtmlSummary:
                g = new GenerateHTML(fileSystem, ProcessRunner);
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
                
                await ReplaceHtmlSummary(data, fileSystem);
                LaunchBrowser(data);
                break;
            default:
                throw new NotImplementedException($"what={what}");
        }


    }
    private static async Task<bool> ReplaceHtmlSummary(string filePath, IFileSystem fileSystem)
    {
        string nameSol = GlobalsForGenerating.NameSolution;
        if(!fileSystem.File.Exists(filePath)) return false;
        fileSystem.File.Copy(filePath, filePath + ".bak",true);
        var content = await fileSystem.File.ReadAllTextAsync(filePath);
        content = content.Replace("driverObj.drive();", "driverObj.drive();var tabs = new Tabby('[data-tabs]');");
        var lines = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string?[] outputLine = new string[lines.Length];
        for(int i=0; i<lines.Length; i++)
        {
            var line = lines[i];
            #region replace all extracted images
            var titleToReplace = """title="image """;
            if (line.Contains(titleToReplace) && !line.Contains(" stay "))
            {
                //find title
                var index = line.IndexOf(titleToReplace);
                var next = line.IndexOf('"', index + titleToReplace.Length + 1);
                var title = line.Substring(index + titleToReplace.Length, next - titleToReplace.Length - index);
                var nameFile = title.Replace(" ", "-");
                nameFile += ".png";
                outputLine[i] = $"<img title='{title}' src='images_{nameSol}_summary/{nameFile}' />";
                while (!line.Contains("</div>")){
                    i++ ;
                    line = lines[i];
                    outputLine[i] = null;
                }
                continue;
            }
            #endregion  
            outputLine[i] = line;
        }

        content = string.Join(Environment.NewLine, outputLine.Where(it => !string.IsNullOrWhiteSpace(it)));
        await fileSystem.File.WriteAllTextAsync(filePath, content);
        
        return true;
    }
}
