using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace Utilities.Collections.Dictionaries
{
    public class TwoWayDictionary<T1,T2> : ICollection<KeyValuePair<T1, T2>>
    {
        private readonly IDictionary<T1, T2> _forward;
        private readonly IDictionary<T2, T1> _backward;

        public TwoWayDictionary() : this(EqualityComparer<T1>.Default, EqualityComparer<T2>.Default)
        {
           
        }

        public TwoWayDictionary(IEnumerable<KeyValuePair<T1,T2>> source) : this(source, EqualityComparer<T1>.Default, EqualityComparer<T2>.Default)
        {
        }

        public TwoWayDictionary(IEqualityComparer<T1> forwardKeyComparer, IEqualityComparer<T2> backwardKeyComparer)
        {
            _forward = new Dictionary<T1, T2>(forwardKeyComparer);
            _backward= new Dictionary<T2, T1>(backwardKeyComparer);

            Forward = new ForwardFacade(this);
            Backward = new BackwardFacade(this);
        }

        public TwoWayDictionary(IEnumerable<KeyValuePair<T1, T2>> source, EqualityComparer<T1> forwardKeyComparer, EqualityComparer<T2> backwardKeyComparer) : this(forwardKeyComparer,backwardKeyComparer)
        {
            foreach (var kvp in source)
                Add(kvp.Key, kvp.Value);
        }

        [PublicAPI]
        public IReadOnlyDictionary<T1, T2> Forward { get; }
        [PublicAPI]
        public IReadOnlyDictionary<T2, T1> Backward { get; }

        [PublicAPI]
        public void Add([NotNull]T1 item1, [NotNull]T2 item2)
        {
            if (ReferenceEquals(item1, null))
                throw new ArgumentNullException(nameof(item1));
            if (ReferenceEquals(item2, null))
                throw new ArgumentNullException(nameof(item2));

            _forward.Add(item1, item2);
            try
            {
                _backward.Add(item2, item1);
            }
            catch (ArgumentException)
            {
                // ensure we don't have disparity
                _forward.Remove(item1);
                throw;
            }
        }

        [PublicAPI]
        public bool Remove([NotNull] T1 item1)
            => _forward.TryGetValue(item1, out var item2)
               && _forward.Remove(item1)
               && _backward.Remove(item2);

        [PublicAPI]
        public bool RemoveBackwards([NotNull] T2 item2)
            => _backward.TryGetValue(item2, out var item1)
               && _backward.Remove(item2)
               && _forward.Remove(item1);

        public void Add(KeyValuePair<T1, T2> item)
        {
            Add(item.Key, item.Value);
        }

        [PublicAPI]
        public void Clear()
        {
            _forward.Clear();
            _backward.Clear();
        }

        public bool Contains(KeyValuePair<T1, T2> item) 
            => _forward.ContainsKey(item.Key) && _backward.ContainsKey(item.Value);

        public void CopyTo(KeyValuePair<T1, T2>[] array, int arrayIndex)
        {
            _forward.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<T1, T2> item)
        {
            return _forward.TryGetValue(item.Key, out var item2)
                   && _backward.TryGetValue(item.Value, out var item1)
                   && _forward.Remove(item1)
                   && _backward.Remove(item2);
        }

        public int Count => _forward.Count;

        public bool IsReadOnly => _forward.IsReadOnly || _backward.IsReadOnly;

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            return _forward.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_forward).GetEnumerator();
        }

        #region Facade

        private abstract class DictionaryFacade
        {
            protected readonly TwoWayDictionary<T1, T2> TwoWayParent;

            protected DictionaryFacade(TwoWayDictionary<T1, T2> twoWayParent)
            {
                TwoWayParent = twoWayParent;
            }
        }

        private sealed class ForwardFacade : DictionaryFacade, IReadOnlyDictionary<T1,T2>
        {
            private IReadOnlyDictionary<T1, T2> DictionaryImplementation { get; }

            public ForwardFacade(TwoWayDictionary<T1, T2> twoWayParent) : base(twoWayParent)
            {
                DictionaryImplementation = new ReadOnlyDictionary<T1, T2>(TwoWayParent._forward);
            }

            IEnumerator<KeyValuePair<T1, T2>> IEnumerable<KeyValuePair<T1, T2>>.GetEnumerator()
            {
                return DictionaryImplementation.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable) DictionaryImplementation).GetEnumerator();
            }

            int IReadOnlyCollection<KeyValuePair<T1, T2>>.Count => DictionaryImplementation.Count;

            bool IReadOnlyDictionary<T1, T2>.ContainsKey(T1 key)
            {
                return DictionaryImplementation.ContainsKey(key);
            }

            bool IReadOnlyDictionary<T1, T2>.TryGetValue(T1 key, out T2 value)
            {
                return DictionaryImplementation.TryGetValue(key, out value);
            }

            T2 IReadOnlyDictionary<T1, T2>.this[T1 key] => DictionaryImplementation[key];

            IEnumerable<T1> IReadOnlyDictionary<T1, T2>.Keys => DictionaryImplementation.Keys;

            IEnumerable<T2> IReadOnlyDictionary<T1, T2>.Values => DictionaryImplementation.Values;
        }

        private sealed class BackwardFacade : DictionaryFacade, IReadOnlyDictionary<T2, T1>
        {
            private IReadOnlyDictionary<T2, T1> DictionaryImplementation { get; }

            public BackwardFacade(TwoWayDictionary<T1, T2> twoWayParent) : base(twoWayParent)
            {
                DictionaryImplementation = new ReadOnlyDictionary<T2, T1>(TwoWayParent._backward);
            }

            IEnumerator<KeyValuePair<T2, T1>> IEnumerable<KeyValuePair<T2, T1>>.GetEnumerator()
            {
                return DictionaryImplementation.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable) DictionaryImplementation).GetEnumerator();
            }

            int IReadOnlyCollection<KeyValuePair<T2, T1>>.Count => DictionaryImplementation.Count;

            bool IReadOnlyDictionary<T2, T1>.ContainsKey(T2 key)
            {
                return DictionaryImplementation.ContainsKey(key);
            }

            bool IReadOnlyDictionary<T2, T1>.TryGetValue(T2 key, out T1 value)
            {
                return DictionaryImplementation.TryGetValue(key, out value);
            }

            T1 IReadOnlyDictionary<T2, T1>.this[T2 key] => DictionaryImplementation[key];

            IEnumerable<T2> IReadOnlyDictionary<T2, T1>.Keys => DictionaryImplementation.Keys;

            IEnumerable<T1> IReadOnlyDictionary<T2, T1>.Values => DictionaryImplementation.Values;
        }
        #endregion
    }
}