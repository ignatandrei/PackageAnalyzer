using NetPackageAnalyzerConsole;
using NPA.ProcessRunner;

namespace NetPackageAnalyzerConsole.Tests;

public class TestProcessStarter
{
    [Fact]
    public void DeserializeEshop()
    {
        FakeProcessRunner fakeProcessRunner = new ();
        fakeProcessRunner.DeserializeFromFolder("eShopJSON");
        string s = ";";
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
