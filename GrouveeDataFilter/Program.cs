using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using GrouveeDataFilter.Filter_Templates;
using GrouveeDataFilter.Models;

namespace GrouveeDataFilter
{
    public class Program
    {

        public static string OutputFileName(FileInfo inputFile)
        {
            return $"{inputFile.DirectoryName}\\{inputFile.Name.Substring(0,inputFile.Name.Length - inputFile.Extension.Length)}-FinishedGameFilterOutput.csv";
        }

        public static void Main(string[] args)
        {
            var grouveeDataFile = new FileInfo(args[0]);

            var gdf = new GrouveeDataFilterOutputter<FinishedGameFilter.FinishedGameFilterModel>(
                new FinishedGameFilterOutputter(OutputFileName(grouveeDataFile)));

            gdf.Run(grouveeDataFile);
        }
    }
}
