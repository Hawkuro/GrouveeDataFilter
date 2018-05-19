using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrouveeDataFilter.Models;
using GrouveeDataFilter.Utils;

namespace GrouveeDataFilter.Filter_Templates
{
    public class IndexNameFilter : IFilterOutputterTemplate<string>
    {
        public bool Filter(GrouveeGame game)
        { // Don't filter anything out
            return true;
        }

        public int Comparer(GrouveeGame x, GrouveeGame y)
        { // Order by game's name
            return x.name.CompareTo(y.name);
        }

        public string Selector(GrouveeGame game, int index)
        { 
            return index + " " + game.name;
        }

        public void Outputter(IEnumerable<string> games)
        { // Write the index and name of every game to StdOut
            games.ForEach(Console.WriteLine);
        }
    }
}
