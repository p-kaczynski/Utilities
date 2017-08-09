using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Utilities.Collections.Enumerables
{
    /// <summary>
    /// Contains extension methods for <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Helper method leveraging <see langword="params"/> in place of an array creator
        /// </summary>
        /// <typeparam name="T">Enumerable element type</typeparam>
        /// <param name="source">source enumerable</param>
        /// <param name="elements">elements to add</param>
        /// <returns>An enumerable representing <paramref name="source"/> followed by <paramref name="elements"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="elements" /> is null.</exception>
        [NotNull]
        [UsedImplicitly]
        public static IEnumerable<T> ConcatParams<T>([NotNull] this IEnumerable<T> source, [NotNull] params T[] elements)
        {
            return Enumerable.Concat(source, elements);
        }

        /// <summary>
        /// Ensures that the return value is not a null enumerable (substitutes empty for null so foreach etc. don't throw)
        /// </summary>
        /// <typeparam name="T">Type of the enumerated item</typeparam>
        /// <param name="src">Any enumerable</param>
        /// <returns><paramref name="src"/> if not null, else empty <typeparamref name="T"/> enumerable</returns>
        [NotNull]
        [UsedImplicitly]
        public static IEnumerable<T> OrEmpty<T>([CanBeNull] this IEnumerable<T> src)
        {
            return src ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<T> SkipLast<T>([NotNull] this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var it = source.GetEnumerator();
            bool hasRemainingItems;
            var isFirstIteration = true;
            var item = default(T);

            do
            {
                hasRemainingItems = it.MoveNext();
                if (!hasRemainingItems) continue;
                if (!isFirstIteration) yield return item;

                item = it.Current;
                isFirstIteration = false;
            } while (hasRemainingItems);
            it.Dispose();
        }

        public static IEnumerable<T> SkipLastN<T>([NotNull] this IEnumerable<T> source, int n)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var it = source.GetEnumerator();
            bool hasRemainingItems;
            var cache = new Queue<T>(n + 1);

            do
            {
                // ReSharper disable once AssignmentInConditionalExpression - on purpose
                if (hasRemainingItems = it.MoveNext())
                {
                    cache.Enqueue(it.Current);
                    if (cache.Count > n)
                        yield return cache.Dequeue();
                }
            } while (hasRemainingItems);
            it.Dispose();
        }

        public static string ToCSV<T>([CanBeNull] this IEnumerable<T> src)
        {
            return src == null ? string.Empty : string.Join(",", src);
        }
    }
}
