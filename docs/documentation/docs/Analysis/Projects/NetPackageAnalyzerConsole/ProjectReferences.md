
# Project relations for NetPackageAnalyzerConsole

```mermaid
graph TB    

NetPackageAnalyzerConsole[NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj]""
click NetPackageAnalyzerConsole "../../Projects/NetPackageAnalyzerConsole/ProjectReferences" "NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj"


AnalyzeMerge[AnalyzeMerge/AnalyzeMerge.csproj]

NetPackageAnalyzerConsole-->AnalyzeMerge

click AnalyzeMerge "../../Projects/AnalyzeMerge/ProjectReferences" "AnalyzeMerge/AnalyzeMerge.csproj"



NetPackageAnalyzerWork[NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj]

NetPackageAnalyzerConsole-->NetPackageAnalyzerWork

click NetPackageAnalyzerWork "../../Projects/NetPackageAnalyzerWork/ProjectReferences" "NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj"



```


# Projects that reference NetPackageAnalyzerConsole
```mermaid
graph TB

NetPackageAnalyzerConsole[NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj]
click NetPackageAnalyzerConsole "../../Projects/NetPackageAnalyzerConsole/ProjectReferences" "NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj"


```


# Full Project relations for NetPackageAnalyzerConsole

```mermaid
graph TB

NetPackageAnalyzerConsole[NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj]
click NetPackageAnalyzerConsole "../../Projects/NetPackageAnalyzerConsole/ProjectReferences" "NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj"


AnalyzeMerge[AnalyzeMerge/AnalyzeMerge.csproj]

click AnalyzeMerge "../../Projects/AnalyzeMerge/ProjectReferences" "AnalyzeMerge/AnalyzeMerge.csproj"


NetPackageAnalyzerConsole-->AnalyzeMerge


NetPackageAnalyzerWork[NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj]

click NetPackageAnalyzerWork "../../Projects/NetPackageAnalyzerWork/ProjectReferences" "NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj"


NetPackageAnalyzerConsole-->NetPackageAnalyzerWork


```


[Packages](Packages.md)


[Back To Solution](../../ProjectRelation.md)

<small>Generated  by https://www.nuget.org/packages/netpackageanalyzerconsole , version 8.2024.308.841</small>

