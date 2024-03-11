
# Projects relations (not included tests)

## Quadrant Packages / Relations

```mermaid

quadrantChart
    title Number of Packages and Relations of solution
    x-axis Small number Packages --> High number Packages
    y-axis Low number Relations --> High number Relations

AnalyzeMerge: [0.11,0.01]

NetPackageAnalyzerConsole: [0.56,0.67]

NetPackageAnalyzerDocusaurus: [0.41,0.33]

NetPackageAnalyzerObjects: [0.33,0.01]
    
```

## All Projects Graph

```mermaid
graph TB




%% start project reference AnalyzeMerge/AnalyzeMerge.csproj



%% start project reference NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj


NetPackageAnalyzerConsole-->AnalyzeMerge


NetPackageAnalyzerConsole-->NetPackageAnalyzerDocusaurus



%% start project reference NetPackageAnalyzerWork/NetPackageAnalyzerDocusaurus.csproj


NetPackageAnalyzerDocusaurus-->NetPackageAnalyzerObjects



%% start project reference NetPackageAnalyzerObjects/NetPackageAnalyzerObjects.csproj


```
<small>Generated  by https://www.nuget.org/packages/NetPackageAnalyzerDocusaurus , version 8.2024.311.2139</small>

