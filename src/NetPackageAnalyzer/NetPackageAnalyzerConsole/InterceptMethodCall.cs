namespace RSCG_InterceptMethodCommon;
internal class InterceptMethodCall
{
    public static void InterceptMethod(DataFromInterceptTiming data)
    {
        var duration  = data.EndTime-data.StartTime;
        Console.WriteLine("generation duration seconds: " + duration.TotalSeconds.ToString("#"));
        //Console.WriteLine("method called: " + data.ClassName + "." + data.MethodName);
        //Console.WriteLine("start time: " + data.StartTime);
        //Console.WriteLine("end time: " + data.EndTime);
    }
}
