using NetPackageAnalyzerConsole;
using NPA.ProcessRunner;

namespace NetPackageAnalyzerConsole.Tests;

public class TestProcessStarter
{
    [Fact]
    public async Task DeserializeEshop()
    {
        FakeProcessRunner fakeProcessRunner = new ();
        fakeProcessRunner.DeserializeFromFolder("eShopJSON");
        RealMainExecuting.ProcessRunner = fakeProcessRunner;
        var args = new[] { "generateFiles",
            "--folder", @"D:\eu\GitHub\eShop",
            "-wg","HtmlSummary",
            "--where", @"D:\eu\GitHub\PackageAnalyzer\src\documentation1\",
            "--verbose","true"
        };

        await RealMainExecuting.RealMain(args);
    }
    [Fact]
    public void CreateRunProductStartInfo_ConfiguresExpectedCommand()
    {
        var startInfo = RealMainExecuting.CreateRunProductStartInfo(@"D:\temp\analysis");

        Assert.Equal("cmd", startInfo.FileName);
        Assert.Equal("/c npm i && npm run start", startInfo.Arguments);
        Assert.Equal(@"D:\temp\analysis", startInfo.WorkingDirectory);
        Assert.False(startInfo.UseShellExecute);
        Assert.False(startInfo.CreateNoWindow);
    }

}
