using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutliersProblem;
using OutliersProblem.Models;
using OutliersProblem.OutlierDetector;
using System.Collections.Generic;
using System;

namespace OutliersProblem_Test
{
    [TestClass]
    public class ZScore_OutlierDetector_Test
    {
        private readonly List<StockPrice> prices;
        private readonly ZScore_OutlierDetector detector;

        public ZScore_OutlierDetector_Test()
        {
            prices = new List<StockPrice>();
            detector = new ZScore_OutlierDetector(2.5);
        }
        [TestMethod]
        public void IsOutlier_Test()
        {
            //  Arrange
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 100.4577m });

            //  When there is only one value in the collection then should return false.
            Assert.AreEqual(false, detector.IsOutlier(prices[^1].Price, prices));

            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 100.2577m });

            //  Should return false
            Assert.AreEqual(false, detector.IsOutlier(prices[^1].Price, prices));

            //  Add more values to the sample
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 100.7628687m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 100.9453853m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 101.1339766m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 101.6215304m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 101.1287124m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 100.6379304m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 101.1124138m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 101.3342192m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 101.2557526m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 101.4379791m });
            prices.Add(new StockPrice() { Date = new DateTime(2000, 1, 1), Price = 1501.649146m });

            //  Should return true
            Assert.AreEqual(true, detector.IsOutlier(prices[^1].Price, prices));
        }
    }
}
