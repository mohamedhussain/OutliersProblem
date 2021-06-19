using OutliersProblem.Models;
using System;
using System.Collections.Generic;

namespace OutliersProblem.OutlierDetector
{
    /// <summary>
    /// Implements the Z Score algorithm to determine if a given value is an outlier or not
    /// </summary>
    public class ZScore_OutlierDetector : IOutlierDetector
    {
        private readonly double _threshold;

        public ZScore_OutlierDetector(double threshold)
        {
            _threshold = threshold;
        }

        /// <summary>
        /// This method takes a set of stock prices and the determines if the given price is an outlier or not.
        /// The method determines the standard score of the given price and if the score falls outside of the threshold then it returns true else it returns false. 
        /// </summary>
        /// <param name="stockPrices">A sample list of stock prices to evaluate the given price against.</param>
        /// <param name="price">The decimal value to evaluate.</param>
        /// <returns>True if the price is an outlier else false.</returns>
        public bool IsOutlier(decimal price, in IEnumerable<StockPrice> stockPrices)
        {
            if (stockPrices is null)
            {
                throw new ArgumentNullException(nameof(stockPrices));
            }

            //  Step 1 : Calculate the z score of the given price
            var zs = CalculateZscore((double)price, stockPrices);

            //  Step 2 : Check if the Z score if above the threshold. Return true if it is outside the bounds else return false.
            return Math.Abs(zs) > _threshold;
        }

        /// <summary>
        /// Returns the Z Score of the specified value compared to a sample data set.
        /// </summary>
        /// <param name="price">A double-precision floating-point value for which the Z Score has to be calculated.</param>
        /// <param name="stockPrices">A sample list of stock prices to evaluate the given price against.</param>
        /// <returns>The Z Score of the price</returns>
        private double CalculateZscore(double price, in IEnumerable<StockPrice> stockPrices)
        {
            /*
                The Z Score is calculated using the formula
                z = (x - m)/ s

                x = An element of the series
                m = The mean of the sample
                s = The standard deviation of the sample
            */

            double sum = 0d, bigSum = 0d;

            if (stockPrices is null)
            {
                throw new ArgumentNullException(nameof(stockPrices));
            }

            int i = 0;

            //  Step 1 : Calculate the mean of the samples

            // Calculate the sum of the values
            foreach (var item in stockPrices)
            {
                sum += (double)item.Price;
                i++;
            }

            var mean = sum / i;

            //  If there is only one item in the collection then return 1 as the z score
            if (i == 1 && mean == price)
            {
                return 1d;
            }

            //  Step 2 : Calculate the standard deviation of the sample

            // Calculate the total for the standard deviation
            foreach (var item in stockPrices)
            {
                bigSum += Math.Pow((double)item.Price - mean, 2);
            }

            // Now we can calculate the standard deviation
            var stdDev = Math.Sqrt(bigSum / i);

            //  Step 3 : Calculate the z score and return it
            return (price - mean) / stdDev;
        }
    }
}
