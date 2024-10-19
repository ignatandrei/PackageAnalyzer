try
{ 
    //Console.WriteLine($"Name:{TheAssemblyInfo.GeneratedName}");
    Console.WriteLine($"{TheAssemblyInfo.GeneratedNameNice}");
     
    if (args.Length == 0)
    {
        args = new[] { "-h" };
        //args = new[] { "generateFiles",
        //    "--folder", @"D:\gth\PackageAnalyzer\src\NetPackageAnalyzer\",
        //    "--where", @"D:\gth\PackageAnalyzer\src\documentation1\",
        //    "--verbose","true"
        //};

        // args = new[] { "generateFiles",
        //    "--folder", @"D:\gth\CleanArchitecture",
        //    "--where", @"D:\gth\PackageAnalyzer\src\documentation1",
        //    "--verbose","false"
        // };

        //args = new[] { "generateFiles",
        //    "--folder", @"D:\gth\CleanArchitecture",
        //    "--where", @"D:\gth\PackageAnalyzer\src\documentation1",
        //    "-wg","HtmlSummary",
        //    "--verbose","false"
        //};

        //args = new[] { "generateFiles",
        //    "--folder", @"D:\gth\evolutionary-architecture-by-example\Chapter-1-initial-architecture\Src",
        //    "--where", @"D:\gth\PackageAnalyzer\src\documentation1",
        //    "-wg","HtmlSummary",
        //    "--verbose","false"
        //};
        //args = new[] { "generateFiles",
        //        "--folder", @"D:\gth\nopCommerce\src",
        //        "--where", @"D:\gth\PackageAnalyzer\src\documentation1",
        //        "-wg","HtmlSummary",
        //        "--verbose","false"
        //    };

        //args = new[] { "generateFiles",
        //    "--folder", @"D:\gth\dotnet-starter-kit",
        //    "--where", @"D:\gth\PackageAnalyzer\src\documentation1",
        //    "-wg","HtmlSummary",
        //    "--verbose","false"
        //};
        //args = new[] { "generateFiles",
        //    "--folder", @"D:\gth\booking-microservices",
        //    "--where", @"D:\gth\PackageAnalyzer\src\documentation1",
        //    "-wg","HtmlSummary",
        //    "--verbose","false"
        //};

        //args = new[] { 
        //    //"[parse]",
        //    "showConsole",
        //    "--folder", @"D:\gth\ContosoUniversityDotNetCore-Pages",
        //    "--verbose","false"
        //}; 

        //args = new[] { "generateFiles",
        //    "--folder", @"D:\gth\eShop",
        //    "-wg","HtmlSummary",
        //    "--where", @"D:\gth\PackageAnalyzer\src\documentation1",
        //    "--verbose","false"
        //};
        args = new[] { "generateFiles",
            "--folder", @"D:\gth\TILT\src\backend\Net6\NetTilt",
            "-wg","HtmlSummary",
            "--where", @"D:\gth\PackageAnalyzer\src\documentation1",
            "--verbose","false"
        };
        //args = new[] { "generateFiles",
        //    "--folder", @"D:\gth\RSCG_Examples\v2",
        //    "-wg","HtmlSummary",
        //    "--where", @"D:\gth\PackageAnalyzer\src\documentation1",
        //    "--verbose","false"
        //}; 
        //args = new[] { "generateFiles",
        //    "--folder", @"D:\source\test\NetPackageAnalyzer",
        //    "-wg","HtmlSummary",
        //    "--where", @"D:\gth\PackageAnalyzer\src\documentation1",
        //    "--verbose","false"
        //};
    }
    return await RealMainExecuting.RealMain(args);
} 
catch (Exception ex) 
{ 
    Console.WriteLine(ex.Message);
    //WhatIAmDoingData.DisplayData.DisplayJustErrors();
    return 1;
} 