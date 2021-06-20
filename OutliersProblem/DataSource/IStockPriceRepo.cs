using OutliersProblem.Models;
using System.Collections.Generic;

namespace OutliersProblem.DataSource
{
    public interface IStockPriceRepo
    {
        /// <summary>
        /// Returns an enumerable collection of Stock prices from the data source
        /// </summary>
        /// <returns>An enumerable collection of Stock Prices</returns>
        IEnumerable<StockPrice> GetStockPrices();

        /// <summary>
        /// Saves the Stock prices to the data source
        /// </summary>
        /// <param name="values">An enumerable collection of Stock Prices</param>
        /// <returns>True if the operation succeeds else false.</returns>
        bool SaveStockPrices(IEnumerable<StockPrice> values);
    }
}