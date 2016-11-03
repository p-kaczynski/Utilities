using System;
using JetBrains.Annotations;

namespace Utilities.Collections.Arrays
{
    /// <summary>
    /// Contains extension methods for Arrays.
    /// </summary>
    [UsedImplicitly]
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
    }
}