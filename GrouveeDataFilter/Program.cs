using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using GrouveeDataFilter.Models;

namespace GrouveeDataFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            //var grouveeDataFileName = args[0];
            var engine = new FileHelperEngine<GrouveeGame>(Encoding.UTF8);
            engine.ReadFile("C:\\Users\\haukuroskar\\Downloads\\Hawkuro_26904_grouvee_export.csv");
        }
    }
}
