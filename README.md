# PackageAnalyzer

Analyzer for .NET solution / projects . Latest version 8.2024.309.1109


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

https://ignatandrei.github.io/PackageAnalyzer/

# Contributors needed!

If you want more to generate, add a Razor / .cshtml file to templates folder and generate in GenerateNow
