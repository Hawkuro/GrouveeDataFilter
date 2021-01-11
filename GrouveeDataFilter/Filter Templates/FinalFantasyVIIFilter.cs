using System;
using System.Collections.Generic;
using System.Linq;
using FileHelpers;
using GrouveeDataFilter.Models;
using GrouveeDataFilter.Utils;
using HtmlAgilityPack;

namespace GrouveeDataFilter.Filter_Templates
{
    public class FinalFantasyVIIFilter : IFilterTemplate<IEnumerable<FinalFantasyVIIFilter.FinalFantasyVIIFilterModel>>
    {
        [DelimitedRecord(","), IgnoreFirst(1)]
        public class FinalFantasyVIIFilterModel
        {
            [FieldOrder(3), FieldTitle("Name")]
            public string name;
            [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
            [FieldOrder(1), FieldTitle("Started date")]
            public DateTime started_date;
            [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
            [FieldOrder(2), FieldTitle("Finished date")]
            public DateTime finished_date;
            [FieldOrder(4), FieldTitle("Image")]
            public string image;
        }

        public bool Filter(GrouveeGame game)
        { // Takes only games that are in the FFVII series
            return game.name.Contains("Final Fantasy VII");
        }

        public int Comparer(GrouveeGame x, GrouveeGame y)
        { // Compares by start date
            return x.dates.First(d => d.date_finished != null).date_finished.Value
                .CompareTo(y.dates.First(d => d.date_finished != null).date_finished.Value);
        }

        public IEnumerable<FinalFantasyVIIFilterModel> Selector(GrouveeGame game, int index)
        {
            var imageUri = Tools.getImageURI(game.url);

            return game.dates.Select(d => new FinalFantasyVIIFilterModel
            {
                name = game.name,
                started_date = d.date_started.Value,
                finished_date = d.date_finished.Value,
                image = imageUri
            });
        }
    }

    public class FinalFantasyVIIFilterOutputter : FinalFantasyVIIFilter,
        IFilterOutputterTemplate<IEnumerable<FinalFantasyVIIFilter.FinalFantasyVIIFilterModel>>
    {
        // The file to save the output to
        public readonly string outputFileName;

        public FinalFantasyVIIFilterOutputter(string outputFileName)
        {
            this.outputFileName = outputFileName;
        }

        public void Outputter(IEnumerable<IEnumerable<FinalFantasyVIIFilterModel>> games)
        { // Writes to csv file
            Tools.GetFileHelperEngine<FinalFantasyVIIFilterModel>().WriteFile(outputFileName, games.SelectMany(g=>g));
        }

    }
}
