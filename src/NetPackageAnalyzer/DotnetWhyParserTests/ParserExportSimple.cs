
using DotnetWhyParserObjects;
using System.Runtime.InteropServices.Marshalling;

namespace DotnetWhyParserTests;

public class ParserExportSimple
{
    public string SampleOutput = @"
Project 'SkinnyControllersCommon' does not have a dependency on 'Microsoft.AspNetCore.Authentication.JwtBearer'.
Project 'SkinnyControllerTest' has the following dependency graph(s) for 'Microsoft.AspNetCore.Authentication.JwtBearer':

  [net10.0]
   │
   └─
Microsoft.AspNetCore.Authentication.JwtBearer (v5.0.0)

Project 'SkinnyControllersGenerator' does not have a dependency on 'Microsoft.AspNetCore.Authentication.JwtBearer'.

";

    [Fact]
    public void ConsolidateData()
    {
        var parser = new DotnetWhyParser();
        var result = parser.Parse(SampleOutput);        
        bool HasProject=false;
        bool HasPackage=false;
        foreach(var item in result.AllChildren())
        {
            switch (item.typeDependencyNode)
            {
                case TypeDependencyNode.Package:
                    HasPackage = true; 
                    break;
                case TypeDependencyNode.Project:
                    HasProject = true;
                    break;
                case TypeDependencyNode.None:
                    Assert.NotEqual(TypeDependencyNode.None, item.typeDependencyNode);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        //it is direct
        Assert.False(HasProject,"should not have projects");
     
        Assert.True(HasPackage,"should have the package");

    }
    [Fact]
    public void FindSmallestProjects()
    {   
        var parser = new DotnetWhyParser();
        var result = parser.Parse(SampleOutput);
        var projDirect = result.SmallestProjectPathsToPackage();
        Assert.Single(projDirect);
        Assert.Equal("SkinnyControllerTest", projDirect[0].MyName);
        var node = projDirect[0].Dependency;
        Assert.NotNull(node);
        Assert.Equal("Microsoft.AspNetCore.Authentication.JwtBearer,5.0.0", node.MyName); 
        Assert.Null(node.Dependency);
    }
    [Fact]
    public void ExportLines()
    {
        var parser = new DotnetWhyParser();
        var result = parser.Parse(SampleOutput);
        DotnetWhyExporter exporter = new();
        var strLines = exporter.ExportToLines(result,"=>",Environment.NewLine);
        //File.WriteAllText(@"D:\eu\GitHub\PackageAnalyzer\src\documentation1\exported_mermaid.json", strLines);
        strLines = strLines.Replace("\r", "").Replace("\n", "");
        var res = lineExport.Replace("\r", "").Replace("\n", "");

        Assert.Equal(res, strLines, ignoreCase: true, ignoreAllWhiteSpace: true, ignoreLineEndingDifferences: true);

    }
    string lineExport = """
SkinnyControllerTest=>Microsoft.AspNetCore.Authentication.JwtBearer,5.0.0
""";
    [Fact]    
    public void ExportDataMermaid()
    {

        var parser = new DotnetWhyParser();
        var result = parser.Parse(SampleOutput);
        DotnetWhyExporter exporter = new ();
        var strMermaid = exporter.ExportToMermaidSmallestProjects(result);
        File.WriteAllText(@"D:\eu\GitHub\PackageAnalyzer\src\documentation1\exported_mermaid.json", strMermaid);
        strMermaid = strMermaid.Replace("\r", "").Replace("\n", "");
        var res= mermaidSmallest.Replace("\r", "").Replace("\n", "");
        Assert.Equal(res,strMermaid,ignoreCase:true,ignoreAllWhiteSpace:true,ignoreLineEndingDifferences:true);
    }
    string mermaidSmallest = """
flowchart 
    direction TB
    classDef proj stroke:#0f0
    classDef package stroke:#0f0

    classDef finalPack1 fill:#f00
    classDef finalPack2 fill:#0f0
    classDef finalPack3 fill:#00f

    classDef proj stroke:#00f
    classDef package stroke:#0f0
    AA[["SkinnyControllerTest"]]:::proj
AB(["Microsoft.AspNetCore.Authentication.JwtBearer,5.0.0"]):::finalPack1
AC[["Microsoft.AspNetCore.Authentication.JwtBearer"]]:::proj
    AB --> AC

    subgraph "Microsoft.AspNetCore.Authentication.JwtBearer,5.0.0"
    direction TB  
    AA --> AB

end
""";

}
