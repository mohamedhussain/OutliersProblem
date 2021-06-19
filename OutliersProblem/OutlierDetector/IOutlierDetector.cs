using OutliersProblem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutliersProblem.OutlierDetector
{
    public interface IOutlierDetector
    {
        /// <summary>
        /// This method takes a set of stock prices and the determines if the given price is an outlier or not.
        /// </summary>
        /// <param name="stockPrices">A list of stock prices.</param>
        /// <param name="price">A decimal to evaluate.</param>
        /// <returns>True if the price is an outlier else false.</returns>
        bool IsOutlier(decimal price, in IEnumerable<StockPrice> stockPrices);
    }
}
