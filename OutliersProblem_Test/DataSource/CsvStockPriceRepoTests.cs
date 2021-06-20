using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutliersProblem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

namespace OutliersProblem.DataSource.Tests
{
    [TestClass()]
    public class CsvStockPriceRepoTests
    {
        private readonly CsvStockPriceRepo repo;
        private readonly MockFileSystem fs;

        public CsvStockPriceRepoTests()
        {
            fs = new MockFileSystem();
            var mockInputFile = new MockFileData(@"Date,Price
09/01/1990,100.4577362
10/01/1990,100.7628687
11/01/1990,100.9453853
12/01/1990,101.1339766
15/01/1990,101.6215304
16/01/1990,101.1287124
17/01/1990,100.6379304
18/01/1990,101.1124138
19/01/1990,101.3342192
23/01/1990,101.4379791
24/01/1990,101.6491346
25/01/1990,101.1454954
26/01/1990,101.2446378
29/01/1990,101.0405322
30/01/1990,110.9257881");

            fs.AddFile(@"C:\temp\in.csv", mockInputFile);

            var config = new CSVRepoConfig()
            {
                InputFile = @"C:\temp\in.csv",
                OutputFile = @"C:\temp\out.csv"
            };

            IOptions<CSVRepoConfig> appConfig = Options.Create(config);
            repo = new CsvStockPriceRepo(appConfig, fs);
        }

        [TestMethod()]
        public void CsvStockPriceRepoTest_ReadsFileCorrectly()
        {
            var data = repo.GetStockPrices();

            //  The method should read the file and return the correct number of records.
            Assert.IsNotNull(data);
            Assert.AreEqual(15, data.Count());
        }

        [TestMethod()]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CsvStockPriceRepoTest_ThrowExecption()
        {
            var config = new CSVRepoConfig()
            {
                InputFile = @"C:\temp\in.csv",
                OutputFile = @"C:\temp\out.csv"
            };

            IOptions<CSVRepoConfig> appConfig = Options.Create(config);
            var repo1 = new CsvStockPriceRepo(appConfig, new MockFileSystem());

            //  The method should throw a file not found exception
            _ = repo1.GetStockPrices();
        }

        [TestMethod()]
        public void GetStockPricesTest_WritesToFileCorrectly()
        {
            var data = new List<StockPrice>() {
                new StockPrice() { Date = new DateTime(1990, 1, 15), Price = 100.0000m },
                new StockPrice() { Date = new DateTime(1990, 2, 27), Price = 101.0000m },
                new StockPrice() { Date = new DateTime(1990, 3, 31), Price = 102.0000m }
            };

            //  The method should write the data correctly to the file.
            var result = repo.SaveStockPrices(data);

            //  Get the file that was written
            MockFileData mockOutputFile = fs.GetFile(@"C:\temp\out.csv");

            string[] outputLines = mockOutputFile.TextContents.SplitLines();

            //  Check values.
            Assert.IsTrue(result);
            Assert.AreEqual("Date,Price", outputLines[0]);
            Assert.AreEqual("15/01/1990,100.0000", outputLines[1]);
            Assert.AreEqual("27/02/1990,101.0000", outputLines[2]);
            Assert.AreEqual("31/03/1990,102.0000", outputLines[3]);
        }
    }
}