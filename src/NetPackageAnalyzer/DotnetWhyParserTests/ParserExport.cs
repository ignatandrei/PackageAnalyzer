
using DotnetWhyParserObjects;
using System.Runtime.InteropServices.Marshalling;

namespace DotnetWhyParserTests;

public class ParserExport
{
    public string SampleOutput = @"
Project 'XP.UI' has the following dependency graph(s) for 'System.Runtime.Caching':

  [net9.0/browser-wasm]
  [net9.0]
   │  
   ├─ XP.DataFromAPI (v1.0.0)
   │  └─ XP.Interfaces (v1.0.0)
   │     └─ XP.Database (v1.0.0)
   │        └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
   │           └─ Microsoft.Data.SqlClient (v5.1.6)
   │              └─ 
System.Runtime.Caching (v6.0.0)
   ├─ XP.Health (v1.0.0)
   │  └─ XP.Interfaces (v1.0.0)
   │     └─ XP.Database (v1.0.0)
   │        └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
   │           └─ Microsoft.Data.SqlClient (v5.1.6)
   │              └─ 
System.Runtime.Caching (v6.0.0)
   └─ XP.Interfaces (v1.0.0)
      └─ XP.Database (v1.0.0)
         └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
            └─ Microsoft.Data.SqlClient (v5.1.6)
               └─ 
System.Runtime.Caching (v6.0.0)

Project 'XpertContract.AppHost' does not have a dependency on 'System.Runtime.Caching'.
Project 'XpertContract.ServiceDefaults' does not have a dependency on 'System.Runtime.Caching'.
Project 'XP.Database' has the following dependency graph(s) for 'System.Runtime.Caching':

  [net9.0]
   │  
   └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
      └─ Microsoft.Data.SqlClient (v5.1.6)
         └─ 
System.Runtime.Caching (v6.0.0)

Project 'XP.Interfaces' has the following dependency graph(s) for 'System.Runtime.Caching':

  [net9.0]
   │  
   └─ XP.Database (v1.0.0)
      └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
         └─ Microsoft.Data.SqlClient (v5.1.6)
            └─ 
System.Runtime.Caching (v6.0.0)

Project 'XP.API' has the following dependency graph(s) for 'System.Runtime.Caching':

  [net9.0]
   │  
   ├─ AspNetCore.HealthChecks.SqlServer (v9.0.0)
   │  └─ Microsoft.Data.SqlClient (v5.2.2)
   │     └─ 
System.Runtime.Caching (v8.0.0)
   ├─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
   │  └─ Microsoft.Data.SqlClient (v5.2.2)
   │     └─ 
System.Runtime.Caching (v8.0.0)
   ├─ XP.Database (v1.0.0)
   │  └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
   │     └─ Microsoft.Data.SqlClient (v5.2.2)
   │        └─ 
System.Runtime.Caching (v8.0.0)
   ├─ XP.DataFromDB (v1.0.0)
   │  ├─ XP.Database (v1.0.0)
   │  │  └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
   │  │     └─ Microsoft.Data.SqlClient (v5.2.2)
   │  │        └─ 
System.Runtime.Caching (v8.0.0)
   │  └─ XP.Interfaces (v1.0.0)
   │     └─ XP.Database (v1.0.0)
   │        └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
   │           └─ Microsoft.Data.SqlClient (v5.2.2)
   │              └─ 
System.Runtime.Caching (v8.0.0)
   ├─ XP.DynamicReports (v1.0.0)
   │  ├─ XP.Database (v1.0.0)
   │  │  └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
   │  │     └─ Microsoft.Data.SqlClient (v5.2.2)
   │  │        └─ 
System.Runtime.Caching (v8.0.0)
   │  └─ XP.Interfaces (v1.0.0)
   │     └─ XP.Database (v1.0.0)
   │        └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
   │           └─ Microsoft.Data.SqlClient (v5.2.2)
   │              └─ 
System.Runtime.Caching (v8.0.0)
   └─ XP.Interfaces (v1.0.0)
      └─ XP.Database (v1.0.0)
         └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
            └─ Microsoft.Data.SqlClient (v5.2.2)
               └─ 
System.Runtime.Caching (v8.0.0)

Project 'XP.DataFromDB' has the following dependency graph(s) for 'System.Runtime.Caching':

  [net9.0]
   │  
   ├─ XP.Database (v1.0.0)
   │  └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
   │     └─ Microsoft.Data.SqlClient (v5.1.6)
   │        └─ 
System.Runtime.Caching (v6.0.0)
   └─ XP.Interfaces (v1.0.0)
      └─ XP.Database (v1.0.0)
         └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
            └─ Microsoft.Data.SqlClient (v5.1.6)
               └─ 
System.Runtime.Caching (v6.0.0)

Project 'XP.DataFromAPI' has the following dependency graph(s) for 'System.Runtime.Caching':

  [net9.0]
   │  
   └─ XP.Interfaces (v1.0.0)
      └─ XP.Database (v1.0.0)
         └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
            └─ Microsoft.Data.SqlClient (v5.1.6)
               └─ 
System.Runtime.Caching (v6.0.0)

Project 'XP.DynamicReports' has the following dependency graph(s) for 'System.Runtime.Caching':

  [net9.0]
   │  
   ├─ XP.Database (v1.0.0)
   │  └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
   │     └─ Microsoft.Data.SqlClient (v5.1.6)
   │        └─ 
System.Runtime.Caching (v6.0.0)
   └─ XP.Interfaces (v1.0.0)
      └─ XP.Database (v1.0.0)
         └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
            └─ Microsoft.Data.SqlClient (v5.1.6)
               └─ 
System.Runtime.Caching (v6.0.0)

Project 'XP.Health' has the following dependency graph(s) for 'System.Runtime.Caching':

  [net9.0]
   │  
   └─ XP.Interfaces (v1.0.0)
      └─ XP.Database (v1.0.0)
         └─ Microsoft.EntityFrameworkCore.SqlServer (v9.0.1)
            └─ Microsoft.Data.SqlClient (v5.1.6)
               └─ 
System.Runtime.Caching (v6.0.0)

Project 'XP.DBData' does not have a dependency on 'System.Runtime.Caching'.
Project 'XP.Tests' does not have a dependency on 'System.Runtime.Caching'.";

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
        Assert.True(HasProject);
        Assert.True(HasPackage);
    }
    [Fact]
    public void FindSmallestProjects()
    {   
        var parser = new DotnetWhyParser();
        var result = parser.Parse(SampleOutput);
        var projDirect = result.SmallestProjectPathsToPackage();
        Assert.Equal(2,projDirect.Length);
        Assert.Equal("XP.Database", projDirect[0].MyName);
        var node = projDirect[0].Dependency;
        Assert.NotNull(node);
        Assert.Equal("Microsoft.EntityFrameworkCore.SqlServer,9.0.1", node.MyName); 
        node = node.Dependency;
        Assert.NotNull(node);
        Assert.Equal("Microsoft.Data.SqlClient,5.1.6", node.MyName);
        node = node.Dependency;
        Assert.NotNull(node);
        Assert.Equal("System.Runtime.Caching,6.0.0", node.MyName);
        Assert.Null(node.Dependency);
        Assert.Equal("XP.API", projDirect[1].MyName);
        node = projDirect[1].Dependency;
        Assert.NotNull(node);
        Assert.Equal("AspNetCore.HealthChecks.SqlServer,9.0.0", node.MyName);
        node = node.Dependency;
        Assert.NotNull(node);
        Assert.Equal("Microsoft.Data.SqlClient,5.2.2", node.MyName);
        node = node.Dependency;
        Assert.NotNull(node);
        Assert.Equal("System.Runtime.Caching,8.0.0", node.MyName);
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
XP.Database=>Microsoft.EntityFrameworkCore.SqlServer,9.0.1=>Microsoft.Data.SqlClient,5.1.6=>System.Runtime.Caching,6.0.0
XP.API=>AspNetCore.HealthChecks.SqlServer,9.0.0=>Microsoft.Data.SqlClient,5.2.2=>System.Runtime.Caching,8.0.0

""";
    [Fact]    
    public void ExportDataMermaid()
    {

        var parser = new DotnetWhyParser();
        var result = parser.Parse(SampleOutput);
        DotnetWhyExporter exporter = new ();
        var strMermaid = exporter.ExportToMermaidSmallestProjects(result);
        strMermaid = strMermaid.Replace("\r", "").Replace("\n", "");
        var res= mermaidSmallest.Replace("\r", "").Replace("\n", "");
        //File.WriteAllText(@"D:\eu\GitHub\PackageAnalyzer\src\documentation1\exported_mermaid.json", strMermaid);
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
    AA[["XP.Database"]]:::proj
AB[["XP.API"]]:::proj
AC(["Microsoft.EntityFrameworkCore.SqlServer,9.0.1"]):::package
AD(["Microsoft.Data.SqlClient,5.1.6"]):::package
AE(["System.Runtime.Caching,6.0.0"]):::finalPack1
AF(["AspNetCore.HealthChecks.SqlServer,9.0.0"]):::package
AG(["Microsoft.Data.SqlClient,5.2.2"]):::package
AH(["System.Runtime.Caching,8.0.0"]):::finalPack2
AI[["System.Runtime.Caching"]]:::proj
    AE --> AI
AH --> AI

    subgraph "System.Runtime.Caching,6.0.0"
    direction TB  
    AA --> AC
AC --> AD
AD --> AE

end
subgraph "System.Runtime.Caching,8.0.0"
    direction TB  
    AB --> AF
AF --> AG
AG --> AH

end
""";

}
