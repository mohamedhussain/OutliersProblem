using OutliersProblem.Models;
using System.Collections.Generic;

namespace OutliersProblem.Service
{
    public interface IOutliersScanner
    {
        /// <summary>
        /// Gets a list of stock prices from the repo and then scans them for outliers. 
        /// Writes the non-outlier stock prices to the repo and returns the list of outliers.
        /// </summary>
        /// <returns>An enumerable collection of Stock Prices</returns>
        IEnumerable<StockPrice> ScanForOutliers();
    }
}