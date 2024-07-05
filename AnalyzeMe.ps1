Get-ChildItem -Path "docs" -File -Recurse |
Where-Object { $_.Name -ne "ico.png"} |
Remove-Item

Get-ChildItem -Path "docs"  -Recurse |
Where-Object { $_.Name -ne "ico.png"} |
Remove-Item -Recurse
$currentPath = Get-Location
$destination = Join-Path $currentPath "docs"
Push-Location .
Set-Location -Path "src"
Set-Location -Path "NetPackageAnalyzer"
dotnet build 
dotnet new tool-manifest
dotnet tool uninstall netpackageanalyzerconsole
dotnet tool update netpackageanalyzerconsole
Write-Host "Current path: $(Get-Location)"

dotnet PackageAnalyzer generateFiles --where $destination

Pop-Location