using OutliersProblem.DataSource;
using OutliersProblem.Models;
using OutliersProblem.OutlierDetector;
using System.Collections.Generic;

namespace OutliersProblem.Service
{
    public class OutliersScanner : IOutliersScanner
    {
        private readonly IStockPriceRepo _stockPriceRepo;
        private readonly IOutlierDetector _outlierDetector;
        private readonly int queueLength;

        public OutliersScanner(IStockPriceRepo stockPriceRepo, IOutlierDetector outlierDetector)
        {
            _stockPriceRepo = stockPriceRepo;
            _outlierDetector = outlierDetector;

            queueLength = 25;
        }

        /// <summary>
        /// Gets a list of stock prices from the repo and then scans them for outliers. 
        /// Writes the non-outlier stock prices to the repo and returns a list of outliers.
        /// This method checks the stock price to 25 preceding values to determine if it is an outlier or not.
        /// </summary>
        /// <returns>An enumerable collection of Stock Prices</returns>
        public IEnumerable<StockPrice> ScanForOutliers()
        {
            var queue = new Queue<StockPrice>(queueLength);
            var sanatizedList = new List<StockPrice>();
            var outliersList = new List<StockPrice>();

            //  Step 1 : Get the dataset from the repo
            var stockPrices = _stockPriceRepo.GetStockPrices();

            //  Step 2 : Loop through the data and check if each value is an outlier or not.
            foreach (var item in stockPrices)
            {
                //  The queue is used to keep a list of the last 25 prices.
                //  If the queue is full then remove the last item and the insert the new value.
                if (queue.Count >= queueLength)
                    queue.Dequeue();

                queue.Enqueue(item);

                //  Check if the current item is an outlier or not
                if (_outlierDetector.IsOutlier(item.Price, queue))
                {
                    //  If current item is an outlier then add to the outlier list.
                    outliersList.Add(item);
                }
                else
                {
                    //  If current item is not an outlier then add it to the sanitized list
                    sanatizedList.Add(item);
                }
            }

            //  Step 3 :  Write the sanitized list to the repo.
            _stockPriceRepo.SaveStockPrices(sanatizedList);

            //  Step 4 : return the list of outliers
            return outliersList;
        }
    }
}