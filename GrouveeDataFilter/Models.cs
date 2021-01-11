using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Get a LevelOfCompletion string based on the level of completion enum
        /// </summary>
        /// <param name="loc">The LevelOfCompletion enum</param>
        /// <returns>The level of completion, as a string. null if None.</returns>
        public static string ConvertLevelOfCompletion(LevelOfCompletion loc)
        {
            return LevelOfCompletionConverter.GetKeyByValue(loc);
        }

        /// <summary>
        /// Get a LevelOfCompletion enum based on the level of completion string
        /// </summary>
        /// <param name="loc">The string to parse</param>
        /// <returns>The level of completion, as an enum. None if unknown.</returns>
        public static LevelOfCompletion ConvertLevelOfCompletion(string loc)
        {
            LevelOfCompletion ret;
            return LevelOfCompletionConverter.TryGetValue(loc??"", out ret) ? ret : LevelOfCompletion.None;
        }

        /// <summary>
        /// A model containing a game's date data
        /// </summary>
        public class DateData
        {
            public DateTime? date_finished;
            public DateTime? date_started;
            public LevelOfCompletion level_of_completion;
            public int? seconds_played;
        }

        /// <summary>
        /// A model for a name->url mapping, used for e.g. publishers and developers
        /// </summary>
        public class NameUrl
        {
            public string name;
            public Uri url;
        }

        /// <summary>
        /// A model for a grouvee shelf, inherits NameUrl because its data is a superset of NameUrl
        /// </summary>
        public class Shelf : NameUrl
        {
            public DateTime date_added;
            public int order;
        }

        // The various fields in the csv
        public int id;
        public string name;
        public IEnumerable<Shelf> shelves;
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
