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
        [PublicAPI]
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
        [PublicAPI]
        public static IEnumerable<T> OrEmpty<T>([CanBeNull] this IEnumerable<T> src)
        {
            return src ?? Enumerable.Empty<T>();
        }

        [NotNull]
        [PublicAPI]
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

        [NotNull]
        [PublicAPI]
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

        [NotNull]
        [PublicAPI]
        public static IEnumerable<TResult> SelectSafe<T, TResult>([NotNull] this IEnumerable<T> source,
            Func<T, TResult> selector, Action<T, Exception> exceptionCallback)
        {
            var result = default(TResult);
            foreach (var item in source)
            {
                var exceptionOccured = false;
                try
                {
                    result = selector(item);
                }
                catch (Exception exception)
                {
                    exceptionOccured = true;
                    exceptionCallback?.Invoke(item, exception);
                }
                if (!exceptionOccured)
                    yield return result;
            }
        }

        [NotNull]
        [PublicAPI]
        public static IEnumerable<TResult> SelectSafe<T, TResult>([NotNull] this IEnumerable<T> source,
            Func<T, TResult> selector) => SelectSafe(source, selector, null);

        [NotNull]
        [PublicAPI]
        public static string ToCSV<T>([CanBeNull] this IEnumerable<T> src)
        {
            return src == null ? string.Empty : string.Join(",", src);
        }

        /// <summary>
        /// For enumerations of {a,b,c,d} and {1,2,3} returns an enumeration of {a,1,b,2,c,3,d}
        /// </summary>
        /// <typeparam name="T">Any</typeparam>
        /// <param name="source">The source enumeration (will start from this</param>
        /// <param name="other">The other enumeration (will start at position 2)</param>
        /// <param name="appendTail">Whether to include at the end remaining elements of the longer enumeration, if they are not even</param>
        /// <returns>An enumeration that intertwines the two passed to it, starting from <paramref name="source"/></returns>
        [NotNull]
        [MustUseReturnValue]
        public static IEnumerable<T> Intertwine<T>([NotNull] this IEnumerable<T> source, [NotNull] IEnumerable<T> other, bool appendTail = true)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (other == null) throw new ArgumentNullException(nameof(other));

            using (var sourceEnumerator = source.GetEnumerator())
            using (var otherEnumerator = other.GetEnumerator())
            {
                var sourceHasNext = sourceEnumerator.MoveNext();
                var otherHasNext = otherEnumerator.MoveNext();

                while (sourceHasNext && otherHasNext)
                {
                    yield return sourceEnumerator.Current;
                    yield return otherEnumerator.Current;

                    sourceHasNext = sourceEnumerator.MoveNext();
                    otherHasNext = otherEnumerator.MoveNext();
                }

                if (!appendTail) yield break;

                // append rest of source
                while (sourceHasNext)
                {
                    yield return sourceEnumerator.Current;
                    sourceHasNext = sourceEnumerator.MoveNext();
                }

                // OR append rest from the other
                while (otherHasNext)
                {
                    yield return otherEnumerator.Current;
                    otherHasNext = otherEnumerator.MoveNext();
                }
            }
        }
    }
}
