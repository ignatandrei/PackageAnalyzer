
# Projects relations (not included tests)

## Quadrant Packages / Relations

```mermaid

quadrantChart
    title Number of Packages and Relations of solution
    x-axis Small number Packages --> High number Packages
    y-axis Low number Relations --> High number Relations

AnalyzeMerge: [0.11,0.01]

NetPackageAnalyzerConsole: [0.56,0.67]

NetPackageAnalyzerDocusaurus: [0.01,0.01]

NetPackageAnalyzerObjects: [0.11,0.01]

NetPackageAnalyzerWork: [0.37,0.33]
    
```

## All Projects Graph

```mermaid
graph TB




%% start project reference AnalyzeMerge/AnalyzeMerge.csproj



%% start project reference NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj


NetPackageAnalyzerConsole-->AnalyzeMerge


NetPackageAnalyzerConsole-->NetPackageAnalyzerWork



%% start project reference NetPackageAnalyzerDocusaurus/NetPackageAnalyzerDocusaurus.csproj



%% start project reference NetPackageAnalyzerObjects/NetPackageAnalyzerObjects.csproj



%% start project reference NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj


NetPackageAnalyzerWork-->NetPackageAnalyzerObjects


```
<small>Generated  by https://www.nuget.org/packages/netpackageanalyzerconsole , version 1.0.0.0</small>

