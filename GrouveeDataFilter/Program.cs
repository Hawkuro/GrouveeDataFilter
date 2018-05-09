﻿using System;
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

        public static void Main(string[] args)
        {

            //var grouveeDataFileName = args[0];
            var grouveeDataFileName = "C:\\Users\\haukuroskar\\Downloads\\Hawkuro_26904_grouvee_export.csv";

            // TODO: Write filter, comparer, selector, outputter
            var gdf = new GrouveeDataFilter<string>(
                g => true,
                (g1,g2)=>g1.name.CompareTo(g2.name),
                g => g.name,
                gs => gs.Select((g,i) => i+" "+g).ForEach(g => Console.WriteLine(g))
                );

            gdf.Run(grouveeDataFileName);

        }
    }
}
