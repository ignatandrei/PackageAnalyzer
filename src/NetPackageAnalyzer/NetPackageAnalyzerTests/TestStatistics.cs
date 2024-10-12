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
        var modes = StatisticalNumbers<int>.Mode(values);
        Assert.AreEqual(1, modes.Count());
        var mode = modes.FirstOrDefault();
        var res= new int[] { 100 };
        Assert.AreEqual(3, mode.Count());
        Assert.AreEqual(res.Length, mode.Values.Length);
        for (int i = 0; i < res.Length; i++)
        {
            Assert.AreEqual(res[i], mode.Values[i]);
        }
        values = new int[] { 5, 5, 5, 100, 100, 100, 300, 400, 500 };
        modes = StatisticalNumbers<int>.Mode(values);
        Assert.AreEqual(1, modes.Count());
        mode = modes.FirstOrDefault();
        res = new int[] { 5,100 };
        Assert.AreEqual(3, mode.Count());
        Assert.AreEqual(res.Length, mode.Values.Length);
        for (int i = 0; i < res.Length; i++)
        {
            Assert.AreEqual(res[i], mode.Values[i]);
        }
    }
}
