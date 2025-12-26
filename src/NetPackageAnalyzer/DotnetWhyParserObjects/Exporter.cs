using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;

namespace DotnetWhyParserObjects;

record TowerFromTo(string From, string To, bool isOnDependencyGraph)
{
    
};
public class DotnetWhyExporter
{
    private IEnumerable<string> ShortNames()
    {        
        for (char letter = 'A'; letter <= 'Z'; letter++)
        {
            for (char letter2 = 'A'; letter2 <= 'Z'; letter2++)
            {
                yield return $"{letter}{letter2}";
            }
        }
    }
    //private IEnumerable<T> VisitNodes<T>(DependencyNode[] nodes ,Func<DependencyNode?, T> predicate)
    //{
    //    foreach (var dep in nodes)
    //    {
    //        var node = dep;
    //        yield return predicate(dep);
    //        while (node != null)
    //        {
    //            node = node.Dependency;
    //            yield return predicate(node);
    //        }
    //    }
    //}
    public string[] ExportToLines(DotnetWhyOutput output, string separatorNode)
    {
        var deps = output.SmallestProjectPathsToPackage();
        int lenght = deps.Length;
        if (lenght == 0) return Array.Empty<string>();
        string[] ret =new string[lenght] ;
        int nr = 0;
        foreach (var dep in deps)
        {
            var node = dep;
            var res = $"{node.MyName}{separatorNode}";
            while (node.Dependency != null)
            {
                node = node.Dependency;
                res += $"{node.MyName}{separatorNode}";

            }
            res = res.Substring(0, res.Length - separatorNode.Length);
            ret[nr++] =res;
        }
        return ret;
    }
    public string ExportToLines(DotnetWhyOutput output,string separatorNode, string separatorHead)
    {
        return string.Join(separatorHead, ExportToLines(output, separatorNode));
    }

    public string ExportToMermaidSmallestProjects(DotnetWhyOutput output)
    {
        string targetPackage = output.TargetPackage;
        var deps = output.SmallestProjectPathsToPackage();
        var names = deps.Select(it=>it.MyName).ToList();
        names.AddRange(deps
            .SelectMany(it => it.AllChildren())
            .Select(it=>it.MyName)
            ); 
        names = names.Distinct().ToList();
        var shorts = ShortNames().Take(names.Count+1).ToArray();
        Dictionary<string, string> nodesHash =
            names
            .Select((it, index) => new { it, shortname = shorts[index] })
            .ToDictionary(it => it.it, it => it.shortname);
        
        nodesHash.Add(targetPackage, shorts[shorts.Length-1] );

        Dictionary<string, string> ClassNamesForPackages = new();

        string nodesString = string.Join(Environment.NewLine,
            nodesHash.Select(it =>
            {
                string separator1 = it.Key.Contains(",") ? "([" : "[[";
                string separator2 = it.Key.Contains(",") ? "])" : "]]";
                string classData = it.Key.Contains(",") ? "package" : "proj";
                if (it.Key.StartsWith(targetPackage + ","))
                {
                    if (!ClassNamesForPackages.ContainsKey(it.Key))
                    {
                        ClassNamesForPackages.Add(it.Key, "finalPack" + (ClassNamesForPackages.Count + 1));
                    }
                    classData = ClassNamesForPackages[it.Key];
                }
                return $$"""
            {{it.Value}}{{separator1}}"{{it.Key}}"{{separator2}}:::{{classData}}
            """;
            }));
        List<string> result = new();
        string root = "";
        foreach (var dep in deps)
        {
            string edges = "";
            var node = dep;
            while (node.Dependency != null)
            {
                edges += nodesHash[node.MyName]+  " --> " + nodesHash[node.Dependency.MyName];
                edges += Environment.NewLine;
                node = node.Dependency;
            }
            root += nodesHash[node.MyName]+ " --> " + nodesHash[targetPackage];
            root += Environment.NewLine;
            var subGraph = $$"""
subgraph "{{node.Name+","+node.Version}}"
    direction TB  
    {{edges}}
end
""";
            result.Add(subGraph);
        }

        var resString = $$"""
flowchart 
    direction TB
    classDef proj stroke:#0f0
    classDef package stroke:#0f0
    
    classDef finalPack1 fill:#f00
    classDef finalPack2 fill:#0f0
    classDef finalPack3 fill:#00f
    
    classDef proj stroke:#00f
    classDef package stroke:#0f0
    {{nodesString}}
    {{root}}
    {{string.Join(Environment.NewLine, result)}}
""";
        return resString;
    }

}
