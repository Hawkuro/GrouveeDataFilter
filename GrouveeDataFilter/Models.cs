using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace GrouveeDataFilter.Models
{
    [DelimitedRecord(","), IgnoreFirst]
    public class GrouveeGame
    {
        public class Status
        {
            public string status;
            public DateTime date;
            public Uri url;
        }

        public enum LevelOfCompletion
        {
            [Description("Main Story")]
            MainStory,
            [Description("Main Story + Extras")]
            MainStoryExtras,
            [Description("100% Completion")]
            HundredPercent
        }

        public class DateData
        {
            public DateTime? date_finished;
            public DateTime? date_started;
            public LevelOfCompletion level_of_completion;
            public int seconds_played;
        }
        public class NameUrl
        {
            public string name;
            public Uri url;
        }
        public class Shelf : NameUrl
        {
            public DateTime date_added;
            public int order;
        }

        public int id;
        public string name;
        public NameUrl shelves;
        public NameUrl platforms;
        public int? rating;
        public string review;
        public DateData dates;
        public Status[] statuses;
        public NameUrl[] genres;
        public NameUrl[] franchises;
        public NameUrl[] developers;
        public NameUrl[] publishers;
        public DateTime release_date;
        public Uri url; 
        public int giantbomb_id;

    }

}
