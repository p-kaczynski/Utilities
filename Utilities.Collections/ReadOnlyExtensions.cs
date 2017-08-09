using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Utilities.Collections
{
    /// <summary>
    /// Provieds helper extension methods to get read-only equivalents of provided collections
    /// </summary>
    public static class ReadOnlyExtensions
    {
        /// <summary>
        /// Enumerates provided <paramref name="enumerable"/> into a read-only collection
        /// </summary>
        /// <typeparam name="T">Type of the element</typeparam>
        /// <param name="enumerable">Enumerable to transform</param>
        /// <returns>A collection as read-only collection interface</returns>
        [CanBeNull]
        [UsedImplicitly]
        [MustUseReturnValue]
        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>([CanBeNull] this IEnumerable<T> enumerable)
        {
            return enumerable?.ToArray();
        }

        /// <summary>
        /// Returns provided list as read-only collection
        /// </summary>
        /// <typeparam name="T">Type of the element</typeparam>
        /// <param name="list">List to return as read-only collection</param>
        /// <returns>A collection as read-only collection interface</returns>
        [CanBeNull]
        [UsedImplicitly]
        [MustUseReturnValue]
        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>([CanBeNull] this List<T> list)
        {
            return list;
        }

        /// <summary>
        /// Returns provided array as read-only collection
        /// </summary>
        /// <typeparam name="T">Type of the element</typeparam>
        /// <param name="array">Array to return as read-only collection</param>
        /// <returns>A collection as read-only collection interface</returns>
        [CanBeNull]
        [UsedImplicitly]
        [MustUseReturnValue]
        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>([CanBeNull] this T[] array)
        {
            return array;
        }

        /// <summary>
        /// Enumerates provided <paramref name="enumerable"/> into a read-only collection
        /// </summary>
        /// <typeparam name="T">Type of the element</typeparam>
        /// <param name="enumerable">Enumerable to transform</param>
        /// <returns>A collection as read-only collection interface</returns>
        [CanBeNull]
        [UsedImplicitly]
        [MustUseReturnValue]
        public static IReadOnlyList<T> ToReadOnlyList<T>([CanBeNull] this IEnumerable<T> enumerable)
        {
            return enumerable?.ToArray();
        }

        /// <summary>
        /// Returns provided list as read-only collection
        /// </summary>
        /// <typeparam name="T">Type of the element</typeparam>
        /// <param name="list">List to return as read-only collection</param>
        /// <returns>A collection as read-only collection interface</returns>
        [CanBeNull]
        [UsedImplicitly]
        [MustUseReturnValue]
        public static IReadOnlyList<T> ToReadOnlyList<T>([CanBeNull] this List<T> list)
        {
            return list;
        }

        /// <summary>
        /// Returns provided array as read-only collection
        /// </summary>
        /// <typeparam name="T">Type of the element</typeparam>
        /// <param name="array">Array to return as read-only collection</param>
        /// <returns>A collection as read-only collection interface</returns>
        [CanBeNull]
        [UsedImplicitly]
        [MustUseReturnValue]
        public static IReadOnlyList<T> ToReadOnlyList<T>([CanBeNull] this T[] array)
        {
            return array;
        }
    }
}