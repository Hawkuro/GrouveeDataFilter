using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrouveeDataFilter.Models;

namespace GrouveeDataFilter
{
    /// <summary>
    /// An object that parses a Grouvee export csv, filters it,
    /// sorts it and returns the results of a selection
    /// </summary>
    /// <typeparam name="S">The type of returned data</typeparam>
    public class GrouveeDataFilter<S>
    {
        // Names speak for themselves
        public readonly Func<GrouveeGame, bool> filter;
        public readonly IComparer<GrouveeGame> comparer;
        public readonly Func<GrouveeGame, int, S> selector;

        /// <summary>
        /// Creates a GrouveeDataFilter based on a template
        /// </summary>
        /// <param name="template">An instance fof the template type</param>
        public GrouveeDataFilter(IFilterTemplate<S> template) : this(template.Filter, template.Comparer, template.Selector) { }

        /// <summary>
        /// Creates a GrouveeDataFilter using delegates
        /// </summary>
        /// <param name="filter">Should return false for games that should be filtered out</param>
        /// <param name="comparer">Used for sorting</param>
        /// <param name="selector">Used to select final output of GetGamesData</param>
        public GrouveeDataFilter(Func<GrouveeGame, bool> filter,
            Comparison<GrouveeGame> comparer, Func<GrouveeGame, int, S> selector)
        {
            this.filter = filter;
            this.comparer = Comparer<GrouveeGame>.Create(comparer);
            this.selector = selector;
        }

        /// <summary>
        /// Helper function to parse the entire csv indicated by the FileInfo
        /// Can also be used without the rest if you just want the parsed data
        /// </summary>
        /// <param name="inputFile">The Grouvee export csv to parse</param>
        /// <returns>A list of GrouveeGame game data as parsed from the file</returns>
        public static IEnumerable<GrouveeGame> ParseCSV(FileInfo inputFile)
        {
            var gameList = new List<GrouveeGame>();

            using (var fileContent = File.OpenText(inputFile.FullName))
            {
                fileContent.ReadLine(); // Skip the first line

                while (!fileContent.EndOfStream)
                { // Parse each line
                    gameList.Add(new GrouveeLineParser(fileContent.ReadLine()).ParseLine());
                }
            }
            return gameList;
        }

        /// <summary>
        /// Runs the game data through the filter, sorts it by comparer and selects
        /// data using selector, and returns it.
        /// </summary>
        /// <param name="fileUri">A string file uri to the file containing the game data</param>
        /// <returns>See description</returns>
        public IEnumerable<S> GetGamesData(string fileUri)
        {
            return GetGamesData(new FileInfo(fileUri));
        }

        /// <summary>
        /// Runs the game data through the filter, sorts it by comparer and selects
        /// data using selector, and returns it.
        /// </summary>
        /// <param name="inputFile">The csv file containing the game data</param>
        /// <returns>See description</returns>
        public IEnumerable<S> GetGamesData(FileInfo inputFile)
        {
            return GetGamesData(ParseCSV(inputFile));
        }

        /// <summary>
        /// Runs the game data through the filter, sorts it by comparer and selects
        /// data using selector, and returns it.
        /// </summary>
        /// <param name="games">The list of parsed games</param>
        /// <returns>See description</returns>
        public IEnumerable<S> GetGamesData(IEnumerable<GrouveeGame> games)
        {
            return games.Where(filter).OrderBy(g => g, comparer).Select(selector);
        }

    }

    public class GrouveeDataFilterOutputter<S> : GrouveeDataFilter<S>
    {
        // Takes the returned data from GetGamesData and outputs it in some manner
        public readonly Action<IEnumerable<S>> outputter;

        /// <summary>
        /// Creates a GrouveeDataFilterOutputter based on delegates
        /// </summary>
        /// <param name="filter">Should return false for games that should be filtered out</param>
        /// <param name="comparer">Used for sorting</param>
        /// <param name="selector">Used to select final output of GetGamesData</param>
        /// <param name="outputter">Takes the data returned from the selector and outputs it in some manner</param>
        public GrouveeDataFilterOutputter(Func<GrouveeGame, bool> filter,
            Comparison<GrouveeGame> comparer, Func<GrouveeGame, int, S> selector, Action<IEnumerable<S>> outputter) :
            base(filter, comparer, selector)
        {
            this.outputter = outputter;
        }

        /// <summary>
        /// Creates a GrouveeDataFilterOutputter based on a template
        /// </summary>
        /// <param name="template">An instance of the template to base the GrouveeDataFilterOutputter on</param>
        public GrouveeDataFilterOutputter(IFilterOutputterTemplate<S> template) : base(template)
        {
            outputter = template.Outputter;
        }

        /// <summary>
        /// Runs the game data through the filter, sorts it by comparer and selects
        /// data using selector, and outputs it using outputter. 
        /// </summary>
        /// <param name="fileUri">A string file uri to the file containing the game data</param>
        public void Run(string fileUri)
        {
            Run(new FileInfo(fileUri));
        }

        /// <summary>
        /// Runs the game data through the filter, sorts it by comparer and selects
        /// data using selector, and outputs it using outputter.
        /// </summary>
        /// <param name="inputFile">The csv file containing the game data</param>
        /// <returns>See description</returns>
        public void Run(FileInfo inputFile)
        {
            Run(ParseCSV(inputFile));
        }

        /// <summary>
        /// Runs the game data through the filter, sorts it by comparer and selects
        /// data using selector, and outputs it using outputter.
        /// </summary>
        /// <param name="games">The list of parsed games</param>
        /// <returns>See description</returns>
        public void Run(IEnumerable<GrouveeGame> games)
        {
            outputter(GetGamesData(games));
        }
    }
}
