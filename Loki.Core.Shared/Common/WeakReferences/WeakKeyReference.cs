using System;
using System.Collections.Generic;

namespace Loki.Common
{
    /// <summary>
    /// Provides a weak reference to an object of the given type to be used in a WeakDictionary along with the given comparer.
    /// </summary>
    /// <typeparam name="T">The weak reference type.</typeparam>
    public class WeakKeyReference<T> : WeakReference
        where T : class
    {
        public int KeyHashCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakKeyReference&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="comparer">The comparer.</param>
        public WeakKeyReference(T key, IEqualityComparer<object> comparer)
            : base(key)
        {
            // retain the object's hash code immediately so that even
            // if the target is GC'ed we will be able to find and
            // remove the dead weak reference.
            this.KeyHashCode = comparer.GetHashCode(key);
        }
    }
}