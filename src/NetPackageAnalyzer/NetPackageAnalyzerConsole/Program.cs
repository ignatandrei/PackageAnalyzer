try
{
    if (args.Length == 0)
    {
        args = new[] { "-h" };
        //args = new[] { "generateFiles",
        //    "--folder", @"D:\gth\PackageAnalyzer\src\NetPackageAnalyzer\",
        //    "--where", @"D:\gth\PackageAnalyzer\src\documentation1\",
        //    "--verbose","true"
        //};
        //args = new[] { "generateFiles",
        //    "--folder", @"D:\gth\ContosoUniversityDotNetCore-Pages",
        //    "--where", @"D:\gth\PackageAnalyzer\src\documentation1",
        //    "--verbose","false"
        //};

        args = new[] { "generateMajorDifferences",
            "--folder", @"D:\gth\ContosoUniversityDotNetCore-Pages",
            "--verbose","false"
        };


    }
    return await RealMainExecuting.RealMain(args);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    WhatIAmDoingData.DisplayData.DisplayJustErrors();
    return 1;
}