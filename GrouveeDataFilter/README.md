# GrouveeDataFilter
Parser for CSVs exported from Grouvee

## Requirements

### To build and run (what I used, others may work)

 - Visual Studio 2015
 - nuget

### To use/make your own

 - Knowledge of C# or willingness and ability to learn

## Demo

In the Demo folder you will find an excel file. To use it:

1. Build the project
2. Get your Grouvee export .csv from grouvee.com (Your avatar in the top right -> Settings -> Export your collection to a CSV file)
3. Drag the .csv file over the GrouveeDataFilter.exe in [Solution folder]\\GrouveeDataFilter\\bin\\Debug
4. An output file should appear in the same folder as the .csv
5. Open it in a text editor, copy the entire contents (Ctrl+A, Ctrl+C)
6. Open [the Excel file](Demo/FinishedGameGraph.xlsx) in the Demo folder in Excel
7. Paste the csv output you copied into cell A1
8. Open the Data Import/Text to columns tool (Data tab)
9. On the first screen, select delimited
10. On the second screen, select only comma
11. Click finish
12. Right click the bottom axis and set dates that make sense
13. You may have to redo the data labels as well, they seem to get messed up
14. Congratulations, you now have a timeline of all your finished games

## Creating your own

All the code is commented fairly thoroughly, here are some pointers to get you started:

### `GrouveeDataFilter.cs`

The meat and bones, the first thing here of interest is the `ParseCSV` function with parses a Grouvee export .csv into a C# object. A `GrouveeDataFilter` object then takes that data and runs it through a filter, sort, selection and optionally outputs it in some specific way (`GrouveeDataFilterOutputter`).

### `IFilterTemplate.cs`

You can input delegates like lambdas into GrouveeDataFilter, or if you like you can write the various functions as part of a class implementing theses interfaces, see the Filter Templates folder for examples