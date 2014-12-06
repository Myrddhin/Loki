using System;

namespace Loki.UI
{
    public class ItemChangedEventArgs<T> : EventArgs
    {
        public T Item { get; private set; }

        public ItemChangedEventArgs(T item)
        {
            Item = item;
        }
    }
}