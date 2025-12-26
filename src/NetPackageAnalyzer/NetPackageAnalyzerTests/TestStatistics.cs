using Statistical;

namespace NetPackageAnalyzerTests;
public class TestStatistics
{

    [Fact]
    public void TestMedian()
    {
        var values = new int[] { 2, 100,  300, 400, 500 };
        var median = StatisticalNumbers<int>.Median(values);
        Assert.Equal(300, median);
    }

    [Fact]
    public void TestMean()
    {
        var values = new int[] { 5, 100, 300, 400, 500 };
        var median = StatisticalNumbers<int>.ArithmeticMean(values);
        Assert.Equal(261, median);
    }
    [Fact]
    public void TestMax()
    {
        var values = new int[] { 5, 1000, 300, 400, 500 };
        var median = StatisticalNumbers<int>.Max(values);
        Assert.Equal(1000, median);
    }
    [Fact]
    public void TestMin()
    {
        var values = new int[] { 15, 1000, 300,5, 400, 500 };
        var median = StatisticalNumbers<int>.Min(values);
        Assert.Equal(5, median);
    }
    //[TestMethod]
    //public void TestMode()
    //{
    //    var values = new int[] { 5, 5,100,100,100, 300, 400, 500 };
    //    var modes = StatisticalNumbers<int>.Mode(values);
    //    Assert.AreEqual(1, modes.Count());
    //    var mode = modes.FirstOrDefault();
    //    var res= new int[] { 100 };
    //    Assert.AreEqual(1, mode.Count());
    //    Assert.AreEqual(res.Length, mode.Values.Length);
    //    for (int i = 0; i < res.Length; i++)
    //    {
    //        Assert.AreEqual(res[i], mode.Values[i]);
    //    }
    //    values = new int[] { 5, 5, 5, 100, 100, 100, 300, 400, 500 };
    //    modes = StatisticalNumbers<int>.Mode(values);
    //    Assert.AreEqual(2, modes.Count());
    //    mode = modes.FirstOrDefault();
    //    res = new int[] { 5,100 };
    //    Assert.AreEqual(1, mode.Count());
    //    Assert.AreEqual(1, mode.Values.Length);
    //    for (int i = 0; i < res.Length; i++)
    //    {
    //        Assert.AreEqual(res[i], mode.Values[i]);
    //    }
    //}
}
