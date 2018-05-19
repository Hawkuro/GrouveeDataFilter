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
    public class GrouveeLineParser
    {
        // The tail of the line being parsed, elements already parse having been removed
        private string tail;
        // The full line being parsed
        public readonly string gameString;

        /// <summary>
        /// Creates a new parse for the given line from Grouvee export .csv
        /// </summary>
        /// <param name="gameString">The content of the line</param>
        public GrouveeLineParser(string gameString)
        {
            this.gameString = gameString;
        }

        /// <summary>
        /// Parses the line
        /// </summary>
        /// <returns>A GrouveeGame object with all the data from the line</returns>
        public GrouveeGame ParseLine()
        {
            // Sets the tail to the full game line
            tail = gameString;
            var game = new GrouveeGame();

            // Each line here parses reads and parses each element of the game line.
            // When an element is read, it is removed from tail, so the subsequent element
            // will be at the front of the tail to be read next. Where an element ends
            // is detected with regex (thankfully internal json '"' are escaped as '""',
            // and '\""' inside json strings) in NextElement()
            game.id = int.Parse(NextElement());
            game.name = NextElement();
            game.shelves = ShelfParser(NextElement());
            game.platforms = NameUrlParser(NextElement());
            game.rating = NullableIntParser(NextElement());
            game.review = NextElement(); // For the love of all that is holy don't end these in '"', that case is a nightmare to handle, but feel free to try it if you're feeling brave. 
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

        /// <summary>
        /// Gets an element from the front of the tail, removes it and its following comma from the tail, and returns it
        /// </summary>
        /// <returns>The next element in a game line, or null if the element is empty</returns>
        private string NextElement()
        {
            // Probably superfluous as the last elemnt is giantbomb id which I think
            // is omnipresent, but better safe than sorry
            if (tail == "")
            {
                return null;
            }

            var firstLetter = tail.First();

            // The element is totally empty (happens when CSV has multiple commans in a row)
            if (firstLetter == ',')
            {
                tail = tail.Substring(1);
                return null;
            }


            int nextComma;
            string nextElement;
            // Handles elements in quotes by finding the next single '"' followed by a comma,
            // as all internal quotes are escaped as '""'
            if (firstLetter == '"')
            {
                // Finds the first single '"' followed by a comma
                // All Json objects have '""' on the inside, '"' outside.
                // Reviews are not json objects, but can also be quoted, with similar
                // escaping. This does not cover the case where reviews end in '"',
                // as it is difficult to handle, but feel free to try.
                // This would not work if the final column were a Json object,
                // but thankfully, it's an int
                var rx = new Regex(@"[^""]"",", RegexOptions.Compiled);

                // "{""json"": ""object""}",next element...
                //                       ^ ^
                //                       | |
                //      Regex matches here |
                //     But I want this index
                // Thus the '+2'
                nextComma = rx.Match(tail).Index + 2;

                // I guess you could account for empty strings here, but I don't think they're
                // possible, elements only appear to be quoted if they contain
                // commas (and maybe some other characters?), i.e. contain something

                // Strip surrounding '"' and replace '""' with '"' for parsing
                nextElement = tail.Substring(1, nextComma - 2).Replace("\"\"","\"");
            } else
            {
                // So the element is not surrounded by quotes, 
                // so we can safely assume the element does not contain any commas,
                // so the next comma is definitely the delimiter

                nextComma = tail.IndexOf(',');
                // end of line, only handed here as giantbomb id will never be quoted
                if (nextComma == -1) 
                {
                    nextElement = tail;
                    tail = "";
                    return nextElement;
                }

                nextElement = tail.Substring(0, nextComma);
            }

            // rid the tail of the element just read and its following comma
            tail = tail.Substring(nextComma + 1);

            return nextElement;
        }

        /// <summary>
        /// Parses an element that is a json string on the format
        /// {"Name1": {"url": "http://example1.com"}, "Name2": {"url": "http://example2.com"}}
        /// </summary>
        /// <param name="nameUrlString">The element as a string</param>
        /// <returns>A NameUrl object with the data from the string</returns>
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

        private IEnumerable<GrouveeGame.Shelf> ShelfParser(string nameUrlString)
        {
            if (string.IsNullOrEmpty(nameUrlString)) return new List<GrouveeGame.Shelf>();

            return JObject.Parse(nameUrlString).Properties()
                .Select(property => new GrouveeGame.Shelf
                {
                    name = property.Name,
                    url = new Uri(property.Value["url"].Value<string>()),
                    date_added = property.Value["date_added"].Value<DateTime>(),
                    order = property.Value["order"].Value<int>()
                });

        }

        /// <summary>
        /// Parses an integer in a string
        /// </summary>
        /// <param name="intString">The string to parse</param>
        /// <returns>An int with the int number in the string, null if string is empty, error if string is not an integer</returns>
        private static int? NullableIntParser(string intString)
        {
            if (string.IsNullOrEmpty(intString)) return null;
            return int.Parse(intString);
        }

        /// <summary>
        /// Parses a DateTime from a string, null if string is not a DateTime
        /// </summary>
        /// <param name="dateTimeString">The string to parse</param>
        /// <returns>A DateTime based on the string or null if the string does not contain a DateTime</returns>
        private static DateTime? NullableDateTimeParser(string dateTimeString)
        {
            if (string.IsNullOrEmpty(dateTimeString)) return null;

            DateTime ret;
            return DateTime.TryParse(dateTimeString, out ret) ? ret : (DateTime?) null;
        }

        /// <summary>
        /// Parse an array of date information
        /// </summary>
        /// <param name="dateDataString">The array as a json string</param>
        /// <returns>The date info as an IEnumerable[DateData]</returns>
        private static IEnumerable<GrouveeGame.DateData> DateDataParser(string dateDataString)
        {
            if (string.IsNullOrEmpty(dateDataString)) return new List<GrouveeGame.DateData>();

            return from JObject t in JArray.Parse(dateDataString)
                   select new GrouveeGame.DateData
                {
                    date_started = NullableDateTimeParser(t["date_started"].Value<string>()),
                    date_finished = NullableDateTimeParser(t["date_finished"].Value<string>()),
                    level_of_completion = GrouveeGame.ConvertLevelOfCompletion(t["level_of_completion"].Value<string>()),
                    seconds_played = t["seconds_played"].Value<int?>()
                };

        }

        /// <summary>
        /// Parses an aray of statuses
        /// </summary>
        /// <param name="statusString">The array as a json string</param>
        /// <returns>An IEnumerable containing the status data</returns>
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
