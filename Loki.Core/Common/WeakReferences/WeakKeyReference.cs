using System;
using System.Runtime.Serialization;

namespace Loki.Common
{
    /// <summary>
    /// Provides a weak reference to an object of the given type to be used in a WeakDictionary along with the given comparer.
    /// </summary>
    /// <typeparam name="T">The weak reference type.</typeparam>
    [Serializable]
    public class WeakKeyReference<T> : WeakReference where T : class
    {
        private readonly int hashCode;

        public int KeyHashCode
        {
            get
            {
                return hashCode;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakKeyReference&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="comparer">The comparer.</param>
        public WeakKeyReference(T key, WeakKeyComparer<T> comparer)
            : base(key)
        {
            // retain the object's hash code immediately so that even
            // if the target is GC'ed we will be able to find and
            // remove the dead weak reference.
            hashCode = comparer.GetHashCode(key);
        }

        /// <summary>
        /// Remplit un objet <see cref="T:System.Runtime.Serialization.SerializationInfo"/> de toutes les données nécessaires à la sérialisation de l'objet <see cref="T:System.WeakReference"/> en cours.
        /// </summary>
        /// <param name="info">Objet contenant toutes les données nécessaires pour sérialiser ou désérialiser l'objet <see cref="T:System.WeakReference"/> actuel.</param>
        /// <param name="context">Réservé: Emplacement où les données sérialisées sont stockées et récupérées.</param>
        /// <exception cref="T:System.ArgumentNullException">Le paramètre <paramref name="info"/> est null. </exception>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(HasCodeName, hashCode);
            base.GetObjectData(info, context);
        }

        protected WeakKeyReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            hashCode = info.GetInt32(HasCodeName);
        }

        private const string HasCodeName = "KeyHashCode";
    }
}