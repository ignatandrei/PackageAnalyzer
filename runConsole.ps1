Push-location .
cd src
cd NetPackageAnalyzer
gci *.sln | % { & $_.FullName }
cd NetPackageAnalyzerConsole
[console]::TreatControlCAsInput = $true
dotnet watch run --no-hot-reload
Pop-Location
