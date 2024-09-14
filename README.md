[![Latest version](https://img.shields.io/nuget/v/netpackageanalyzerconsole.svg)](https://www.nuget.org/packages/netpackageanalyzerconsole)

# PackageAnalyzer

Analyzer for .NET solution / projects .  It shows relations in projects / packages / commits .

Please read the Wiki at https://github.com/ignatandrei/packageAnalyzer/wiki/ 

## Install as local tool

Go to where your sln is and enter this:

```
dotnet new tool-manifest
dotnet tool update netpackageanalyzerconsole
```

If you want a fast summary of the solution, just run

```
dotnet PackageAnalyzer generateFiles -wg HtmlSummary
```

And will generate a html file with all the information.  As an example , see https://ignatandrei.github.io/PackageAnalyzer/NetPackageAnalyzer_summary.html



If you want a site of all solution, run

```
dotnet PackageAnalyzer generateFiles -wg Docusaurus
```

and see results at Analysis folder as a Docusaurus site . You should run

```
npm i
npm run start
```

to see what is generated ( see https://ignatandrei.github.io/PackageAnalyzer/docs/category/solutions )


It will show

10. Solution Analyzer - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/ProjectRelation
15. Project references with another projects - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/summaryProjectReferences
20. Project Building Blocks - https://ignatandrei0.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/BuildingBlocks
30. Root Projects - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/RootProjects
40. Test Projects - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/TestProjects
50. Packages Versions - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/DisplayAllVersions
60. Packages that differ in major versions  -  https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/MermaidVisualizerMajorDiffer 
70. Each project with their packages - https://ignatandrei.github.io/PackageAnalyzer/docs/category/projects
80. Each project and relations with another - upstream and downstream - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerDocusaurus/ProjectReferences
85. Classes relations in a project: https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerDocusaurus/NetPackageAnalyzerDocusaurus_rel_csproj
90. Each project with their packages - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerDocusaurus/Packages
110. Commits (full time and per year ) and median - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/Commits
120. Commits per project and median - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerConsole/Commits
130. Commits per file and median - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerConsole/Commits
140. Commits with most files -  https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerConsole/Commits
150. Classes / Projects with most public methods - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/summaryPublicClasses
160. Number of lines per class / project / method - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/summaryPublicClasses
170. Radar summary - https://ignatandrei.github.io/PackageAnalyzer/NetPackageAnalyzer_summary.html#radar
180. Commits per year and folder https://ignatandrei.github.io/PackageAnalyzer/NetPackageAnalyzer_summary.html#Commitsperyearandfolder


And a summary https://ignatandrei.github.io/PackageAnalyzer/NetPackageAnalyzer_summary.html


Please read the Wiki at https://github.com/ignatandrei/packageAnalyzer/wiki/ 


## How it looks

Those are the files generated for summary:
https://ignatandrei.github.io/PackageAnalyzer/NetPackageAnalyzer_summary.html

Or for site:
https://ignatandrei.github.io/PackageAnalyzer/



# Contributors needed!

If you want more to generate, add a Razor / .cshtml file to templates folder and generate in GenerateNow

# Errors

If you have errors, please run with 

```
dotnet PackageAnalyzer generateFiles --verbose true 
```

and open an issue with the verbose file mentioned in the output at 

```
Please see verbose file at
```
