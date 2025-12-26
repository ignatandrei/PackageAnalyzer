
namespace NetPackageAnalyzerTests;

public class TestSolutionExists
{
    [Fact]
    public async Task TestDoesNotWorkWithoutSln()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\myfile.txt", new MockFileData("Testing is meh.") },
           });
        var g = new GenerateFilesDocusaurus(fileSystem);
        var res =await g.GenerateDataForSln(@"C:\");
        Assert.False(res);
    }
}