using System;
using System.Collections.Generic;
using System.Linq;
using FileHelpers;
using GrouveeDataFilter.Models;
using GrouveeDataFilter.Utils;

namespace GrouveeDataFilter.Filter_Templates
{
    public class FinishedGameFilter : IFilterTemplate<FinishedGameFilter.FinishedGameFilterModel>
    {
        [DelimitedRecord(","), IgnoreFirst(1)]
        public class FinishedGameFilterModel
        {
            [FieldOrder(1), FieldTitle("Name")]
            public string name;
            [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
            [FieldOrder(2), FieldTitle("Started date")]
            public DateTime? started_date;
            [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
            [FieldOrder(3), FieldTitle("Finished date")]
            public DateTime? finished_date;
            [FieldOrder(4), FieldTitle("Games Completed")]
            public int index;
            [FieldOrder(5), FieldTitle("Main Story")]
            public int? MainStory;
            [FieldOrder(6), FieldTitle("Main Story + Extras")]
            public int? MainStoryExtras;
            [FieldOrder(7), FieldTitle("100% Completion")]
            public int? HundredPercent;
        }

        public bool Filter(GrouveeGame game)
        { // Takes only games that have a finish date
            return game.dates.FirstOrDefault(d => d.date_finished != null) != null;
        }

        public int Comparer(GrouveeGame x, GrouveeGame y)
        { // Compares by start date
            return x.dates.First(d => d.date_finished != null).date_finished.Value
                .CompareTo(y.dates.First(d => d.date_finished != null).date_finished.Value);
        }

        public FinishedGameFilterModel Selector(GrouveeGame game, int index)
        {
            var finishedDateObject = game.dates.First(d => d.date_finished != null);

            var model = new FinishedGameFilterModel
            {
                name = game.name,
                started_date = finishedDateObject.date_started.Value,
                finished_date = finishedDateObject.date_finished.Value,
                index = index + 1
            };
            model.MainStory = finishedDateObject.level_of_completion == GrouveeGame.LevelOfCompletion.MainStory
                ? model.index
                : (int?)null;
            model.MainStoryExtras = finishedDateObject.level_of_completion == GrouveeGame.LevelOfCompletion.MainStoryExtras
                ? model.index
                : (int?)null;
            model.HundredPercent = finishedDateObject.level_of_completion == GrouveeGame.LevelOfCompletion.HundredPercent
                ? model.index
                : (int?)null;

            return model;
        }
    }

    public class FinishedGameFilterOutputter : FinishedGameFilter,
        IFilterOutputterTemplate<FinishedGameFilter.FinishedGameFilterModel>
    {
        // The file to save the output to
        public readonly string outputFileName;

        public FinishedGameFilterOutputter(string outputFileName)
        {
            this.outputFileName = outputFileName;
        }

        public void Outputter(IEnumerable<FinishedGameFilterModel> games)
        { // Writes to csv file
            Tools.GetFileHelperEngine<FinishedGameFilterModel>().WriteFile(outputFileName, games);
        }

    }
}
