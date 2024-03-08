
# Projects relations

## Building Blocks - Projects with 0 project references




        ### AnalyzeMerge

        Full Name : AnalyzeMerge/AnalyzeMerge.csproj

        [AnalyzeMerge Relations ](Projects/AnalyzeMerge/ProjectReferences.md)

        [AnalyzeMerge Packages](Projects/AnalyzeMerge/Packages.md)

    


        ### NetPackageAnalyzerWork

        Full Name : NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj

        [NetPackageAnalyzerWork Relations ](Projects/NetPackageAnalyzerWork/ProjectReferences.md)

        [NetPackageAnalyzerWork Packages](Projects/NetPackageAnalyzerWork/Packages.md)

    


## Root Projects - projects that are not referenced anywhere

```mermaid
graph TB
%% start root projects 

Solution[Solution]


NetPackageAnalyzerConsole[NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj]

click NetPackageAnalyzerConsole "./Projects/NetPackageAnalyzerConsole/ProjectReferences" "NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj"

Solution-->NetPackageAnalyzerConsole







```



## NetPackageAnalyzerConsole

[Relations](Projects/NetPackageAnalyzerConsole/ProjectReferences.md)

[Packages](Projects/NetPackageAnalyzerConsole/Packages.md)


```mermaid
graph TB
%% start project reference NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj

NetPackageAnalyzerConsole[NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj]
click NetPackageAnalyzerConsole "./Projects/NetPackageAnalyzerConsole/ProjectReferences" "NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj"


AnalyzeMerge[AnalyzeMerge/AnalyzeMerge.csproj]
click AnalyzeMerge "./Projects/AnalyzeMerge/ProjectReferences" "AnalyzeMerge/AnalyzeMerge.csproj"

NetPackageAnalyzerConsole-->AnalyzeMerge


NetPackageAnalyzerWork[NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj]
click NetPackageAnalyzerWork "./Projects/NetPackageAnalyzerWork/ProjectReferences" "NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj"

NetPackageAnalyzerConsole-->NetPackageAnalyzerWork

```


## All Projects Graph

```mermaid
graph TB




%% start project reference AnalyzeMerge/AnalyzeMerge.csproj



%% start project reference NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj


NetPackageAnalyzerConsole-->AnalyzeMerge


NetPackageAnalyzerConsole-->NetPackageAnalyzerWork



%% start project reference NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj


```
<small>Generated  by https://www.nuget.org/packages/netpackageanalyzerconsole , version 8.2024.308.841</small>

