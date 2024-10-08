﻿function AnalyzeProject ( $url  ){

Push-Location .

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

    Write-host $index "------>" $_.FullName 
    $solutionPath = Split-Path $_.FullName
    Set-Location $solutionPath
    Write-Host $solutionPath
    gci *.sln
    dotnet restore
    dotnet build 
    dotnet new tool-manifest
    dotnet tool uninstall netpackageanalyzerconsole
    dotnet tool update netpackageanalyzerconsole
    Write-Host "Current path: $(Get-Location)"
    # Push-Location .
    # dotnet PackageAnalyzer generateFiles -wg Docusaurus
    dotnet PackageAnalyzer generateFiles -wg HtmlSummary

    cd Analysis
    cd docs
    cd Analysis
    $destination = Join-Path $copyPath $index
    if(-not (Test-Path $destination)) {
        New-Item -ItemType Directory -Path $destination    
    }
    Copy-Item -Path * -Destination $destination -Recurse -Force
    Write-Host "Finding name folder : $solutionPath from $name"
    # $nameFolder = $solutionPath.Substring($solutionPath.IndexOf($name)+$name.Length+1).Replace('\', '_')
    $nameFolder = $name
    $jsonContent = @"
{
    "label": "$nameFolder",
    "position": 1,
    "link": {
        "type": "generated-index"
    }
}
"@
    
    $filePath = Join-Path $destination '_category_.json'
    
    Set-Content -Path $filePath -Value $jsonContent
    Write-Host "Please see : $destination"
    
    #npm install
    #npm run start
    # $processInfo = Start-Process npm -ArgumentList "run", "start" -PassThru -NoNewWindow
    
    #Start-Sleep -Milliseconds 30000
    
    # Stop-Process -Id $processInfo.Id
    # Set-Location $currentPath
}

Pop-Location
cls
}


#done
AnalyzeProject  "https://github.com/fullstackhero/dotnet-starter-kit"
#done
AnalyzeProject "https://github.com/ivanpaulovich/clean-architecture-manga"
#many sln
#$url ="https://github.com/evolutionary-architecture/evolutionary-architecture-by-example"
#done
AnalyzeProject "https://github.com/jasontaylordev/CleanArchitecture"
#done
AnalyzeProject "https://github.com/jbogard/ContosoUniversityDotNetCore-Pages"
#3 sln
AnalyzeProject "https://github.com/dotnet/eShop"
#the project do not compile -errors 
# $url="https://github.com/kgrzybek/modular-monolith-with-ddd"
AnalyzeProject "https://github.com/nitish-kaushik/aspnetcore-webapi-clean-architecture"
AnalyzeProject "https://github.com/nopSolutions/nopCommerce"
AnalyzeProject "https://github.com/dotnetcore/CAP"
AnalyzeProject "https://github.com/meysamhadeli/booking-microservices"
AnalyzeProject "https://github.com/rafaelfgx/Architecture"
AnalyzeProject "https://github.com/ardalis/CleanArchitecture"

AnalyzeProject  "https://github.com/danpdc/cleanArchitectureTemplate"

#TODO: https://github.com/topics/architecture?l=c%23&o=desc&s=stars

#TODO: https://github.com/simplcommerce/SimplCommerce
#TODO: https://github.com/grandnode/grandnode2
#TODO: https://github.com/gothinkster/aspnetcore-realworld-example-app
