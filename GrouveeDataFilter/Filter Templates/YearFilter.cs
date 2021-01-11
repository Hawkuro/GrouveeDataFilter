using System;
using System.Collections.Generic;
using System.Linq;
using FileHelpers;
using GrouveeDataFilter.Models;
using GrouveeDataFilter.Utils;
using HtmlAgilityPack;

namespace GrouveeDataFilter.Filter_Templates
{
    public class YearFilter : IFilterTemplate<IEnumerable<YearFilter.YearFilterModel>>
    {
        [DelimitedRecord(","), IgnoreFirst(1)]
        public class YearFilterModel
        {
            [FieldOrder(3), FieldTitle("Name")]
            public string name;
            [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
            [FieldOrder(1), FieldTitle("Started date")]
            public DateTime? started_date;
            [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
            [FieldOrder(2), FieldTitle("Finished date")]
            public DateTime? finished_date;
            [FieldOrder(4), FieldTitle("Image")]
            public string image;
        }

        private int _year;

        public YearFilter(int year)
        {
            _year = year;
        }

        public bool Filter(GrouveeGame game)
        { // Takes only games that were played on the given year
            return game.dates.Any(d=>d.date_started?.Year == _year || d.date_finished?.Year == _year);
        }

        public int Comparer(GrouveeGame x, GrouveeGame y)
        { // Compares by start date
            return String.Compare(x.name, y.name, StringComparison.InvariantCulture);
        }

        public IEnumerable<YearFilterModel> Selector(GrouveeGame game, int index)
        {
            var imageUri = Tools.getImageURI(game.url);

            return game.dates.Select(d => new YearFilterModel
            {
                name = game.name,
                started_date = d.date_started,
                finished_date = d.date_finished,
                image = imageUri
            });
        }
    }

    public class YearFilterOutputter : YearFilter,
        IFilterOutputterTemplate<IEnumerable<YearFilter.YearFilterModel>>
    {
        // The file to save the output to
        public readonly string outputFileName;

        public YearFilterOutputter(int year, string outputFileName) : base(year)
        {
            this.outputFileName = outputFileName;
        }

        public void Outputter(IEnumerable<IEnumerable<YearFilterModel>> games)
        { // Writes to csv file
            Tools.GetFileHelperEngine<YearFilterModel>().WriteFile(outputFileName, games.SelectMany(g=>g));
        }

    }
}
