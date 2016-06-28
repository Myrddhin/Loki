using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

using Loki.Common.Diagnostics;
using Loki.Core.Resources;

namespace Loki.Common
{
    /// <summary>
    /// Represents a dictionary mapping keys to values.
    /// </summary>
    /// <typeparam name="TKey">
    /// Key type.
    /// </typeparam>
    /// <typeparam name="TValue">
    /// Value type.
    /// </typeparam>
    /// <remarks>
    /// Provides the plumbing for the portions of IDictionary&lt;TKey, TValue&gt; which can reasonably be implemented without any dependency on the underlying representation of the dictionary.
    /// </remarks>
    [DebuggerDisplay(DebugDisplay)]
    [DebuggerTypeProxy(Prefix + DebugProxy + Suffix)]
    public abstract class BaseDictionary<TKey, TValue> : BaseObject, IDictionary<TKey, TValue>
    {
        protected BaseDictionary(IDiagnostics logger)
            : base(logger)
        {
        }

        private const string DebugDisplay = "Count = {Count}";

        private const string DebugProxy = "DictionaryDebugView`2";

        private const string Prefix = "System.Collections.Generic.Mscorlib_";

        private const string Suffix = ",mscorlib,Version=2.0.0.0,Culture=neutral,PublicKeyToken=b77a5c561934e089";

        private KeyCollection keys;

        private ValueCollection values;

        public abstract int Count { get; }

        public abstract void Clear();

        public abstract void Add(TKey key, TValue value);

        public abstract bool ContainsKey(TKey key);

        public abstract bool Remove(TKey key);

        public abstract bool TryGetValue(TKey key, out TValue value);

        public abstract IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();

        protected abstract void SetValue(TKey key, TValue value);

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                if (keys == null)
                {
                    Interlocked.CompareExchange(ref keys, new KeyCollection(this), null);
                }

                return keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                if (values == null)
                {
                    Interlocked.CompareExchange(ref values, new ValueCollection(this), null);
                }

                return values;
            }
        }

        [SuppressMessage("StyleCop.Extensions.LokiNamingRules", "EX1002:ElementMustBeginWithUpperCaseLetter", Justification = "this is indexer keyword")]
        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (!TryGetValue(key, out value))
                {
                    throw new KeyNotFoundException();
                }

                return value;
            }

            set
            {
                SetValue(key, value);
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            if (!TryGetValue(item.Key, out value))
            {
                return false;
            }

            return EqualityComparer<TValue>.Default.Equals(value, item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            Copy(this, array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!Contains(item))
            {
                return false;
            }

            return Remove(item.Key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private abstract class Collection<T> : ICollection<T>
        {
            protected readonly BaseDictionary<TKey, TValue> ReferenceDict;

            protected Collection(BaseDictionary<TKey, TValue> dictionary)
            {
                ReferenceDict = dictionary;
            }

            public int Count
            {
                get { return ReferenceDict.Count; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                ReferenceDict.Copy(this, array, arrayIndex);
            }

            public virtual bool Contains(T item)
            {
                return this.Any(element => EqualityComparer<T>.Default.Equals(element, item));
            }

            public IEnumerator<T> GetEnumerator()
            {
                return ReferenceDict.Select(GetItem).GetEnumerator();
            }

            protected abstract T GetItem(KeyValuePair<TKey, TValue> pair);

            public bool Remove(T item)
            {
                throw new NotSupportedException();
            }

            public void Add(T item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class KeyCollection : Collection<TKey>
        {
            public KeyCollection(BaseDictionary<TKey, TValue> dictionary)
                : base(dictionary)
            {
            }

            protected override TKey GetItem(KeyValuePair<TKey, TValue> pair)
            {
                return pair.Key;
            }

            public override bool Contains(TKey item)
            {
                return ReferenceDict.ContainsKey(item);
            }
        }

        private class ValueCollection : Collection<TValue>
        {
            public ValueCollection(BaseDictionary<TKey, TValue> dictionary)
                : base(dictionary)
            {
            }

            protected override TValue GetItem(KeyValuePair<TKey, TValue> pair)
            {
                return pair.Value;
            }
        }

        private void Copy<T>(ICollection<T> source, IList<T> array, int arrayIndex)
        {
            if (array == null)
            {
                throw BuildError<ArgumentException>(Errors.Utils_BaseDictionary_CopyNullDest);
            }

            if (arrayIndex < 0 || arrayIndex > array.Count)
            {
                throw BuildError<ArgumentOutOfRangeException>(Errors.Utils_BaseDictionary_CopyIndexOutOfRange);
            }

            if ((array.Count - arrayIndex) < source.Count)
            {
                throw BuildError<ArgumentException>(Errors.Utils_BaseDictionary_CopySmallDest);
            }

            foreach (T item in source)
            {
                array[arrayIndex++] = item;
            }
        }
    }
}