
# Project relations for NetPackageAnalyzerConsole

```mermaid
graph TB    

NetPackageAnalyzerConsole[NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj]""
click NetPackageAnalyzerConsole "../../NetPackageAnalyzerConsole/ProjectReferences" "NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj"


AnalyzeMerge[AnalyzeMerge/AnalyzeMerge.csproj]

NetPackageAnalyzerConsole-->AnalyzeMerge

click AnalyzeMerge "../../AnalyzeMerge/ProjectReferences" "AnalyzeMerge/AnalyzeMerge.csproj"



NetPackageAnalyzerWork[NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj]

NetPackageAnalyzerConsole-->NetPackageAnalyzerWork

click NetPackageAnalyzerWork "../../NetPackageAnalyzerWork/ProjectReferences" "NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj"



```


# Projects that reference NetPackageAnalyzerConsole
```mermaid
graph TB

NetPackageAnalyzerConsole[NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj]
click NetPackageAnalyzerConsole "../../NetPackageAnalyzerConsole/ProjectReferences" "NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj"


```


# Full Project relations for NetPackageAnalyzerConsole

```mermaid
graph TB

NetPackageAnalyzerConsole[NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj]
click NetPackageAnalyzerConsole "../../NetPackageAnalyzerConsole/ProjectReferences" "NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj"


AnalyzeMerge[AnalyzeMerge/AnalyzeMerge.csproj]

click AnalyzeMerge "../../AnalyzeMerge/ProjectReferences" "AnalyzeMerge/AnalyzeMerge.csproj"


NetPackageAnalyzerConsole-->AnalyzeMerge


NetPackageAnalyzerWork[NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj]

click NetPackageAnalyzerWork "../../NetPackageAnalyzerWork/ProjectReferences" "NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj"


NetPackageAnalyzerConsole-->NetPackageAnalyzerWork


```


[Packages](Packages.md)


[Back To Solution](../../ProjectRelation.md)

<small>Generated  by https://www.nuget.org/packages/netpackageanalyzerconsole , version 8.2024.308.2104</small>

