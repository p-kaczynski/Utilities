using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Utilities.Collections.Arrays
{
    /// <summary>
    /// Contains extension methods for Arrays.
    /// </summary>
    [PublicAPI]
    public static class ArrayExtensions
    {
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">margins must be non-negative.</exception>
        [Pure]
        [NotNull]
        public static T[] Splice<T>([NotNull]this T[] array, int preMargin, int postMargin)
        {
            if(array == null) throw new ArgumentNullException(nameof(array));
            if(preMargin < 0) throw new ArgumentException("Margins must be non-negative.", nameof(preMargin));
            if(postMargin < 0) throw new ArgumentException("Margins must be non-negative.", nameof(postMargin));

            var newLength = array.Length - (preMargin + postMargin);
            if(newLength <= 0) return new T[0];

            var newArray = new T[newLength];

            // The exceptions should not happen due to checks above
            // ReSharper disable ExceptionNotDocumented
            // ReSharper disable ExceptionNotDocumentedOptional
            Array.Copy(array, preMargin, newArray, 0, newLength);
            // ReSharper restore ExceptionNotDocumentedOptional
            // ReSharper restore ExceptionNotDocumented        

            return newArray;
        }

        [NotNull]
        [PublicAPI]
        public static T[] OrEmptyArray<T>([CanBeNull] this T[] arr) => arr ?? new T[0];

        ///// <summary>
        ///// Resizes the array and adds <paramref name="elements"/> to the end of it, in provided order.
        ///// </summary>
        ///// <typeparam name="T">Array element type</typeparam>
        ///// <param name="array">Array to modify</param>
        ///// <param name="elements">Elements to add</param>
        ///// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null" />.</exception>
        ///// <exception cref="ArgumentNullException"><paramref name="elements"/> is <see langword="null" />.</exception>
        ///// <exception cref="OverflowException">The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue" /> elements.</exception>
        //[NotNull]
        //public static void Append<T>([NotNull]this T[] array, [NotNull] params T[] elements)
        //{
        //    if (array == null) throw new ArgumentNullException(nameof(array));
        //    if (elements == null) throw new ArgumentNullException(nameof(elements));

        //    // quick bail if no elements to add
        //    if (elements.Length == 0) return;

        //    Array.Resize(ref array, array.Length + elements.Length);
        //    for (var i = 0; i < elements.Length; ++i)
        //        array[array.Length - i - 1] = elements[i];
        //}
    }
}