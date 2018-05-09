using System;
using System.Collections.Generic;
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
                nextElement = WebUtility.HtmlDecode( tail.Substring(1, nextComma - 2).Replace("\"\"","\"") );
            } else
            {
                nextComma = tail.IndexOf(',');
                nextElement = tail.Substring(0, nextComma);
            }


            tail = tail.Substring(nextComma + 1);

            return nextElement;
        }

        private IEnumerable<GrouveeGame.NameUrl> NameUrlParser(string nameUrlString)
        {
            var NameUrls = new List<GrouveeGame.NameUrl>();
            if (string.IsNullOrEmpty(nameUrlString)) return NameUrls;

            var json = JObject.Parse(nameUrlString);
            foreach (var property in json.Properties().Select(p=>p.Name))
            {
                var NameUrl = new GrouveeGame.NameUrl();
                NameUrl.name = property;
                NameUrl.url = new Uri(((JObject) json.Property(property).Value).Property("url").Value.ToString());

                NameUrls.Add(NameUrl);
            }

            return NameUrls;
        }

        private int? NullableIntParser(string intString)
        {
            if (string.IsNullOrEmpty(intString)) return null;
            return int.Parse(intString);
        }
    }
}
