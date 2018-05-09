using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using GrouveeDataFilter.Models;

namespace GrouveeDataFilter
{
    public class Program
    {
        public static void Run<S>(GrouveeGame[] games, Func<GrouveeGame,bool> filter,
            IComparer<GrouveeGame> comparer, Func<GrouveeGame, S> selector, Action<IEnumerable<S>> outputter)
        {
            outputter(games.Where(filter).OrderBy(g => g, comparer).Select(selector));
        }

        public static void Main(string[] args)
        {

            //var grouveeDataFileName = args[0];
            var grouveeDataFileName = "C:\\Users\\haukuroskar\\Downloads\\Hawkuro_26904_grouvee_export.csv";

            // TODO: Parse the data

            // TODO: Write filter, comparer, selector, outputter
            Run(new GrouveeGame[] { }, g => true, Comparer<GrouveeGame>.Default, g => g, gs => Console.WriteLine(gs));

        }
    }
}
