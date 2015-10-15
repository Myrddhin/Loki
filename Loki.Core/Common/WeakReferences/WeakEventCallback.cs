using System;

namespace Loki.Common
{
    public class WeakEventCallback<TListener, TArgs> : IWeakEventCallback<TArgs>
    {
        public object Listener
        {
            get
            {
                return listenerReference.Target;
            }
        }

        private readonly WeakReference listenerReference;

        private readonly Action<TListener, object, TArgs> callback;

        public WeakEventCallback(
            object listener,
            Action<TListener, object, TArgs> callback)
        {
            listenerReference = new WeakReference(listener);
            this.callback = callback;
        }

        public bool Invoke(object sender, TArgs e)
        {
            TListener listenerForCallback = (TListener)listenerReference.Target;
            if (listenerForCallback != null)
            {
                callback(listenerForCallback, sender, e);
                return true;
            }

            return false;
        }
    }
}