using System;

namespace Loki.UI.Models
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