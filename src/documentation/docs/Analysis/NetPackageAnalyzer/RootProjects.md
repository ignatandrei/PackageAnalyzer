
## Root Projects - projects that are not referenced anywhere

```mermaid
graph TB
%% start root projects

Solution[Solution]


        NetPackageAnalyzerConsole[NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj]

        %% find a way to interpret first the path
        %% click NetPackageAnalyzerConsole "pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerConsole/ProjectReferences" "NetPackageAnalyzerConsole/NetPackageAnalyzerConsole.csproj"

        Solution-->NetPackageAnalyzerConsole


    
        NetPackageAnalyzerDocusaurus[NetPackageAnalyzerDocusaurus/NetPackageAnalyzerDocusaurus.csproj]

        %% find a way to interpret first the path
        %% click NetPackageAnalyzerDocusaurus "pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerDocusaurus/ProjectReferences" "NetPackageAnalyzerDocusaurus/NetPackageAnalyzerDocusaurus.csproj"

        Solution-->NetPackageAnalyzerDocusaurus


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
    

        ### NetPackageAnalyzerDocusaurus

        [Relations](pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerDocusaurus/ProjectReferences)

        [Packages](pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerDocusaurus/Packages)


        ```mermaid
        graph TB
        %% start project reference NetPackageAnalyzerDocusaurus/NetPackageAnalyzerDocusaurus.csproj

        NetPackageAnalyzerDocusaurus[NetPackageAnalyzerDocusaurus/NetPackageAnalyzerDocusaurus.csproj]

        %% find a way to interpret first the path
        %% click NetPackageAnalyzerDocusaurus "pathname:///docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerDocusaurus/ProjectReferences" "NetPackageAnalyzerDocusaurus/NetPackageAnalyzerDocusaurus.csproj"

        ```
    


<small>Generated  by https://www.nuget.org/packages/netpackageanalyzerconsole , version 1.0.0.0</small>