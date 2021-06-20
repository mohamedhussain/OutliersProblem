using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutliersProblem.DataSource;
using OutliersProblem.Models;
using OutliersProblem.OutlierDetector;
using OutliersProblem.Service;
using System;
using System.Collections.Generic;
using System.IO;

namespace OutliersProblem
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //  Setup the configuration builder so that we can read in the appsettings file
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            //  Setup all the services that the application will need to use
            var services = new ServiceCollection();
            ConfigureServices(services, configuration);
            var serviceProvider = services.BuildServiceProvider();

            /*
             *  Here an instance of the IOutliersScanner is created by calling the built in Dependency Injector.
             *  The DI resolves the correct types for IOutlierDetector and IStockPriceRepo using a factory method 
             *  and the settings in the appsetting file. The concrete types are injected into OutliersScanner constructor.
             *  
             *  Alternative implementations of the IOutlierDetector and IStockPriceRepo types can be easily 
             *  added to the application with minimal code change.
             *  
             */

            //  Get an instance of the OutliersScanner from the DI
            var service = serviceProvider.GetService<IOutliersScanner>();

            //  Call the Scan method to determine the outliers
            Console.WriteLine("Scanning Stock Prices to identify outliers");
            var outliers = (List<StockPrice>)service.ScanForOutliers();

            //  Log the result to the console.            
            if (outliers.Count == 0)
            {
                Console.WriteLine("No outliers detected.");
            }
            else
            {
                Console.WriteLine("The outliers are...\n\nDate\t\tPrice");
                
                foreach (var item in outliers)
                {
                    Console.WriteLine($"{item.Date:dd/MM/yyyy}\t{item.Price}");
                }
            }

            Console.ReadLine();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions()
                .Configure<ZScoreConfig>(configuration.GetSection("ZScoreConfig"))
                .Configure<CSVRepoConfig>(configuration.GetSection("CSVRepoConfig"));

            //  Register the various interfaces and their concrete implementation with the DI
            services.AddScoped<IOutliersScanner, OutliersScanner>();

            services.AddScoped<CsvStockPriceRepo>();
            services.AddScoped<IStockPriceRepo, CsvStockPriceRepo>(s => s.GetService<CsvStockPriceRepo>());

            services.AddScoped<ZScoreOutlierDetector>();
            services.AddScoped<IOutlierDetector, ZScoreOutlierDetector>(s => s.GetService<ZScoreOutlierDetector>());

            services.AddScoped<DbScan_OutlierDetector>();
            services.AddScoped<IOutlierDetector, DbScan_OutlierDetector>(s => s.GetService<DbScan_OutlierDetector>());

            //  Set up a factory method to resolve the correct IOutlierDetector instance
            services.AddTransient<IOutlierDetector>((IServiceProvider serviceProvider) =>
            {
                //  Read from the configuration the OutlierDetector type to use.
                var detectorType = configuration.GetValue<string>("OutlierDetector");

                //  Instantiate and return the requested version of IOutlierDetector
                switch (detectorType)
                {
                    case "Z-Score":
                        return (IOutlierDetector)serviceProvider.GetService(typeof(ZScoreOutlierDetector));

                    case "DbScan":
                        return (IOutlierDetector)serviceProvider.GetService(typeof(DbScan_OutlierDetector));

                    default:
                        throw new NotImplementedException();
                }
            });

            //  Set up a factory method to resolve the correct IStockPriceRepo instance
            services.AddTransient<IStockPriceRepo>((IServiceProvider serviceProvider) =>
            {
                //  Read from the configuration the DataSource type to use.
                var detectorType = configuration.GetValue<string>("DataSource");

                //  Instantiate and return the requested version of IStockPriceRepo
                switch (detectorType)
                {
                    case "CSV":
                        return (IStockPriceRepo)serviceProvider.GetService(typeof(CsvStockPriceRepo));

                    default:
                        throw new NotImplementedException();
                }
            });
        }
    }
}