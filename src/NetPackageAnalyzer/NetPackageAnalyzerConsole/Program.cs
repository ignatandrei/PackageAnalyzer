try
{
    return await RealMainExecuting.RealMain(args);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    WhatIAmDoingData.DisplayData.DisplayJustErrors();
    return 1;
}