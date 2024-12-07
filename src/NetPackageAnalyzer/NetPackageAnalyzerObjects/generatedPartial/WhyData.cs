namespace NetPackageAnalyzerObjects;

public class WhyData
{
    public string ProjectText { get; set; } = string.Empty;
    public string ProjectName()
    {
        if (ProjectText.Contains("Too many lines"))
            return string.Empty;
        var last = "has the following";
        var start = ProjectText.IndexOf(last);
        try
        {
            var text = ProjectText.Substring(0, ProjectText.IndexOf(last));
            text = text.Replace("'", string.Empty);
            text = text.Substring("Project".Length);
            text = text.Trim();
            return text;
        }
        catch (Exception)
        {
            Console.WriteLine($"!!! ProjectText: {ProjectText}");
            throw;

        }
        
    }
    public string WhyText { get; set; } = string.Empty;
}
