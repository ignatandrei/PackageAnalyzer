
# Projects relations

## Root Projects

```mermaid
graph TB
%% start root projects 

Solution[Solution]


NetPackageAnalyzerConsole[NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj]
Solution-->NetPackageAnalyzerConsole



```



## NetPackageAnalyzerConsole

[Relations](Projects/NetPackageAnalyzerConsole/ProjectReferences.md)

[Packages](Projects/NetPackageAnalyzerConsole/Packages.md)


```mermaid
graph TB
%% start project reference NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj

NetPackageAnalyzerConsole[NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj]

AnalyzeMerge[AnalyzeMerge/AnalyzeMerge.csproj]
NetPackageAnalyzerConsole-->AnalyzeMerge

NetPackageAnalyzerWork[NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj]
NetPackageAnalyzerConsole-->NetPackageAnalyzerWork
```

## Building Blocks - Projects with 0 project references 




### [AnalyzeMerge Relations ](Projects/AnalyzeMerge/ProjectReferences.md)

### [AnalyzeMerge Packages](Projects/AnalyzeMerge/Packages.md)




### [NetPackageAnalyzerWork Relations ](Projects/NetPackageAnalyzerWork/ProjectReferences.md)

### [NetPackageAnalyzerWork Packages](Projects/NetPackageAnalyzerWork/Packages.md)




## All Projects Graph

```mermaid
graph TB




%% start project reference AnalyzeMerge/AnalyzeMerge.csproj



%% start project reference NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj


NetPackageAnalyzerConsole-->AnalyzeMerge


NetPackageAnalyzerConsole-->NetPackageAnalyzerWork



%% start project reference NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj


```
<small>Generated  by https://www.nuget.org/packages/netpackageanalyzerconsole , version 8.2024.315.1900</small>

