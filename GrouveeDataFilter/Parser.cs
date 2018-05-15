using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GrouveeDataFilter.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GrouveeDataFilter
{
    public class Parser
    {
        private string tail;
        public readonly string gameString;

        public Parser(string gameString)
        {
            this.gameString = gameString;
        }

        public GrouveeGame ParseLine()
        {
            tail = gameString;
            var game = new GrouveeGame();

            game.id = int.Parse(NextElement());
            game.name = NextElement();
            game.shelves = NameUrlParser(NextElement());
            game.platforms = NameUrlParser(NextElement());
            game.rating = NullableIntParser(NextElement());
            game.review = NextElement();
            game.dates = DateDataParser(NextElement());
            game.statuses = StatusParser(NextElement());
            game.genres = NameUrlParser(NextElement());
            game.franchises = NameUrlParser(NextElement());
            game.developers = NameUrlParser(NextElement());
            game.publishers = NameUrlParser(NextElement());
            game.release_date = NullableDateTimeParser(NextElement());
            game.url = new Uri(NextElement());
            game.giantbomb_id = NullableIntParser(NextElement());

            return game;
        }

        private string NextElement()
        {
            if (tail == "")
            {
                return null;
            }

            var firstLetter = tail.First();

            if (firstLetter == ',')
            {
                tail = tail.Substring(1);
                return null;
            }


            int nextComma;
            string nextElement;
            if (firstLetter == '"')
            {
                // Finds the first single double quote followed by a comma
                // All Json objects have double quotes on the inside, single quotes outside
                // This would not work if the final column were a Json object,
                // but thankfully, it's an int
                var rx = new Regex(@"[^""]"",", RegexOptions.Compiled);

                nextComma = rx.Match(tail).Index + 2;

                // I guess you could account for empty strings here, but I don't think they're possible
                // Replace double double quotes with single double quotes for Json parsing
                nextElement = tail.Substring(1, nextComma - 2).Replace("\"\"","\"");
            } else
            {
                nextComma = tail.IndexOf(',');
                if (nextComma == -1) // end of line
                {
                    nextElement = tail;
                    tail = "";
                    return nextElement;
                }

                nextElement = tail.Substring(0, nextComma);
            }


            tail = tail.Substring(nextComma + 1);

            return nextElement;
        }

        private IEnumerable<GrouveeGame.NameUrl> NameUrlParser(string nameUrlString)
        {
            if (string.IsNullOrEmpty(nameUrlString)) return new List<GrouveeGame.NameUrl>();

            return JObject.Parse(nameUrlString).Properties()
                .Select(property => new GrouveeGame.NameUrl
                {
                    name = property.Name,
                    url = new Uri(property.Value["url"].Value<string>())
                });

        }

        private static int? NullableIntParser(string intString)
        {
            if (string.IsNullOrEmpty(intString)) return null;
            return int.Parse(intString);
        }

        private static DateTime? NullableDateTimeParser(string dateTimeString)
        {
            if (string.IsNullOrEmpty(dateTimeString)) return null;

            DateTime ret;
            return DateTime.TryParse(dateTimeString, out ret) ? ret : (DateTime?) null;
        }

        private static GrouveeGame.LevelOfCompletion LevelOfCompletionParser(string locString)
        {
            var dict = new Dictionary<string, GrouveeGame.LevelOfCompletion>
            {
                {"Main Story", GrouveeGame.LevelOfCompletion.MainStory},
                {"Main Story + Extras", GrouveeGame.LevelOfCompletion.MainStoryExtras },
                {"100% Completion", GrouveeGame.LevelOfCompletion.HundredPercent }
            };
            GrouveeGame.LevelOfCompletion ret;
            return dict.TryGetValue(locString, out ret) ? ret : GrouveeGame.LevelOfCompletion.None;
        }

        private static IEnumerable<GrouveeGame.DateData> DateDataParser(string dateDataString)
        {
            if (string.IsNullOrEmpty(dateDataString)) return new List<GrouveeGame.DateData>();

            return from JObject t in JArray.Parse(dateDataString)
                   select new GrouveeGame.DateData
                {
                    date_started = NullableDateTimeParser(t["date_started"].Value<string>()),
                    date_finished = NullableDateTimeParser(t["date_finished"].Value<string>()),
                    level_of_completion = LevelOfCompletionParser(t["level_of_completion"].Value<string>()),
                    seconds_played = t["seconds_played"].Value<int?>()
                };

        }

        private static IEnumerable<GrouveeGame.Status> StatusParser(string statusString)
        {
            if (string.IsNullOrEmpty(statusString)) return new List<GrouveeGame.Status>();

            return from JObject t in JArray.Parse(statusString)
                   select new GrouveeGame.Status
                {
                    status = t["status"].Value<string>(),
                    date = DateTime.Parse(t["date"].Value<string>()),
                    url = new Uri(t["url"].Value<string>())
                };
        }
    }
}
