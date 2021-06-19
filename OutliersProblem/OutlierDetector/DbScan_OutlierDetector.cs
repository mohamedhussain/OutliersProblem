using OutliersProblem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutliersProblem.OutlierDetector
{
    /// <summary>
    /// A sample implementation of the IOutlierDetector interface to demonstrate how multiple outlier detection algorithms can be used with this application.
    /// </summary>
    public class DbScan_OutlierDetector : IOutlierDetector
    {
        public DbScan_OutlierDetector()
        {

        }

        public bool IsOutlier(decimal price, in IEnumerable<StockPrice> stockPrices)
        {
            throw new NotImplementedException();
        }
    }
}
