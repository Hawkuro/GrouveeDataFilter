using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrouveeDataFilter.Models;

namespace GrouveeDataFilter
{
    public class GrouveeDataFilter<S>
    {
        public readonly Func<GrouveeGame, bool> filter;
        public readonly IComparer<GrouveeGame> comparer;
        public readonly Func<GrouveeGame, int, S> selector;

        public GrouveeDataFilter(IFilterTemplate<S> template) : this(template.Filter, template.Comparer, template.Selector) { }

        public GrouveeDataFilter(Func<GrouveeGame, bool> filter,
            Comparison<GrouveeGame> comparer, Func<GrouveeGame, int, S> selector)
        {
            this.filter = filter;
            this.comparer = Comparer<GrouveeGame>.Create(comparer);
            this.selector = selector;
        }

        public static IEnumerable<GrouveeGame> ParseCSV(FileInfo inputFile)
        {
            var gameList = new List<GrouveeGame>();

            using (var fileContent = File.OpenText(inputFile.FullName))
            {
                fileContent.ReadLine(); // Skip the first line

                while (!fileContent.EndOfStream)
                {
                    gameList.Add(new Parser(fileContent.ReadLine()).ParseLine());
                }
            }
            return gameList;
        }

        public IEnumerable<S> GetGamesData(string fileUri)
        {
            return GetGamesData(new FileInfo(fileUri));
        }

        public IEnumerable<S> GetGamesData(FileInfo inputFile)
        {
            return GetGamesData(ParseCSV(inputFile));
        }

        public IEnumerable<S> GetGamesData(IEnumerable<GrouveeGame> games)
        {
            return games.Where(filter).OrderBy(g => g, comparer).Select(selector);
        }

    }

    public class GrouveeDataFilterOutputter<S> : GrouveeDataFilter<S>
    {
        public readonly Action<IEnumerable<S>> outputter;

        public GrouveeDataFilterOutputter(Func<GrouveeGame, bool> filter,
            Comparison<GrouveeGame> comparer, Func<GrouveeGame, int, S> selector, Action<IEnumerable<S>> outputter) :
            base(filter, comparer, selector)
        {
            this.outputter = outputter;
        }

        public GrouveeDataFilterOutputter(IFilterOutputterTemplate<S> template) : base(template)
        {
            outputter = template.Outputter;
        }

        public void Run(string fileUri)
        {
            Run(new FileInfo(fileUri));
        }

        public void Run(FileInfo inputFile)
        {
            Run(ParseCSV(inputFile));
        }

        public void Run(IEnumerable<GrouveeGame> games)
        {
            outputter(GetGamesData(games));
        }
    }
}
