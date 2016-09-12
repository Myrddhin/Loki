using System.Collections.Generic;

namespace Loki.Common
{
    public sealed class WeakKeyComparer<T> : IEqualityComparer<object>
    where T : class
    {
        private readonly IEqualityComparer<T> internalComparer;

        public WeakKeyComparer(IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                internalComparer = EqualityComparer<T>.Default;
            }
            else
            {
                this.internalComparer = comparer;
            }
        }

        public int GetHashCode(object obj)
        {
            var weakKey = obj as WeakKeyReference<T>;
            return weakKey?.KeyHashCode ?? this.internalComparer.GetHashCode((T)obj);
        }

        // Note: There are actually 9 cases to handle here.
        //
        //  Let Wa = Alive Weak Reference
        //  Let Wd = Dead Weak Reference
        //  Let S  = Strong Reference
        //
        //  x  | y  | Equals(x,y)
        // -------------------------------------------------
        //  Wa | Wa | comparer.Equals(x.Target, y.Target)
        //  Wa | Wd | false
        //  Wa | S  | comparer.Equals(x.Target, y)
        //  Wd | Wa | false
        //  Wd | Wd | x == y
        //  Wd | S  | false
        //  S  | Wa | comparer.Equals(x, y.Target)
        //  S  | Wd | false
        //  S  | S  | comparer.Equals(x, y)
        // -------------------------------------------------
        public new bool Equals(object x, object y)
        {
            bool firstIsDead, secondIsDead;
            var first = GetTarget(x, out firstIsDead);
            var second = GetTarget(y, out secondIsDead);

            if (firstIsDead)
            {
                return secondIsDead && x == y;
            }

            return !secondIsDead && this.internalComparer.Equals(first, second);
        }

        private static T GetTarget(object obj, out bool dead)
        {
            var weakRef = obj as WeakKeyReference<T>;
            T target;
            if (weakRef != null)
            {
                target = (T)weakRef.Target;
                dead = !weakRef.IsAlive;
            }
            else
            {
                target = (T)obj;
                dead = false;
            }

            return target;
        }
    }
}