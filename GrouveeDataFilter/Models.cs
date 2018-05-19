using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using GrouveeDataFilter.Utils;

namespace GrouveeDataFilter.Models
{
    public class GrouveeGame
    {
        /// <summary>
        /// A status update and its metadata
        /// </summary>
        public class Status
        {
            public string status;
            public DateTime date;
            public Uri url;
        }

        /// <summary>
        /// An enurable enumrating the Level of Completion of a given game
        /// </summary>
        public enum LevelOfCompletion
        {
            None,
            MainStory,
            MainStoryExtras,
            HundredPercent
        }

        /// <summary>
        /// Maps Level of completion as a string to the enum, not including None
        /// </summary>
        private static readonly Dictionary<string, LevelOfCompletion> LevelOfCompletionConverter =
            new Dictionary<string, LevelOfCompletion>
            {
                {"Main Story", LevelOfCompletion.MainStory},
                {"Main Story + Extras", LevelOfCompletion.MainStoryExtras},
                {"100% Completion", LevelOfCompletion.HundredPercent}
            };

        public static string ConvertLevelOfCompletion(LevelOfCompletion loc)
        {
            return LevelOfCompletionConverter.GetKeyByValue(loc);
        }

        /// <summary>
        /// Get a LevelOfCompletion enum based on the level of completion string
        /// </summary>
        /// <param name="locString">The string to parse</param>
        /// <returns>The level of completion, as an enum. None if unknown.</returns>
        public static LevelOfCompletion ConvertLevelOfCompletion(string loc)
        {
            LevelOfCompletion ret;
            return LevelOfCompletionConverter.TryGetValue(loc, out ret) ? ret : LevelOfCompletion.None;
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
