using RSCG_WhatIAmDoing_Common;
namespace WhatIAmDoingData;

public class DisplayData
{
    static int count = 0;
    public static string VerboseFile() {
        return Path.Combine(Environment.CurrentDirectory, "WhatIAmDoingData.txt");
    }
    private static void AppendToFile(string file, string text)
    {
        File.AppendAllText(file, text+Environment.NewLine);        
    }
    public static bool Verbose {
        get
        {
            return CachingData.OnMethodCalled != null;
        }
        set
        {
            if(!value)
            {
                CachingData.OnMethodCalled = null;
                return;
            }
            if(value)
            {
                var file = VerboseFile();
                if(File.Exists(file))
                {
                    File.Delete(file);
                }
                CachingData.OnMethodCalled += (id, acc, method) =>
                {
                    if((acc & AccumulatedStateMethod.Finished )== AccumulatedStateMethod.Finished)
                    {
                        return;
                    }
                    
                    if(acc == AccumulatedStateMethod.Started)
                    {
                        count++;
                    }
                    else
                    {
                        acc -= AccumulatedStateMethod.Started;
                    }
                    string space=Enumerable.Repeat("-",count).Aggregate((a,b)=>a+b);
                    AppendToFile(file,$"{space}({id}){acc} at {method.StartedAtDate}");
                    AppendToFile(file,$" => {acc} Method {method.typeAndMethodData.MethodName} from class {method.typeAndMethodData.TypeOfClass}   ");
                    AppendToFile(file,$" => Arguments: {method.ArgumentsAsString()}");
                    if ((acc & AccumulatedStateMethod.HasFinishedWithResult) == AccumulatedStateMethod.HasFinishedWithResult)
                    {
                        var typeResult= method.Result!.GetType();
                        if(typeResult.IsValueType || typeResult.Name=="String")
                        {
                            AppendToFile(file,$" => result: {method.Result}");
                        }
                        else
                        {
                            switch (typeResult.Name)
                            {
                                case "String[]":
                                    var data = (string[])method.Result!;
                                    var res=string.Join(Environment.NewLine,data);
                                    AppendToFile(file, $" => result: {res}");
                                    break;
                                default:
                                    AppendToFile(file,$" => result: {typeResult.Name}");
                                    break;
                            }
                        }
                        //AppendToFile(file,$" => result: {method.Result}");
                    }
                    if ((acc & AccumulatedStateMethod.RaiseException) == AccumulatedStateMethod.RaiseException)
                    {
                        AppendToFile(file,$" => exception: {method.Exception!.Message}");
                    }
                    if ((acc & AccumulatedStateMethod.Finished) == AccumulatedStateMethod.Finished)
                    {
                        count--;
                    }

                };
            }
        }
    }
    public static void DisplayJustErrors()
    {
        var data = CachingData.MethodsError().ToArray();
        Console.WriteLine("methods:" + data.Length);
        foreach (var item in data)
        {
            //if ((item.State & AccumulatedStateMethod.RaiseException) != AccumulatedStateMethod.RaiseException)
            //{
            //    AppendToFile(file,"not interested in " + item.typeAndMethodData.MethodName);
            //    continue;
            //}
            Console.WriteLine($"{item.typeAndMethodData.MethodName} {item.State}");
            Console.WriteLine($"Method {item.typeAndMethodData.MethodName} from class {item.typeAndMethodData.TypeOfClass} Time: {item.StartedAtDate} state {item.State} ");
            Console.WriteLine($"  =>Arguments: {item.ArgumentsAsString()}");
            

        }
    }
}