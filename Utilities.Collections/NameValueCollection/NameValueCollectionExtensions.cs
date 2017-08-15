using System;
using JetBrains.Annotations;

namespace Utilities.Collections.NameValueCollection
{
    public static class NameValueCollectionExtensions
    {
        /// <exception cref="FormatException">Setting cannot be read as integer and <paramref name="suppressException"/> is false.</exception>
        [PublicAPI]
        public static int GetInt(this System.Collections.Specialized.NameValueCollection source, string key,
            bool suppressException = false) => 
            int.TryParse(source[key], out int result) || suppressException
            ? result
            : throw new FormatException($"The entry {key} does not have correct integer value: {source[key]}");

        [PublicAPI]
        public static int GetInt(this System.Collections.Specialized.NameValueCollection source, string key,
            int defaultValue) => int.TryParse(source[key], out int result) ? result : defaultValue;

        [PublicAPI]
        public static bool GetBool(this System.Collections.Specialized.NameValueCollection source, string key,
            bool suppressException = false) =>
            bool.TryParse(source[key], out bool result) || suppressException
                ? result
                : throw new FormatException($"The entry {key} does not have correct boolean value: {source[key]}");

        [PublicAPI]
        public static bool GetBoolOrDefault(this System.Collections.Specialized.NameValueCollection source, string key,
            bool defaultValue)
            => bool.TryParse(source[key], out bool result) ? result : defaultValue;

        /// <exception cref="FormatException">Entry <paramref name="key"/> cannot be read as DateTime and <paramref name="suppressException"/> is false.</exception>
        [PublicAPI]
        public static DateTime GetDateTime(this System.Collections.Specialized.NameValueCollection source, string key,
            bool suppressException = false) => 
            DateTime.TryParse(source[key], out DateTime result) || suppressException
            ? result
            : throw new FormatException($"The entry {key} does not have correct DateTime value: {source[key]}");

        [PublicAPI]
        public static DateTime? GetDateTimeOrNull(this System.Collections.Specialized.NameValueCollection source,
            string key) => 
            DateTime.TryParse(source[key], out DateTime result)
            ? (DateTime?)result 
            : null;

        /// <exception cref="ArgumentException">Entry <paramref name="key"/> is missing.</exception>
        [PublicAPI]
        public static string[] GetFromCSV(this System.Collections.Specialized.NameValueCollection source, string key,
    bool suppressException = false)
        {
            var value = source[key];

            if (value != null) return value.Split(',');

            if (suppressException)
                return null;

            throw new ArgumentException($"The entry {key} is missing or empty");
        }
    }
}