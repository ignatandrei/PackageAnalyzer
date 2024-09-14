Get-ChildItem -Path "docs" -File -Recurse |
Where-Object { $_.Name -ne "ico.png"} |
Remove-Item

Get-ChildItem -Path "docs"  -Recurse |
Where-Object { $_.Name -ne "ico.png"} |
Remove-Item -Recurse



$currentPath = Get-Location

$docs = Join-Path $currentPath "docs"

$destination = Join-Path $currentPath "src"
$destination = Join-Path $destination "documentation1"

Remove-Item $destination -Recurse


Push-Location .
Set-Location -Path "src"
Set-Location -Path "NetPackageAnalyzer"
dotnet build 
# dotnet new tool-manifest
dotnet tool uninstall netpackageanalyzerconsole
dotnet tool update netpackageanalyzerconsole
Write-Host "Current path: $(Get-Location)"

Push-Location .

dotnet PackageAnalyzer generateFiles -wg Docusaurus --where $destination

Set-Location $destination

npm i
npm run build
$destination = Join-Path $destination "build"
Set-Location $destination
Copy-Item -Path .\* -Destination $docs -Recurse 

Pop-Location

Write-Host "Current path: $(Get-Location)"

dotnet PackageAnalyzer generateFiles -wg HtmlSummary --where $docs

Pop-Location
