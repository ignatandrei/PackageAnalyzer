$url=  "https://github.com/fullstackhero/dotnet-starter-kit"
$url="https://github.com/ivanpaulovich/clean-architecture-manga"
$url ="https://github.com/evolutionary-architecture/evolutionary-architecture-by-example"
$url="https://github.com/jasontaylordev/CleanArchitecture"
$url="https://github.com/jbogard/ContosoUniversityDotNetCore-Pages"
$url="https://github.com/dotnet/eShop"
$url="https://github.com/kgrzybek/modular-monolith-with-ddd"


$name = $url.Split('/')[-1]
Push-Location ..

if(-not (Test-Path $name)) {
    git clone $url
}

cd $name
$currentPath = Get-Location
$sln = (Get-ChildItem *.sln -r)
Write-Host "found $sln.Count solutions"
$sln | ForEach-Object {
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
    npm install
    #npm run start
    # Start the npm process and redirect output to null to keep the script running
    $processInfo = Start-Process npm -ArgumentList "run", "start" -PassThru -NoNewWindow
    
    # Wait for 1 minute (60000 milliseconds)
    Start-Sleep -Milliseconds 30000
    
    # Stop the process
    Stop-Process -Id $processInfo.Id
    Set-Location $currentPath

}

Pop-Location

