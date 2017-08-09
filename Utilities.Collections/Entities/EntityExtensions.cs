using System.Collections.Generic;

namespace Utilities.Collections.Entities
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Returns a single object as <see cref="IEnumerable{T}"/> containing this object.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="obj">Object to wrap as <see cref="IEnumerable{T}"/></param>
        /// <returns><see cref="IEnumerable{T}"/> containing only <paramref name="obj"/></returns>
        public static IEnumerable<T> Yield<T>(this T obj)
        {
            yield return obj;
        }
    }
}