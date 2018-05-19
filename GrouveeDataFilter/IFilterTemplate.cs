using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrouveeDataFilter.Models;

namespace GrouveeDataFilter
{
    /// <summary>
    /// Implement this to use (and write) a class instead of
    /// a set of delegates for a GrouveeDataFilter
    /// </summary>
    /// <typeparam name="S">The type of data to be returned from the GrouveeDataFilter</typeparam>
    public interface IFilterTemplate<S>
    {
        /// <summary>
        /// The filter to use
        /// </summary>
        /// <param name="game">The game data</param>
        /// <returns>False if game should be filtered out, otherwise true</returns>
        bool Filter(GrouveeGame game);

        /// <summary>
        /// A comparer to sort by
        /// </summary>
        /// <param name="x">The first game to compare</param>
        /// <param name="y">The second game to compare</param>
        /// <returns>
        /// If x &lt; y, an int &lt; 0
        /// If x = y, then 0
        /// If x &gt; y, an int &gt; 0
        /// </returns>
        int Comparer(GrouveeGame x, GrouveeGame y);

        /// <summary>
        /// A selector to select and return only the data you want
        /// </summary>
        /// <param name="game">The game's raw parsed data</param>
        /// <param name="index">The game's position in the ordering</param>
        /// <returns>The game data you want</returns>
        S Selector(GrouveeGame game, int index);
    }

    /// <summary>
    /// An extension of IFilterTemplate for use with GrouveeDataFilterOutputter
    /// </summary>
    /// <typeparam name="S">The type of data the outputter recieves</typeparam>
    public interface IFilterOutputterTemplate<S> : IFilterTemplate<S>
    {
        /// <summary>
        /// An outputter that outputs the data in some way
        /// </summary>
        /// <param name="games">The list of games</param>
        void Outputter(IEnumerable<S> games); // Separated mostly to be able to plug and unplug e.g. stdOut and write to file
    }
}
