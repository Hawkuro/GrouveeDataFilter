using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrouveeDataFilter.Models;

namespace GrouveeDataFilter.Filter_Templates
{
    public class FinishedGameFilterStub : IFilterTemplateStub<string>
    {
        public bool Filter(GrouveeGame game)
        {
            return game.dates.FirstOrDefault()?.date_finished != null;
        }

        public int Comparer(GrouveeGame x, GrouveeGame y)
        {
            throw new NotImplementedException();
        }

        public string Selector(GrouveeGame game, int index)
        {
            throw new NotImplementedException();
        }
    }
}
