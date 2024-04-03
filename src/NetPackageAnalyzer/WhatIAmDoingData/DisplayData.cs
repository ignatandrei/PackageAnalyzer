using RSCG_WhatIAmDoing_Common;
namespace WhatIAmDoingData;

public class DisplayData
{
    public static void DisplayJustErrors()
    {
        var data = CachingData.MethodsError().ToArray();
        Console.WriteLine("methods:" + data.Length);
        foreach (var item in data)
        {
            //if ((item.State & AccumulatedStateMethod.RaiseException) != AccumulatedStateMethod.RaiseException)
            //{
            //    Console.WriteLine("not interested in " + item.typeAndMethodData.MethodName);
            //    continue;
            //}
            Console.WriteLine($"{item.typeAndMethodData.MethodName} {item.State}");
            Console.WriteLine($"Method {item.typeAndMethodData.MethodName} from class {item.typeAndMethodData.TypeOfClass} Time: {item.StartedAtDate} state {item.State} ");
            Console.WriteLine($"  =>Arguments: {item.ArgumentsAsString()}");
            

        }
    }
}