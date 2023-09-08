# PackageAnalyzer

Analyzer for .NET solution / projects . Latest version 7.2023.909.140


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

and see the template results at Analysis folder


## How it looks

This are the files generated :

1. 
https://ignatandrei.github.io/PackageAnalyzer/DisplayAllVersions.html

List of nuget  package versions - can be filtered by differences in version

2.  https://github.com/ignatandrei/PackageAnalyzer/blob/main/docs/DisplayAllVersions.md

3. https://github.com/ignatandrei/PackageAnalyzer/blob/main/docs/MermaidVisualizerMajorDiffer.md

A mermaid enabled display for projects where major versions differ 

3.  https://github.com/ignatandrei/PackageAnalyzer/blob/main/docs/ProjectRelation.md

Relations between projects

# Contributors needed!

If you want more to generate, add a Razor / .cshtml file to templates folder and generate in GenerateNow
