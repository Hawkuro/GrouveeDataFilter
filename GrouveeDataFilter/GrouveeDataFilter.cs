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
        private Func<GrouveeGame, bool> filter;
        private IComparer<GrouveeGame> comparer;
        private Func<GrouveeGame, int, S> selector;
        private Action<IEnumerable<S>> outputter;

        public GrouveeDataFilter(IFilterTemplate<S> template) : this(template.Filter, template.Comparer, template.Selector, template.Outputter) { }

        public GrouveeDataFilter(Func<GrouveeGame, bool> filter,
            Comparison<GrouveeGame> comparer, Func<GrouveeGame, int, S> selector, Action<IEnumerable<S>> outputter)
        {
            this.filter = filter;
            this.comparer = Comparer<GrouveeGame>.Create(comparer);
            this.selector = selector;
            this.outputter = outputter;
        }

        public static IEnumerable<GrouveeGame> ParseCSV(FileInfo inputFile)
        {
            var gameList = new List<GrouveeGame>();

            using (var fileContent = File.OpenText(inputFile.FullName))
            {
                fileContent.ReadLine(); // Skip the first line

                while (!fileContent.EndOfStream)
                {
                    var newGame = new GrouveeGame {name = fileContent.ReadLine()};
                    //TODO: actually parse
                    gameList.Add(newGame);
                }
            }
            return gameList;
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
            outputter(games.Where(filter).OrderBy(g => g, comparer).Select(selector));
        }

    }
}
