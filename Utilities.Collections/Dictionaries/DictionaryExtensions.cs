using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Utilities.Collections.Dictionaries
{
    public static class DictionaryExtensions
    {
        [CanBeNull]
        [PublicAPI]
        public static TValue GetOrAdd<TKey, TValue>([NotNull]this Dictionary<TKey, TValue> source, [NotNull] TKey key,
            [NotNull]Func<TValue> valueCreator)
        {
            if (!source.ContainsKey(key))
                source[key] = valueCreator();

            return source[key];
        }

        [CanBeNull]
        [PublicAPI]
        public static TValue GetOrThrowWithKey<TKey, TValue>([NotNull]this IDictionary<TKey, TValue> source, [NotNull]TKey key)
        {
            if (source.TryGetValue(key, out TValue value))
                return value;
            throw new KeyNotFoundException($"Key: '{key}' was not found in the dictionary.");
        }
    }
}