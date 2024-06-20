try
{
    if(args.Length == 0)
    {
        args = new[] { "generateFiles",
            "--folder", @"D:\gth\PackageAnalyzer\src\NetPackageAnalyzer\",
            "--where", @"D:\gth\PackageAnalyzer\src\documentation1\",
            "--verbose","true"
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