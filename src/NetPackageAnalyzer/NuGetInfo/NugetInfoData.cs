namespace NuGetInfo;

public record NugetInfoData(string PackageId)
{

    private  static string GetNuGetLocation()
    {
        var envPath = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
        if (!String.IsNullOrEmpty(envPath))
            return envPath;

        string? nugetPackagesRoot = null;
        var OS = Environment.OSVersion;
        if (OS.Platform == PlatformID.Win32NT)
        {
            nugetPackagesRoot = Environment.GetEnvironmentVariable("UserProfile");
        }
        else if (OS.Platform == PlatformID.Unix ||  OS.Platform == PlatformID.MacOSX)
        {
            nugetPackagesRoot = Environment.GetEnvironmentVariable("HOME");
        }

        if (nugetPackagesRoot != null)
        {
            return Path.Combine(nugetPackagesRoot, ".nuget", "packages");
        }

        var userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(userPath, ".nuget", "packages");
    }

    public AnswersFindNuget GetNugetInfoLicence(string PackageVersion)
    {
        string folderPackage = Path.Combine(GetNuGetLocation(), PackageId.ToLower());
        
        if (!Directory.Exists(folderPackage))
        {
            return new FolderNotFound(folderPackage);
        }
        folderPackage = Path.Combine(folderPackage, PackageVersion);
        if (!Directory.Exists(folderPackage))
        {
            return new FolderNotFound(folderPackage);
        }
        var fileNuspec= Path.Combine(folderPackage, $"{PackageId}.nuspec");
        if (!File.Exists(fileNuspec))
        {
            return new FileNotFound(fileNuspec);
        }
        var nuspec = File.ReadAllText(fileNuspec);
        var doc = XDocument.Parse(nuspec);
        var license = doc.Descendants().FirstOrDefault(x => x.Name.LocalName == "license");
        if(license == null)
        {
            return new NoLicenseFound();
        }
        
        var value = license.Value;
        if (value == null)
        {
            return new NoLicenseFound();
        }
        string fileLicense = Path.Combine(folderPackage, value);
        if (File.Exists(fileLicense))
        {
            value=fileLicense;
        }
        return new LicenseFound(value);
    }
}
