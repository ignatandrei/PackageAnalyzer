using Statistical;

namespace NetPackageAnalyzerTests;
[TestClass]
public class TestStatistics
{

    [TestMethod]
    public void TestMedian()
    {
        var values = new int[] { 2, 100,  300, 400, 500 };
        var median = StatisticalNumbers<int>.Median(values);
        Assert.AreEqual(300, median);
    }

    [TestMethod]
    public void TestMean()
    {
        var values = new int[] { 5, 100, 300, 400, 500 };
        var median = StatisticalNumbers<int>.ArithmeticMean(values);
        Assert.AreEqual(261, median);
    }


}
