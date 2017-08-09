using System;
using System.Collections.Generic;
using System.Linq;
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
        [PublicAPI]
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
        [PublicAPI]
        public static string Until([CanBeNull]this string str, [NotNull]string search, StringComparison stringComparison = StringComparison.Ordinal)
        {
            if(search == null) throw new ArgumentException("Search string must not be null.", nameof(search));
            if (string.IsNullOrEmpty(str)) return str;
            var index = str.IndexOf(search, stringComparison);
            if (index == -1) return str;
            return str.Substring(0, index);
        }

        [NotNull]
        [PublicAPI]
        public static string ToCSV([CanBeNull] this string[] stringArray, bool removeEmpty = false)
        {
            if (stringArray == null) return string.Empty;
            return string.Join(",", removeEmpty ? stringArray.Where(str => !string.IsNullOrEmpty(str)) : stringArray);
        }

        [NotNull]
        [PublicAPI]
        public static string[] FromCSV([CanBeNull] this string str, bool removeEmpty = false)
        {
            return string.IsNullOrEmpty(str)
                ? new string[0]
                : str.Split(new[] {','}, removeEmpty ? StringSplitOptions.None : StringSplitOptions.RemoveEmptyEntries);
        }

        [Flags]
        public enum DelimitedSplitOptions
        {
            None = 1 << 0,
            RemoveEmpty = 1 << 1,
            RemoveWhitespaceOnly = 1 << 2,
            Trim = 1 << 3
        }

        [NotNull]
        [PublicAPI]
        public static string[] FromDelimited(this string str, char delimiter, DelimitedSplitOptions splitOptions = DelimitedSplitOptions.None)
        {
            if(string.IsNullOrEmpty(str))
                return new string[0];

            var query = str.Split(new[] {delimiter},
                splitOptions.HasFlag(DelimitedSplitOptions.RemoveEmpty) ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None).AsQueryable();
            if (splitOptions.HasFlag(DelimitedSplitOptions.RemoveWhitespaceOnly))
                query = query.Where(subStr => !string.IsNullOrWhiteSpace(subStr));
            if (splitOptions.HasFlag(DelimitedSplitOptions.Trim))
                query = query.Select(subStr => subStr.Trim());

            return query.ToArray();
        }

        [NotNull]
        [PublicAPI]
        public static int[] AsInts([CanBeNull] this string[] strArray) => strArray?.Select(int.Parse).ToArray() ??
                                                                          throw new ArgumentNullException(
                                                                              nameof(strArray));
        [NotNull]
        [PublicAPI]
        public static int?[] AsIntsOrNulls([CanBeNull] this string[] strArray)
        {
            if(strArray == null)
                return new int?[0];

            return strArray.Select(val =>
            {
                if (int.TryParse(val, out int parsed))
                    return (int?) parsed;
                return null;
            }).ToArray();
        }

        /// <summary>
        /// <see cref="string.EndsWith(string)"/> implementation that checks against multiple phrases
        /// </summary>
        /// <param name="source">String to check</param>
        /// <param name="search">Collection of possible endings</param>
        /// <returns><c>true</c> if <paramref name="source"/> ends in any of the <paramref name="search"/> strings, <c>false</c> otherwise </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="search"/> is <see langword="null" />.</exception>
        public static bool EndsWithAny([NotNull] this string source, [NotNull] ICollection<string> search)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (search == null) throw new ArgumentNullException(nameof(search));
            return search.Any(source.EndsWith);
        }

        [PublicAPI]
        public static string Truncate([NotNull] this string value, int maxLength)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        [NotNull]
        [PublicAPI]
        public static string WordAwareTruncate([NotNull] this string text, int maxLength)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            // Just in case someone reuses this code...
            if (text.Length < maxLength)
                return text;

            var lastGoodCutPosition = -1;
            for (var position = 0; position < maxLength; ++position)
            {
                if (char.IsWhiteSpace(text[position]))
                    lastGoodCutPosition = position;
            }

            if (lastGoodCutPosition == -1)
            {
                // No good place!
                // We'll have to cut wherever:
                return text.Substring(0, maxLength);
            }
            // We got a whitespace we can use
            // Also, we will trim all but letters and digits from the end.
            // This might not be your prefered behaviour, if so: modify to take predicate as 
            // parameter and overload
            return text.Substring(0, lastGoodCutPosition).TrimEnd(c => !char.IsLetterOrDigit(c));
        }

        [NotNull]
        [PublicAPI]
        public static string TrimEnd([NotNull] this string str, Func<char, bool> predicate)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (str.Length == 0)
                return str;

            for (var i = str.Length - 1; i >= 0; --i)
            {
                if (!predicate(str[i]))
                {
                    // we got something we are not supposed to cut, let's cut just after that
                        return str.Substring(0, i + 1);
                }
            }
            // We got through the loop, returning empty then
            return string.Empty;
        }
    }
}