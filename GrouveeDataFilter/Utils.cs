using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FileHelpers;
using HtmlAgilityPack;

namespace GrouveeDataFilter.Utils
{
    public static class Tools
    {
        /// <summary>
        /// Helper function to call foreach in a linq style
        /// </summary>
        /// <typeparam name="T">The type of this IEnumerable</typeparam>
        /// <param name="source">this, an IEnumerable</param>
        /// <param name="action">The action to be called on each element in the IEnumerable</param>
        public static void ForEach<T>(
            this IEnumerable<T> source,
            Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }

        // From http://www.dreamincode.net/forums/topic/78080-two-way-dictionary/
        public static TKey GetKeyByValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value)
        {
            //this will return either the key, or a default value for TKey
            return dict.SingleOrDefault(x => x.Value.Equals(value)).Key;
        }

        /// <summary>
        /// Get a FileHelperEngine with the header generated from T's annotations
        /// </summary>
        /// <typeparam name="T">Class type annotated with FileHelpers annotations FieldAttribute and FieldOrder</typeparam>
        /// <returns>A FileHelperEngine for the type that will output a correct header</returns>
        public static FileHelperEngine<T> GetFileHelperEngine<T>() where T : class
        {
            return new FileHelperEngine<T>
            {
                HeaderText = typeof(T).GetCsvHeader()
            };
        }

        internal static string getImageURI(Uri gameUri)
        {
            return new HtmlWeb().Load(gameUri).DocumentNode.SelectNodes("//div[contains(@class, 'game-image')]/div/img").First().Attributes["src"].Value;
        }
    }

    // Copied from code presented as a solution on http://stackoverflow.com/questions/3975741/column-headers-in-csv-using-filehelpers-library/8258420#8258420 

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FieldTitleAttribute : Attribute
    {
        public FieldTitleAttribute(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            Name = name;
        }

        public string Name { get; private set; }
    }

    public static class FileHelpersTypeExtensions
    {
        public static IEnumerable<string> GetFieldTitles(this Type type)
        {
            var fields = from field in type.GetFields(
                BindingFlags.GetField |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance)
                         where field.IsFileHelpersField()
                         select field;

            return from field in fields
                   let attrs = field.GetCustomAttributes(true)
                   let order = attrs.OfType<FieldOrderAttribute>().Single().Order
                   let title = attrs.OfType<FieldTitleAttribute>().Single().Name
                   orderby order
                   select title;
        }

        public static string GetCsvHeader(this Type type)
        {
            return string.Join(",", type.GetFieldTitles());
        }

        static bool IsFileHelpersField(this FieldInfo field)
        {
            return field.GetCustomAttributes(true)
                .OfType<FieldOrderAttribute>()
                .Any();
        }
    }
}
