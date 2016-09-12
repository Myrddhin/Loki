using System.Diagnostics.CodeAnalysis;

namespace Loki.Common
{
    internal class ConcurrentListNode<T> : IListNode<T>
    {
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "must be field to support interlock read/write")]
        internal bool Deleted;

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "must be field to support interlock read/write")]
        internal bool Read;

        public long Version { get; private set; }

        public T Value { get; private set; }

        public ConcurrentListNode<T> Next { get; set; }

        public ConcurrentListNode<T> Previous { get; set; }

        public ConcurrentListNode(T value, long version)
        {
            Deleted = false;
            Read = false;
            Value = value;
            Version = version;
        }
    }
}