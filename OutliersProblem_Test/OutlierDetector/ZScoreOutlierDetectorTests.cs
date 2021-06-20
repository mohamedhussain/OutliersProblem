using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutliersProblem.Models;
using System;
using System.Collections.Generic;

namespace OutliersProblem.OutlierDetector.Tests
{
    [TestClass]
    public class ZScoreOutlierDetectorTests
    {
        private readonly List<StockPrice> prices;
        private readonly ZScoreOutlierDetector detector;

        public ZScoreOutlierDetectorTests()
        {
            prices = new List<StockPrice>();

            var config = new ZScoreConfig()
            {
                Threshold = 2.5
            };
            IOptions<ZScoreConfig> appConfig = Options.Create(config);
            detector = new ZScoreOutlierDetector(appConfig);
        }

        [TestMethod]
        public void IsOutlierTest()
        {
            //  Arrange
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 100.4577m });

            //  When there is only one value in the collection then should return false.
            Assert.AreEqual(false, detector.IsOutlier(prices[^1].Price, prices));

            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 100.2577m });

            //  Should return false
            Assert.AreEqual(false, detector.IsOutlier(prices[^1].Price, prices));
        }

        [TestMethod()]
        public void IsOutlierTest_DetectOutlier()
        {
            //  Test if the method detects an outlier
            //  Arrange
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 100.4573457m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 2), Price = 100.2577679m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 3), Price = 100.7628687m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 4), Price = 100.9453853m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 5), Price = 101.1339766m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 6), Price = 101.6215304m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 7), Price = 101.1287124m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 8), Price = 100.6379304m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 9), Price = 101.1124138m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 10), Price = 101.3342192m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 11), Price = 101.2557526m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 12), Price = 101.4379791m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 13), Price = 150.649146m });

            //  Should return true
            Assert.AreEqual(true, detector.IsOutlier(prices[^1].Price, prices));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsOutlierTest_ThrowNullExecption()
        {
            //  Test if the method throws ArgumentNullException exception when the collection is null
            detector.IsOutlier(100, null);
        }
    }
}