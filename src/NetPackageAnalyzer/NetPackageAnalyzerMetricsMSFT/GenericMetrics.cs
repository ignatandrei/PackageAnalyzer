using System.Xml;

namespace NetPackageAnalyzerMetricsMSFT;
public class GenericMetrics
{
    public string Name="";
    public Dictionary<string ,Metric> metrics;
    public GenericMetrics()
    {
        metrics = new ()
        {
            { "MaintainabilityIndex", new  ("MaintainabilityIndex")},
            {"CyclomaticComplexity", new ( "CyclomaticComplexity") },
            {"ClassCoupling", new ( "ClassCoupling")},
            {"DepthOfInheritance" ,new ( "DepthOfInheritance") },
            {"SourceLines" ,new ( "SourceLines") },
            { "ExecutableLines",new ( "ExecutableLines") },
        };
        
    }
    private static T CreateFromXML<T>(string name,XmlNode xmlNode)
        where T: GenericMetrics, new()
    {
        T genericMetrics = new();
        genericMetrics.Name = name;
        foreach (var nodeChild in xmlNode.ChildNodes)
        {
            var typeNodeChild = nodeChild as XmlNode;
            if (typeNodeChild == null) continue;
            var nameMetric = typeNodeChild.Attributes!["Name"]!.Value;
            var valueMetric = typeNodeChild.Attributes!["Value"]!.Value; 
            genericMetrics.metrics[nameMetric].Value=int.Parse(valueMetric);
        }
        
        return genericMetrics;
    }
    public static GenericMetrics[] CreateFromXML(string fileName)
    {
        XmlDocument xmlDoc = new ();
        xmlDoc.Load(fileName);
        var element = xmlDoc.DocumentElement;
        if(element == null)return Array.Empty<GenericMetrics>();
        var nodes = element.SelectNodes("//Target");
        if(nodes == null) return Array.Empty<GenericMetrics>();
        List<GenericMetrics> metrics = new();
        foreach (var target in nodes)
        {
            var node = target as XmlNode;
            if (node == null) continue;
            string name = node.Attributes!["Name"]!.Value;
            var assemblyNode=node.SelectSingleNode("Assembly");
            if (assemblyNode == null) continue;
            var data = CreateFromXML<GenericMetricsAssembly>(name, assemblyNode.FirstChild!);
            metrics.Add(data);
            var types = node.SelectNodes("//NamedType");
            if (types == null) continue;
            foreach (var type in types)
            {
                var typeNode = type as XmlNode;
                if (typeNode == null) continue;
                name = node.Attributes!["Name"]!.Value;
                var typeData = CreateFromXML<GenericMetricsClass>(name, typeNode.FirstChild!);
                metrics.Add(typeData);
            }
        }
        return metrics.ToArray();
    }
}
