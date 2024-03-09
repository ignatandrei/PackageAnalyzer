
# Projects relations

## Data

```mermaid

quadrantChart
    title Number of Packages and Relations of solution
    x-axis Small number Package --> High number Package
    y-axis Low number Relations --> High number Relations
    quadrant-1 Difficult
    quadrant-2 Business experience
    quadrant-3 Easy
    quadrant-4 Nuget experience


AnalyzeMerge: [0.11,0.00]

NetPackageAnalyzerConsole: [0.56,0.67]

NetPackageAnalyzerTests: [0.96,0.33]

NetPackageAnalyzerWork: [0.37,0.00]
    
```




## Building Blocks - Projects with 0 project references




### AnalyzeMerge

Full Name : AnalyzeMerge/AnalyzeMerge.csproj

[AnalyzeMerge Relations ](pathname:///docs/Analysis/NetPackageAnalyzer/Projects/AnalyzeMerge/ProjectReferences)

[AnalyzeMerge Packages](pathname:///docs/Analysis/NetPackageAnalyzer/Projects/AnalyzeMerge/Packages)

    


### NetPackageAnalyzerWork

Full Name : NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj

[NetPackageAnalyzerWork Relations ](pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerWork/ProjectReferences)

[NetPackageAnalyzerWork Packages](pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerWork/Packages)

    


## Root Projects - projects that are not referenced anywhere

```mermaid
graph TB
%% start root projects 

Solution[Solution]


NetPackageAnalyzerConsole[NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj]

%% find a way to interpret first the path 
%% click NetPackageAnalyzerConsole "pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerConsole/ProjectReferences" "NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj"

Solution-->NetPackageAnalyzerConsole



NetPackageAnalyzerTests[NetPackageAnalyzerTests/NetPackageAnalyzerTests.csproj]

%% find a way to interpret first the path 
%% click NetPackageAnalyzerTests "pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerTests/ProjectReferences" "NetPackageAnalyzerTests/NetPackageAnalyzerTests.csproj"

Solution-->NetPackageAnalyzerTests


```



### NetPackageAnalyzerConsole

[Relations](pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerConsole/ProjectReferences)

[Packages](pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerConsole/Packages)


```mermaid
graph TB
%% start project reference NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj

NetPackageAnalyzerConsole[NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj]

%% find a way to interpret first the path
%% click NetPackageAnalyzerConsole "pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerConsole/ProjectReferences" "NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj"


AnalyzeMerge[AnalyzeMerge/AnalyzeMerge.csproj]

%% find a way to interpret first the path
%% click AnalyzeMerge "pathname:///docs/Analysis/NetPackageAnalyzer/Projects/AnalyzeMerge/ProjectReferences" "AnalyzeMerge/AnalyzeMerge.csproj"

NetPackageAnalyzerConsole-->AnalyzeMerge


NetPackageAnalyzerWork[NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj]

%% find a way to interpret first the path
%% click NetPackageAnalyzerWork "pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerWork/ProjectReferences" "NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj"

NetPackageAnalyzerConsole-->NetPackageAnalyzerWork

```


### NetPackageAnalyzerTests

[Relations](pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerTests/ProjectReferences)

[Packages](pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerTests/Packages)


```mermaid
graph TB
%% start project reference NetPackageAnalyzerTests/NetPackageAnalyzerTests.csproj

NetPackageAnalyzerTests[NetPackageAnalyzerTests/NetPackageAnalyzerTests.csproj]

%% find a way to interpret first the path
%% click NetPackageAnalyzerTests "pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerTests/ProjectReferences" "NetPackageAnalyzerTests/NetPackageAnalyzerTests.csproj"


NetPackageAnalyzerWork[NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj]

%% find a way to interpret first the path
%% click NetPackageAnalyzerWork "pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerWork/ProjectReferences" "NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj"

NetPackageAnalyzerTests-->NetPackageAnalyzerWork

```


## All Projects Graph

```mermaid
graph TB




%% start project reference AnalyzeMerge/AnalyzeMerge.csproj



%% start project reference NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj


NetPackageAnalyzerConsole-->AnalyzeMerge


NetPackageAnalyzerConsole-->NetPackageAnalyzerWork



%% start project reference NetPackageAnalyzerTests/NetPackageAnalyzerTests.csproj


NetPackageAnalyzerTests-->NetPackageAnalyzerWork



%% start project reference NetPackageAnalyzerWork/NetPackageAnalyzerWork.csproj


```
<small>Generated  by https://www.nuget.org/packages/netpackageanalyzerconsole , version 8.2024.309.1834</small>

