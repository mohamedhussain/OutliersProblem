# Outliers Detection Problem
This application reads a set of data points from a .csv file on the disk and will try to determine the outliers in the data set.
It will print the outliers to the console window and will write the sanitized data with the outliers removed to a new .csv file on the disk.

## How to run the application
1. Clone the repository <https://github.com/mohamedhussain/OutliersProblem.git> onto your computer.
2. Open the solution file "**OutliersProblem.sln**" that is located in the root directory using Visual Studio.
3. There should be two projects in the solution.
   * OutliersProblem
   * OutliersProblem_Test
4. Right click on the "OutliersProblem" project and set it as the Startup Project.
5. Open the appsettings.json file and set the file path for the input file and the output file.
```json  
    "CSVRepoConfig": {
        "InputFile": "C:\\temp\\Outliers.csv",
        "Outputfile": "C:\\temp\\Outliers_Sanitized.csv"
    }
```
6. Compile and run the solution.
7. The outliers if any are displayed on the console. The sanitized data is written to the output file specified in the settings.

## Overview

The application defines two interfaces that each handle the main functions.
  * **IOutlierDetector** <br>
  This interface defines a method that takes in a collection of stock prices and a value to evaluate against the collection. It returns `true` when the value is an outlier.
  * **IStockPriceRepo**<br>
  This interface defines methods to read and write stock prices from a data source.

The main application uses the **Factory Method** pattern and **Dependency Injection** to create instances of these interfaces at runtime based on the values in the application settings.

This approach makes the application flexible. The application can be extended in the future to use a different repository or a new outlier detection algorithm with minimal code change. 