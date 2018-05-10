using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrouveeDataFilter.Models;

namespace GrouveeDataFilter.Filter_Templates
{
    public class IndexNameFilter : IFilterOutputterTemplate<string>
    {
        public bool Filter(GrouveeGame game)
        {
            return true;
        }

        public int Comparer(GrouveeGame x, GrouveeGame y)
        {
            return x.name.CompareTo(y.name);
        }

        public string Selector(GrouveeGame game, int index)
        {
            return index + " " + game.name;
        }

        public void Outputter(IEnumerable<string> games)
        {
            games.ForEach(Console.WriteLine);
        }
    }
}
