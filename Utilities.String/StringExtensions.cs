using System;
using JetBrains.Annotations;

namespace Utilities.String
{
    /// <summary>
    /// Contains extension methods for Strings.
    /// </summary>
    [UsedImplicitly]
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the string until specific character is encountered.
        /// </summary>
        /// <param name="str">String to cut</param>
        /// <param name="search">Character to look for</param>
        /// <returns><param name="str"/> cut before <param name="search"/>'s first occurence</returns>
        public static string Until([CanBeNull]this string str, char search)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var index = str.IndexOf(search);
            if (index == -1) return str;
            return str.Substring(0, index);
        }

        /// <summary>
        /// Returns the string until specific string is encountered
        /// </summary>
        /// <param name="str">String to cut</param>
        /// <param name="search">String to look for</param>
        /// <param name="stringComparison">(optional) string comparison method</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Search string must not be null.</exception>
        public static string Until([CanBeNull]this string str, [NotNull]string search, StringComparison stringComparison = StringComparison.Ordinal)
        {
            if(search == null) throw new ArgumentException("Search string must not be null.", nameof(search));
            if (string.IsNullOrEmpty(str)) return str;
            var index = str.IndexOf(search, stringComparison);
            if (index == -1) return str;
            return str.Substring(0, index);
        }
    }
}