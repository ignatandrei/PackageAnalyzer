
## Root Projects - projects that are not referenced anywhere

```mermaid
graph TB
%% start root projects

Solution[Solution]


        NetPackageAnalyzerConsole[NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj]

        %% find a way to interpret first the path
        %% click NetPackageAnalyzerConsole "pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerConsole/ProjectReferences" "NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj"

        Solution-->NetPackageAnalyzerConsole


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

            
                NetPackageAnalyzerDocusaurus[NetPackageAnalyzerWork/NetPackageAnalyzerDocusaurus.csproj]

                %% find a way to interpret first the path
                %% click NetPackageAnalyzerDocusaurus "pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerDocusaurus/ProjectReferences" "NetPackageAnalyzerWork/NetPackageAnalyzerDocusaurus.csproj"

                NetPackageAnalyzerConsole-->NetPackageAnalyzerDocusaurus

                    ```
    


<small>Generated  by https://www.nuget.org/packages/NetPackageAnalyzerDocusaurus , version 8.2024.310.1936</small>