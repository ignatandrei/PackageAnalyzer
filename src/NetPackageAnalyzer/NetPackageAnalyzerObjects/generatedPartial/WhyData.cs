namespace NetPackageAnalyzerObjects;

public class WhyData
{
    public string ProjectText { get; set; } = string.Empty;
    public string ProjectName()
    {
        var last = "has the following";
        var start = ProjectText.IndexOf(last);
        var text = ProjectText.Substring(0,ProjectText.IndexOf(last));
        text=text.Replace("'", string.Empty);
        text = text.Substring("Project".Length);
        text =text.Trim();
        return text;
    }
    public string WhyText { get; set; } = string.Empty;
}
