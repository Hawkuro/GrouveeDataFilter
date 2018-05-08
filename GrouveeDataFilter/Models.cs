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
        [FieldQuoted] //Shelf
        public string shelves;
        [FieldQuoted] //NameUrl
        public string platforms;
        public int? rating;
        public string review;
        [FieldQuoted] //DateData
        public string dates;
        [FieldQuoted]
        public string[] statuses;
        [FieldQuoted] //NameUrl
        public string[] genres;
        [FieldQuoted] //NameUrl
        public string[] franchises;
        [FieldQuoted] //NameUrl
        public string[] developers;
        [FieldQuoted] //NameUrl
        public string[] publishers;
        public DateTime release_date;
        public string url; //Uri
        public int giantbomb_id;

    }

}
