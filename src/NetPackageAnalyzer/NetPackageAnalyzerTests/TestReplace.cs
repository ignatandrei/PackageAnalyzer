using System;
using System.Collections.Generic;
using System.Text;

namespace NetPackageAnalyzerTests;

public class TestReplace
{
    [Fact]
    public void TestReplacePNG()
    {
        var line = """<div class="mermaid" title="image projects without tests">""";
        var titleToReplace = """title="image """;
        var index = line.IndexOf(titleToReplace);
        var next = line.IndexOf('"', index + titleToReplace.Length + 1);
        var title = line.Substring(index+ titleToReplace.Length, next - titleToReplace.Length - index );
        var nameFile = title.Replace(" ", "-");
        nameFile += ".png";
        Assert.Equal("projects-without-tests.png", nameFile);
    }
}
