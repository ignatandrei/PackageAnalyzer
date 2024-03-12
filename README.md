# PackageAnalyzer

Analyzer for .NET solution / projects . Latest version 8.2024.311.2139


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

1. Solution Analyzer 
2. Project Building Blocks
3. Root Projects
4. Test Projects
5. Packages Versions
6. Packages that differ in major versions
7. Each project with their packages 
8. Each project and relations with another - upstream and downstream


## How it looks

This are the files generated :

https://ignatandrei.github.io/PackageAnalyzer/

# Contributors needed!

If you want more to generate, add a Razor / .cshtml file to templates folder and generate in GenerateNow
