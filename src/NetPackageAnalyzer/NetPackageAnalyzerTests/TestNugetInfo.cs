
namespace NetPackageAnalyzerTests;
[TestClass]
public class TestNugetInfo
{
    [TestMethod]
    public void TestFindVersionNuget()
    {
        var nugetInfo = new NugetInfoData("RSCG_NameGenerator" );
        var res = nugetInfo.GetNugetInfoLicence("2024.26.8.2002");
        if(res.TryGetLicenseFound(out var lic))
        {
            Assert.AreEqual("MIT",  lic.license);
        }
        else
        {
            Assert.Fail("License not found");
        }
    }
}
