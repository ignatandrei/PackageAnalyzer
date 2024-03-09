
# Projects relations (not included tests)

## Quadrant Packages / Relations

```mermaid

quadrantChart
    title Number of Packages and Relations of solution
    x-axis Small number Packages --> High number Packages
    y-axis Low number Relations --> High number Relations

AnalyzeMerge: [0.11,0.01]

NetPackageAnalyzerConsole: [0.56,0.67]

NetPackageAnalyzerWork: [0.37,0.01]
    
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
<small>Generated  by https://www.nuget.org/packages/netpackageanalyzerconsole , version 8.2024.309.2334</small>

