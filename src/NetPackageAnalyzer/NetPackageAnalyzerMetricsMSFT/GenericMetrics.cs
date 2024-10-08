using System.ComponentModel.DataAnnotations;
using System.Xml;
//https://learn.microsoft.com/en-us/visualstudio/code-quality/code-metrics-cyclomatic-complexity?view=vs-2022

namespace NetPackageAnalyzerMetricsMSFT;
public enum eMSFTMetrics
{
    None = 0,
    MaintainabilityIndex,
    CyclomaticComplexity,
    ClassCoupling,
    DepthOfInheritance,
    SourceLines,
    ExecutableLines

}
public class GenericMetrics: IValidatableObject
{
    public string Name="";
    public Dictionary<eMSFTMetrics ,Metric> metrics;
    public GenericMetrics[] Childs=[]; 
    public GenericMetrics()
    {
        metrics = new ()
        {
            { eMSFTMetrics.MaintainabilityIndex, new  (eMSFTMetrics.MaintainabilityIndex)},
            {eMSFTMetrics.CyclomaticComplexity, new ( eMSFTMetrics.CyclomaticComplexity) },
            {eMSFTMetrics.ClassCoupling, new ( eMSFTMetrics.ClassCoupling)},
            {eMSFTMetrics.DepthOfInheritance ,new ( eMSFTMetrics.DepthOfInheritance) },
            {eMSFTMetrics.SourceLines ,new ( eMSFTMetrics.SourceLines) },
            { eMSFTMetrics.ExecutableLines,new ( eMSFTMetrics.ExecutableLines) },
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
            eMSFTMetrics val= Enum.Parse<eMSFTMetrics>( nameMetric);
            genericMetrics.metrics[val].Value=int.Parse(valueMetric);
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
        List<GenericMetrics> metricsAssembly = new();
        foreach (var target in nodes)
        {

            var node = target as XmlNode;
            if (node == null) continue;
            string name = node.Attributes!["Name"]!.Value;
            var assemblyNode=node.SelectSingleNode("Assembly");
            if (assemblyNode == null) continue;
            var data = CreateFromXML<GenericMetricsAssembly>(name, assemblyNode.FirstChild!);

            var types = node.SelectNodes("//NamedType");
            if (types == null) continue;
            List<GenericMetricsClass> typesClass = new();
            foreach (var type in types)
            {
                var typeNode = type as XmlNode;
                if (typeNode == null) continue;
                if(typeNode.FirstChild == null) continue;
                name = typeNode.Attributes!["Name"]!.Value;
                var typeData = CreateFromXML<GenericMetricsClass>(name, typeNode.FirstChild!);
                //add methods
                var methods = typeNode.FirstChild.SelectNodes("//Method");
                if (methods == null) continue;
                List<GenericMetricsMethod> methodsMetrics = new();
                foreach (var method in methods)
                {
                    var methodNode = method as XmlNode;
                    if (methodNode == null) continue;
                    var parent = methodNode.ParentNode;
                    if(parent == null) continue;
                    parent = parent.ParentNode;
                    if(parent == null) continue;
                    if(parent != typeNode)
                        continue;
                    name = methodNode.Attributes!["Name"]!.Value;
                    var methodData = CreateFromXML<GenericMetricsMethod>(name, methodNode.FirstChild!);
                    if (!methodData.IsGetSetWith100())
                        methodsMetrics.Add(methodData);
                }
                typeData.Childs = methodsMetrics.ToArray();
                typesClass.Add(typeData);

            }
            data.Childs = typesClass.ToArray();
            metricsAssembly.Add(data);
        }
        return metricsAssembly.Where(it=>
        {
            var vc = new ValidationContext(it);
            return !it.Validate(vc).Any();
        })
            .ToArray();
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var invalidMetrics= new List<ValidationResult>();

        var metricsInvalid = metrics.Count(it => !it.Value.IsInitialized);
        if (metricsInvalid > 1)//could be ClassCoupling that is not applied
        {
             yield return new("more than 1 metric invalid");
        }        
    }
}
