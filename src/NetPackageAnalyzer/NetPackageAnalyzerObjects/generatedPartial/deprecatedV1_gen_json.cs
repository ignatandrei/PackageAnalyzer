namespace NS_GeneratedJson_deprecatedV1_gen_json;

[Packages(false)]
public partial class deprecatedV1_gen_json
{   
    public string[]? PackagesID()
    {
        if (!HasProjects()) return null;
        var ids = Projects!
            .Where(it=>it!=null && it.Frameworks != null)
            .SelectMany(it=>it.Frameworks!)
            .Where(it=>it!=null && it.TopLevelPackages!=null)
            .SelectMany(it=>it.TopLevelPackages!)
            .Where(it=>!string.IsNullOrWhiteSpace(it.Id))
            .Select(it=>it.Id!)
            .ToArray();

        return ids;

    }
}

