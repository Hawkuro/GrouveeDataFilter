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
        /// <summary>
        /// Given a file, creates a string full file name that is identical but with
        /// and added "-FinishedGameFilterOutput" before the extension
        /// </summary>
        /// <param name="inputFile">The file to base the new file name off of</param>
        /// <returns>A string file name as described</returns>
        public static string OutputFileName(FileInfo inputFile)
        {
            return $"{inputFile.DirectoryName}\\{inputFile.Name.Substring(0,inputFile.Name.Length - inputFile.Extension.Length)}-FinishedGameFilterOutput.csv";
        }

        /// <summary>
        /// The start point of the program, reads any file names present in args,
        /// parses the files they indicate, and writes new files with results
        /// from the FinishedGameFilter to a new file in the same folder
        /// with the same name with "-FinishedGameFilterOutput" appended.
        /// For demo and personal use purposes.
        /// </summary>
        /// <param name="args">The Grouvee export data file locations</param>
        public static void Main(string[] args)
        {
            foreach (string fileName in args)
            {
                // The FileInfo for the file
                var grouveeDataFile = new FileInfo(fileName);

                // A GrouveeDataFilterOutputter based on the FinishedGameFilter class
                var gdfo = new GrouveeDataFilterOutputter<FinishedGameFilter.FinishedGameFilterModel>(
                    new FinishedGameFilterOutputter(OutputFileName(grouveeDataFile)));

                // Parse the data in the file; filter, sort & map it, and finally output it to
                // a new file.
                gdfo.Run(grouveeDataFile);
            }
        }
    }
}
