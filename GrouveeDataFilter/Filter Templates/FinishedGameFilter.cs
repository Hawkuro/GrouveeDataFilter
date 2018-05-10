using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrouveeDataFilter.Models;

namespace GrouveeDataFilter.Filter_Templates
{
    public class FinishedGameFilterStub : IFilterTemplate<FinishedGameFilterStub.FinishedGameFilterModel>
    {
        public class FinishedGameFilterModel
        {
            public string name;
            public DateTime finished_date;
            public int index;
            public int? MainStory;
            public int? MainStoryExtras;
            public int? HundredPercent;
        }

        public bool Filter(GrouveeGame game)
        {
            return game.dates.FirstOrDefault(d => d.date_finished != null) != null;
        }

        public int Comparer(GrouveeGame x, GrouveeGame y)
        {
            return x.dates.First(d => d.date_finished != null).date_finished.Value
                .CompareTo(y.dates.First(d => d.date_finished != null).date_finished.Value);
        }

        public FinishedGameFilterModel Selector(GrouveeGame game, int index)
        {
            var finishedDateObject = game.dates.First(d => d.date_finished != null);

            var model = new FinishedGameFilterModel
            {
                name = game.name,
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

    public class FinishedGameFilter : FinishedGameFilterStub,
        IFilterOutputterTemplate<FinishedGameFilterStub.FinishedGameFilterModel>
    {
        public void Outputter(IEnumerable<FinishedGameFilterModel> games)
        {
            
        }
    }
}
