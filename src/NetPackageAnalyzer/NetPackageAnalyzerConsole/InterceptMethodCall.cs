namespace RSCG_InterceptMethodCommon;
internal class InterceptMethodCall
{
    public static void InterceptMethod(DataFromInterceptTiming data)
    {
        var duration  = data.EndTime-data.StartTime;
        Console.WriteLine($"{TheAssemblyInfo.GeneratedNameNice}");
        var durat = duration.TotalSeconds; 
        if(durat< 120)
        {
            Console.WriteLine("Generated duration seconds: " + duration.TotalSeconds.ToString("#"));
        }
        else
        {
            Console.WriteLine("Generated duration minutes: " + duration.TotalMinutes.ToString("#"));
        }

        //Console.WriteLine("method called: " + data.ClassName + "." + data.MethodName);
        //Console.WriteLine("start time: " + data.StartTime);
        //Console.WriteLine("end time: " + data.EndTime);
    }
}
