using Microsoft.Extensions.Options;
using OutliersProblem.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;

namespace OutliersProblem.DataSource
{
    /// <summary>
    /// Reads and writes the stock prices from a CSV file.
    /// </summary>
    public class CsvStockPriceRepo : IStockPriceRepo
    {
        private readonly string inputFilePath;
        private readonly string outputFilePath;
        private readonly IFileSystem fileSystem;

        public CsvStockPriceRepo(IOptions<CSVRepoConfig> options, IFileSystem fileSystem)
        {
            inputFilePath = options?.Value?.InputFile;
            outputFilePath = options?.Value?.OutputFile;

            this.fileSystem = fileSystem;
        }

        //  use default implementation which calls System.IO
        public CsvStockPriceRepo(IOptions<CSVRepoConfig> options)
            : this(options, fileSystem: new FileSystem()) { }

        /// <summary>
        /// Returns the Stock Prices from a CSV file.
        /// </summary>
        /// <returns>An enumerable collection of Stock Prices</returns>
        public IEnumerable<StockPrice> GetStockPrices()
        {
            var stockPrices = new List<StockPrice>();

            //  Check if the file exists
            if (!fileSystem.File.Exists(inputFilePath))
            {
                throw new FileNotFoundException("File not found.", inputFilePath);
            }

            //  Open the input file to read its content.
            using (var reader = fileSystem.File.OpenText(inputFilePath))
            {
                //  Skip the first line because it contains the headers.
                reader.ReadLine();

                //  Read the file until the end
                while (!reader.EndOfStream)
                {
                    //  Read the next line
                    var line = reader.ReadLine();

                    //  Split the row data
                    var values = line.Split(',');

                    //  Add the data into the in-memory list
                    stockPrices.Add(new StockPrice() { Date = DateTime.ParseExact(values[0], "dd/MM/yyyy", CultureInfo.InvariantCulture), Price = Convert.ToDecimal(values[1]) });
                }
            }

            return stockPrices;
        }

        /// <summary>
        /// Saves the Stock Prices to a CSV file.
        /// </summary>
        /// <param name="values">An enumerable collection of Stock Prices</param>
        /// <returns>True if the operation succeeds else false.</returns>
        public bool SaveStockPrices(IEnumerable<StockPrice> values)
        {
            //  Open output file to write to it. If the file does not exists then create it.
            using (var writer = fileSystem.File.CreateText(outputFilePath))
            {
                //  Write the header
                writer.WriteLine("Date,Price");

                //  loop through the collection and write it to the output file.
                foreach (var item in values)
                {
                    writer.WriteLine($"{item.Date:dd/MM/yyyy},{item.Price}");
                }
            }

            return true;
        }
    }
}