# PackageAnalyzer

Analyzer for .NET solution / projects .  It shows relations in projects / packages / commits .

Please read the Wiki at https://github.com/ignatandrei/packageAnalyzer/wiki/ 

## Install as local tool

Go to where your sln is and enter this:
```
dotnet new tool-manifest
dotnet tool update netpackageanalyzerconsole
```

Then you can run

```
dotnet PackageAnalyzer generateFiles
```

and see results at Analysis folder as a Docusaurus site .

Just run

```
npm i
npm run start
```

to see what is generated ( see https://ignatandrei.github.io/PackageAnalyzer/docs/category/solutions )


It will show

1. Solution Analyzer - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/ProjectRelation
2. Project Building Blocks - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/BuildingBlocks
3. Root Projects - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/RootProjects
4. Test Projects - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/TestProjects
5. Packages Versions - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/DisplayAllVersions
6. Packages that differ in major versions  -  https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/MermaidVisualizerMajorDiffer 
7. Each project with their packages - https://ignatandrei.github.io/PackageAnalyzer/docs/category/projects
8. Each project and relations with another - upstream and downstream - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerDocusaurus/ProjectReferences
9. Each project with their packages - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerDocusaurus/Packages
10. Commits (full time and per year ) - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/Commits
11. Commits per project - https://ignatandrei.github.io/PackageAnalyzer/docs/Analysis/NetPackageAnalyzer/Projects/NetPackageAnalyzerConsole/Commits


Please read the Wiki at https://github.com/ignatandrei/packageAnalyzer/wiki/ 


## How it looks

Those are the files generated :

https://ignatandrei.github.io/PackageAnalyzer/


# Contributors needed!

If you want more to generate, add a Razor / .cshtml file to templates folder and generate in GenerateNow

# Errors

If you have errors, please run with 

```
dotnet PackageAnalyzer generateFiles --verbose true 
```

and open an issue with the verbose file mentioned in the output at 

Please see verbose file at

