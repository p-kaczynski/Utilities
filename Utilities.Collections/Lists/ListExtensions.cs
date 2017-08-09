using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Utilities.Collections.Entities;

namespace Utilities.Collections.Lists
{
    /// Contains extension methods for <see cref="IList{T}"/> interface
    public static class ListExtensions
    {
        /// <exception cref="ArgumentException"><paramref name="howMany"/> argument exceeds <paramref name="source"/> collection length.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null" />.</exception>
        [Pure]
        [NotNull]
        [UsedImplicitly]
        public static IEnumerable<T> TakeRandom<T>([NotNull] this IList<T> source, int howMany)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if(howMany > source.Count)
                throw new ArgumentException($"{nameof(howMany)} argument exceeds {nameof(source)} collection length;");
            if (howMany < 0)
                throw new ArgumentException($"{nameof(howMany)} argument is negative");
            if (howMany == 0)
                return Enumerable.Empty<T>();
            var random = new Random();

            // That will greatly increase efficiency for reasonably common use of getting a single random element
            // A dedicated method would be probably much better, but even then people would surely call this one with (1) instead
            if (howMany == 1)
                return source[random.Next(0, source.Count - 1)].Yield();

            // This method shuffles the possible IList indexes and takes top howMany - it will allocate source.Count bytes for indexers
            // as well as perform source.Count random.Next() calls. It is easy and effective, but not very efficient, especially
            // when source.Count >> howMany.
            // TODO: compare operational complexity against randomly generating numbers until howMany non duplicated are generated, then put both here and select depending on source.Count vs howMany
            return
                Enumerable.Range(0, source.Count - 1).OrderBy(x => random.Next()).Take(howMany).Select(id => source[id]);
        }

        public static IEnumerable<List<T>> Partition<T>(this IList<T> source, int size)
        {
            for (var i = 0; i < Math.Ceiling(source.Count / (double)size); i++)
                yield return new List<T>(source.Skip(size * i).Take(size));
        }
    }
}