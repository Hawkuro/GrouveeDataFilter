using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrouveeDataFilter.Models;

namespace GrouveeDataFilter
{
    public interface IFilterTemplateStub<S>
    {
        bool Filter(GrouveeGame game);
        int Comparer(GrouveeGame x, GrouveeGame y);
        S Selector(GrouveeGame game, int index);
    }

    public interface IFilterTemplate<S> : IFilterTemplateStub<S>
    {
        void Outputter(IEnumerable<S> games); // Separated mostly to be able to plug and unplug e.g. stdOut and write to file
    }
}
