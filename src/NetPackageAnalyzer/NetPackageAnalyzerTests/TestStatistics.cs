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

    [TestMethod]
    public void TestMode()
    {
        var values = new int[] { 5, 5,100,100,100, 300, 400, 500 };
        var mode = StatisticalNumbers<int>.Mode(values);
        var res= new int[] { 100 };
        Assert.AreEqual(3, mode.Count);
        Assert.AreEqual(res.Length, mode.Values.Length);
        for (int i = 0; i < res.Length; i++)
        {
            Assert.AreEqual(res[i], mode.Values[i]);
        }
        values = new int[] { 5, 5, 5, 100, 100, 100, 300, 400, 500 };
        mode = StatisticalNumbers<int>.Mode(values);
        res = new int[] { 5,100 };
        Assert.AreEqual(3, mode.Count);
        Assert.AreEqual(res.Length, mode.Values.Length);
        for (int i = 0; i < res.Length; i++)
        {
            Assert.AreEqual(res[i], mode.Values[i]);
        }
    }
}
