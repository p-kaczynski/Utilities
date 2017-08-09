using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Utilities.Collections.Enumerables
{
    /// <summary>
    /// Contains a set of methods providing generic-based version of Enumerable.OfType{TResult} that accept multiple types.
    /// </summary>
    /// <remarks>
    /// Note that those can be made faster by duplicating Where clause into every version, which skips redundant checks in the method that accepts Type[]
    /// </remarks>
    public static class OfTypesExtensions
    {
        ///// <summary>
        ///// Filters the provided <paramref name="enumerable"/> into a new one containing only those elements which are instance of any of T1, T2... type parameters.
        ///// </summary>
        ///// <typeparam name="T">Base class</typeparam>
        ///// <typeparam name="T1">Sub-class #1</typeparam>
        ///// <typeparam name="T2">Sub-class #2</typeparam>
        ///// <param name="enumerable">Enumerable to filter</param>
        ///// <returns>Enumerable that contains items of <paramref name="enumerable"/> that are instances of any of provided types</returns>
        ///// <exception cref="ArgumentNullException">enumerable is <see langword="null" />.</exception>
        //[NotNull]
        //[UsedImplicitly]
        //public static IEnumerable<T> OfTypes<T, T1, T2>([NotNull] this IEnumerable<T> enumerable) where T1 : T where T2 : T
        //{
        //    return enumerable.OfTypes(typeof(T1), typeof(T2));
        //}

        ///// <summary>
        ///// Filters the provided <paramref name="enumerable"/> into a new one containing only those elements which are instance of any of T1, T2... type parameters.
        ///// </summary>
        ///// <typeparam name="T">Base class</typeparam>
        ///// <typeparam name="T1">Sub-class #1</typeparam>
        ///// <typeparam name="T2">Sub-class #2</typeparam>
        ///// <typeparam name="T3">Sub class #3</typeparam>
        ///// <param name="enumerable">Enumerable to filter</param>
        ///// <returns>Enumerable that contains items of <paramref name="enumerable"/> that are instances of any of provided types</returns>
        ///// <exception cref="ArgumentNullException">enumerable is <see langword="null" />.</exception>
        //[NotNull]
        //[UsedImplicitly]
        //public static IEnumerable<T> OfTypes<T, T1, T2, T3>([NotNull] this IEnumerable<T> enumerable) where T1 : T where T2 : T
        //{
        //    return enumerable.OfTypes(typeof(T1), typeof(T2), typeof(T3));
        //}

        ///// <summary>
        ///// Filters the provided <paramref name="enumerable"/> into a new one containing only those elements which are instance of any of T1, T2... type parameters.
        ///// </summary>
        ///// <typeparam name="T">Base class</typeparam>
        ///// <typeparam name="T1">Sub-class #1</typeparam>
        ///// <typeparam name="T2">Sub-class #2</typeparam>
        ///// <typeparam name="T3">Sub class #3</typeparam>
        ///// <typeparam name="T4">Sub class #4</typeparam>
        ///// <param name="enumerable">Enumerable to filter</param>
        ///// <returns>Enumerable that contains items of <paramref name="enumerable"/> that are instances of any of provided types</returns>
        ///// <exception cref="ArgumentNullException">enumerable is <see langword="null" />.</exception>
        //[NotNull]
        //[UsedImplicitly]
        //public static IEnumerable<T> OfTypes<T, T1, T2, T3, T4>([NotNull] this IEnumerable<T> enumerable) where T1 : T where T2 : T
        //{
        //    return enumerable.OfTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        //}

        ///// <summary>
        ///// Filters the provided <paramref name="enumerable"/> into a new one containing only those elements which are instance of any of T1, T2... type parameters.
        ///// </summary>
        ///// <typeparam name="T">Base class</typeparam>
        ///// <typeparam name="T1">Sub-class #1</typeparam>
        ///// <typeparam name="T2">Sub-class #2</typeparam>
        ///// <typeparam name="T3">Sub class #3</typeparam>
        ///// <typeparam name="T4">Sub class #4</typeparam>
        ///// <typeparam name="T5">Sub class #5</typeparam>
        ///// <param name="enumerable">Enumerable to filter</param>
        ///// <returns>Enumerable that contains items of <paramref name="enumerable"/> that are instances of any of provided types</returns>
        ///// <exception cref="ArgumentNullException">enumerable is <see langword="null" />.</exception>
        //[NotNull]
        //[UsedImplicitly]
        //public static IEnumerable<T> OfTypes<T, T1, T2, T3, T4, T5>([NotNull] this IEnumerable<T> enumerable) where T1 : T where T2 : T
        //{
        //    return enumerable.OfTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        //}

        ///// <summary>
        ///// Filters the provided <paramref name="enumerable"/> into a new one containing only those elements which are instance of any of T1, T2... type parameters.
        ///// </summary>
        ///// <typeparam name="T">Base class</typeparam>
        ///// <typeparam name="T1">Sub-class #1</typeparam>
        ///// <typeparam name="T2">Sub-class #2</typeparam>
        ///// <typeparam name="T3">Sub class #3</typeparam>
        ///// <typeparam name="T4">Sub class #4</typeparam>
        ///// <typeparam name="T5">Sub class #5</typeparam>
        ///// <typeparam name="T6">Sub class #6</typeparam>
        ///// <param name="enumerable">Enumerable to filter</param>
        ///// <returns>Enumerable that contains items of <paramref name="enumerable"/> that are instances of any of provided types</returns>
        ///// <exception cref="ArgumentNullException">enumerable is <see langword="null" />.</exception>
        //[NotNull]
        //[UsedImplicitly]
        //public static IEnumerable<T> OfTypes<T, T1, T2, T3, T4, T5, T6>([NotNull] this IEnumerable<T> enumerable) where T1 : T where T2 : T
        //{
        //    return enumerable.OfTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
        //}

        /// <summary>
        /// Filters the provided <paramref name="enumerable"/> into a new one containing only those elements which are instance of any of <paramref name="types"/>.
        /// </summary>
        /// <typeparam name="T">Base type of the collection</typeparam>
        /// <param name="enumerable">Enumerable to filter</param>
        /// <param name="types">Types to filter by</param>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="types"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="types"/> contains types that are not <typeparamref name="T"/> or types assignable to <typeparamref name="T"/></exception>
        /// <returns>Enumerable that contains items of <paramref name="enumerable"/> that are instances of any of provided types</returns>
        [NotNull]
        [UsedImplicitly]
        public static IEnumerable<T> OfTypes<T>([NotNull] this IEnumerable<T> enumerable, [NotNull] params Type[] types)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (types == null) throw new ArgumentNullException(nameof(types));
            if (!types.All(type => typeof (T).IsAssignableFrom(type)))
                throw new ArgumentException(
                    $"Parameter {nameof(types)} contains types that are not {typeof (T).Name} or types assignable to {typeof (T).Name}");

            return enumerable.Where(item => types.Any(type => type.IsInstanceOfType(item)));
        }
    }
}