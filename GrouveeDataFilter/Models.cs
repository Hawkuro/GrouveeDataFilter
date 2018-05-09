using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace GrouveeDataFilter.Models
{
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
            None,
            MainStory,
            MainStoryExtras,
            HundredPercent
        }

        public class DateData
        {
            public DateTime? date_finished;
            public DateTime? date_started;
            public LevelOfCompletion level_of_completion;
            public int? seconds_played;
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
        public IEnumerable<NameUrl> shelves;
        public IEnumerable<NameUrl>  platforms;
        public int? rating;
        public string review;
        public IEnumerable<DateData>  dates;
        public IEnumerable<Status>  statuses;
        public IEnumerable<NameUrl>  genres;
        public IEnumerable<NameUrl>  franchises;
        public IEnumerable<NameUrl>  developers;
        public IEnumerable<NameUrl>  publishers;
        public DateTime? release_date;
        public Uri url; 
        public int? giantbomb_id;
    }
}
