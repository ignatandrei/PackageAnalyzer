﻿$url=  "https://github.com/fullstackhero/dotnet-starter-kit"
$url="https://github.com/ivanpaulovich/clean-architecture-manga"
#many sln
$url ="https://github.com/evolutionary-architecture/evolutionary-architecture-by-example"
$url="https://github.com/jasontaylordev/CleanArchitecture"
$url="https://github.com/jbogard/ContosoUniversityDotNetCore-Pages"
#3 sln
$url="https://github.com/dotnet/eShop"
$url="https://github.com/kgrzybek/modular-monolith-with-ddd"

$name = $url.Split('/')[-1]
$analyzerPath = Get-Location
$copyPath = Join-Path  $analyzerPath 'src' 
$copyPath = Join-Path $copyPath 'documentation1'
$copyPath = Join-Path $copyPath 'docs'
$copyPath = Join-Path $copyPath 'analysis'
$copyPath = Join-Path $copyPath $name


if(-not (Test-Path $copyPath)) {
    New-Item -ItemType Directory -Path $copyPath    
$jsonContent = @"
{
    "label": "$name",
    "position": 1,
    "link": {
        "type": "generated-index"
    }
}
"@

# Specify the file path where you want to save the JSON content
$filePath = Join-Path $copyPath '_category_.json'

# Write the JSON content to the file
Set-Content -Path $filePath -Value $jsonContent

}


Push-Location ..

if(-not (Test-Path $name)) {
    git clone $url
}

cd $name
$currentPath = Get-Location
$sln = (Get-ChildItem *.sln -r)
Write-Host "found $($sln.Count) solutions"
$index=0
$sln | ForEach-Object {
    $index++
    Write-host $_.FullName 
    $solutionPath = Split-Path $_.FullName
    Set-Location $solutionPath
    Write-Host $solutionPath
    gci *.sln
    dotnet build 
    dotnet new tool-manifest
    dotnet tool uninstall netpackageanalyzerconsole
    dotnet tool update netpackageanalyzerconsole
    Write-Host "Current path: $(Get-Location)"

    dotnet PackageAnalyzer generateFiles
    cd Analysis
    cd docs
    cd Analysis
    $destination = Join-Path $copyPath $index
    if(-not (Test-Path $destination)) {
        New-Item -ItemType Directory -Path $destination    
    }
    Copy-Item -Path * -Destination $destination -Recurse
    #npm install
    #npm run start
    # $processInfo = Start-Process npm -ArgumentList "run", "start" -PassThru -NoNewWindow
    
    #Start-Sleep -Milliseconds 30000
    
    # Stop-Process -Id $processInfo.Id
    # Set-Location $currentPath

}

Pop-Location

