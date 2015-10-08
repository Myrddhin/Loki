using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Loki.Core.Resources;

namespace Loki.Common
{
    /// <summary>
    /// A generic dictionary, which allows both its keys and values to be garbage collected if there
    /// are no other references to them than from the dictionary itself.
    /// </summary>
    /// <remarks>
    /// If either the key or value of a particular entry in the dictionary has been collected, then
    /// both the key and value become effectively unreachable. However, left-over WeakReference
    /// objects for the key and value will physically remain in the dictionary until
    /// RemoveCollectedEntries is called. This will lead to a discrepancy between the Count property
    /// and the number of iterations required to visit all of the elements of the dictionary using
    /// its enumerator or those of the Keys and Values collections. Similarly, CopyTo will copy
    /// fewer than Count elements in this situation.
    /// </remarks>
    public sealed class WeakDictionary<TKey, TValue> : BaseDictionary<TKey, TValue>
        where TKey : class
        where TValue : class
    {
        private ConcurrentDictionary<WeakKeyReference<TKey>, WeakReference<TValue>> internalDictionary;

        private WeakKeyComparer<TKey> internalComparer;

        public WeakDictionary()
            : this(0, null)
        {
        }

        public WeakDictionary(int capacity)
            : this(capacity, null)
        {
        }

        public WeakDictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer)
        {
        }

        public WeakDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            this.internalComparer = new WeakKeyComparer<TKey>(comparer);
            this.internalDictionary = new ConcurrentDictionary<WeakKeyReference<TKey>, WeakReference<TValue>>(Environment.ProcessorCount * 2, capacity, this.internalComparer);
        }

        /// <summary>
        /// WARNING: The count returned here may include entries for which
        /// either the key or value objects have already been garbage collected. Call
        /// RemoveCollectedEntries to weed out collected entries and update the count accordingly.
        /// </summary>
        public override int Count
        {
            get { return this.internalDictionary.Count; }
        }

        public override void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                throw Toolkit.Common.ErrorManager.BuildError<ArgumentException>(Errors.Utils_WeakDictionnay_KeyNullException);
            }

            WeakKeyReference<TKey> weakKey = new WeakKeyReference<TKey>(key, this.internalComparer);
            WeakReference<TValue> weakValue = new WeakReference<TValue>(value);
            this.internalDictionary.TryAdd(weakKey, weakValue);
        }

        public override bool ContainsKey(TKey key)
        {
            WeakKeyReference<TKey> weakKey = new WeakKeyReference<TKey>(key, this.internalComparer);
            return this.internalDictionary.ContainsKey(weakKey);
        }

        public override bool Remove(TKey key)
        {
            WeakReference<TValue> weakValue;
            WeakKeyReference<TKey> weakKey = new WeakKeyReference<TKey>(key, this.internalComparer);
            return this.internalDictionary.TryRemove(weakKey, out weakValue);
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            WeakReference<TValue> weakValue;
            WeakKeyReference<TKey> weakKey = new WeakKeyReference<TKey>(key, this.internalComparer);
            if (this.internalDictionary.TryGetValue(weakKey, out weakValue))
            {
                bool buffer = weakValue.TryGetTarget(out value);
                return buffer;
            }

            value = null;
            return false;
        }

        protected override void SetValue(TKey key, TValue value)
        {
            WeakKeyReference<TKey> weakKey = new WeakKeyReference<TKey>(key, this.internalComparer);
            this.internalDictionary[weakKey] = new WeakReference<TValue>(value);
        }

        public override void Clear()
        {
            this.internalDictionary.Clear();
        }

        public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var kvp in this.internalDictionary)
            {
                WeakKeyReference<TKey> weakKey = kvp.Key;
                WeakReference<TValue> weakValue = kvp.Value;
                TKey key = (TKey)weakKey.Target;
                TValue value = null;
                bool valueAlive = weakValue.TryGetTarget(out value);
                if (weakKey.IsAlive && valueAlive)
                {
                    yield return new KeyValuePair<TKey, TValue>(key, value);
                }
            }
        }

        /// <summary>
        /// Removes the left-over weak references for entries in the dictionary whose key or value
        /// has already been reclaimed by the garbage collector. This will reduce the dictionary's
        /// Count by the number of dead key-value pairs that were eliminated.
        /// </summary>
        public void RemoveCollectedEntries()
        {
            List<WeakKeyReference<TKey>> entriesToRemove = null;
            foreach (var kvp in this.internalDictionary)
            {
                WeakKeyReference<TKey> weakKey = kvp.Key;
                WeakReference<TValue> weakValue = kvp.Value;

                TValue value = null;
                bool valueAlive = weakValue.TryGetTarget(out value);

                if (!weakKey.IsAlive || !valueAlive)
                {
                    if (entriesToRemove == null)
                    {
                        entriesToRemove = new List<WeakKeyReference<TKey>>();
                    }

                    entriesToRemove.Add(weakKey);
                }
            }

            if (entriesToRemove != null)
            {
                WeakReference<TValue> weakValue;
                foreach (var key in entriesToRemove)
                {
                    this.internalDictionary.TryRemove(key, out weakValue);
                }
            }
        }
    }
}