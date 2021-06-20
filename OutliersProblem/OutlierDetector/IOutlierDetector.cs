using OutliersProblem.Models;
using System.Collections.Generic;

namespace OutliersProblem.OutlierDetector
{
    public interface IOutlierDetector
    {
        /// <summary>
        /// This method takes a set of stock prices and determines if the given price is an outlier or not.
        /// </summary>
        /// <param name="price">A decimal to evaluate.</param>
        /// <param name="stockPrices">A list of stock prices.</param>
        /// <returns>True if the price is an outlier else false.</returns>
        bool IsOutlier(decimal price, in IEnumerable<StockPrice> stockPrices);
    }
}