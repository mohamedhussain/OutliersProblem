using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OutliersProblem.DataSource;
using OutliersProblem.Models;
using OutliersProblem.OutlierDetector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OutliersProblem.Service.Tests
{
    [TestClass()]
    public class OutliersScannerTests
    {
        private readonly List<StockPrice> data;
        private readonly IOutliersScanner scanner;
        private readonly Mock<IStockPriceRepo> repoMock;
        private readonly Mock<IOutlierDetector> detectorMock;

        public OutliersScannerTests()
        {
            data = new List<StockPrice>()
            {
                new StockPrice() { Date = new DateTime(1990, 1, 1), Price = 100.0000m },
                new StockPrice() { Date = new DateTime(1990, 1, 2), Price = 102.0000m },
                new StockPrice() { Date = new DateTime(1990, 1, 3), Price = 104.0000m },
                new StockPrice() { Date = new DateTime(1990, 1, 4), Price = 106.0000m },
                new StockPrice() { Date = new DateTime(1990, 1, 5), Price = 122.0000m }
            };

            repoMock = new Mock<IStockPriceRepo>();

            repoMock.Setup(m => m.GetStockPrices()).Returns(() => data);
            repoMock.Setup(m => m.SaveStockPrices(It.IsAny<IEnumerable<StockPrice>>())).Returns(() => true);

            detectorMock = new Mock<IOutlierDetector>();
            detectorMock.Setup(m => m.IsOutlier(It.IsAny<decimal>(), It.Ref<IEnumerable<StockPrice>>.IsAny))
                .Returns((decimal price, IEnumerable<StockPrice> stockPrices) => price >= 110m);

            scanner = new OutliersScanner(repoMock.Object, detectorMock.Object);
        }

        [TestMethod()]
        public void OutliersScannerTest()
        {
            var outliers = scanner.ScanForOutliers();

            //  Verify that all the methods where called
            repoMock.Verify(mock => mock.GetStockPrices(), Times.Once());
            repoMock.Verify(mock => mock.SaveStockPrices(It.IsAny<IEnumerable<StockPrice>>()), Times.Once());
            detectorMock.Verify(mock => mock.IsOutlier(It.IsAny<decimal>(), It.Ref<IEnumerable<StockPrice>>.IsAny), Times.Exactly(data.Count));

            //  Verify that the correct data is returned.
            Assert.AreEqual(1, outliers.Count());
            Assert.AreEqual(data[^1], outliers.FirstOrDefault());
        }
    }
}