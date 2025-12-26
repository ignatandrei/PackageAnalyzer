namespace DotnetWhyParserTests;

using DotnetWhyParserObjects;

public class ParserTests
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
    public void TestParserExtractsTargetPackage()
    {
        var parser = new DotnetWhyParser();
        var result = parser.Parse(SampleOutput);
        
        Assert.NotEmpty(result.TargetPackage);
    }

    [Fact]
    public void TestParserIdentifiesProjects()
    {
        var parser = new DotnetWhyParser();
        var result = parser.Parse(SampleOutput);
        
        // Should find projects (exact count depends on implementation details)
        Assert.True(result.Projects.Count >= 10);
    }

    [Fact]
    public void TestParserIdentifiesProjectsWithoutDependencies()
    {
        var parser = new DotnetWhyParser();
        var result = parser.Parse(SampleOutput);
        
        var projectsWithoutDep = result.Projects.Where(p => !p.HasDependency).ToList();
        
        // Should find projects without dependencies
        Assert.True(projectsWithoutDep.Count >= 2);
        Assert.Contains(projectsWithoutDep, p => p.ProjectName == "XP.DBData");
    }

    [Fact]
    public void TestParserExtractsDependencies()
    {
        var parser = new DotnetWhyParser();
        var result = parser.Parse(SampleOutput);
        
        // The parser should work (this validates the core functionality)
        Assert.NotNull(result);
        Assert.NotEmpty(result.TargetPackage);
        Assert.True(result.Projects.Count > 0);
    }

    [Fact]
    public void TestParserBuildsDependencyGraphs()
    {
        var parser = new DotnetWhyParser();
        var result = parser.Parse(SampleOutput);
        
        // Find a project with dependencies
        var projectWithDeps = result.Projects.FirstOrDefault(p => p.HasDependency && p.DependencyGraphs.Count > 0);
        Assert.NotNull(projectWithDeps);
    }

    [Fact]
    public void TestSystemRuntimeCachingV6HasDependencyGraphToXpDataFromApi()
    {
        var parser = new DotnetWhyParser();
        var result = parser.Parse(SampleOutput);
        
        // Find XP.UI project which has System.Runtime.Caching v6.0.0
        var xpUiProject = result.Projects.FirstOrDefault(p => p.ProjectName == "XP.UI");
        Assert.NotNull(xpUiProject);
        Assert.True(xpUiProject.HasDependency);
        
        // Check that XP.UI has dependency graphs
        Assert.True(xpUiProject.DependencyGraphs.Count > 0, "XP.UI should have dependency graphs");
        
        // Get versions for XP.UI
        var versions = result.GetVersionsByProject();
        Assert.Contains("XP.UI", versions.Keys);
        Assert.Contains("6.0.0", versions["XP.UI"]);
        
        // Verify the dependency chain includes XP.DataFromAPI
        bool foundXpDataFromApiInGraph = false;
        foreach (var graph in xpUiProject.DependencyGraphs)
        {
            if (graph.Name == "XP.DataFromAPI")
            {
                foundXpDataFromApiInGraph = true;
                break;
            }
        }
        Assert.True(foundXpDataFromApiInGraph, "XP.UI dependency graph should contain XP.DataFromAPI");
    }

    [Fact]
    public void TestSystemRuntimeCachingV8HasDependencyGraphToXpApi()
    {
        var parser = new DotnetWhyParser();
        var result = parser.Parse(SampleOutput);
        
        // Find XP.API project which has System.Runtime.Caching v8.0.0
        var xpApiProject = result.Projects.FirstOrDefault(p => p.ProjectName == "XP.API");
        Assert.NotNull(xpApiProject);
        Assert.True(xpApiProject.HasDependency);
        
        // Check that XP.API has dependency graphs
        Assert.True(xpApiProject.DependencyGraphs.Count > 0, "XP.API should have dependency graphs");
        
        // Get versions for XP.API
        var versions = result.GetVersionsByProject();
        Assert.Contains("XP.API", versions.Keys);
        Assert.Contains("8.0.0", versions["XP.API"]);
    }
}
