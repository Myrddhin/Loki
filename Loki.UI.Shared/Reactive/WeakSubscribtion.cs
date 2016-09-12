using System;
using System.Collections.Generic;
using System.Text;

namespace Loki.UI.Reactive
{
    public class WeakSubscription<T> : IDisposable, IObserver<T>
    {
        private readonly WeakReference reference;
        private readonly IDisposable subscription;
        private bool disposed;

        public WeakSubscription(IObservable<T> observable, IObserver<T> observer)
        {
            this.reference = new WeakReference(observer);
            this.subscription = observable.Subscribe(this);
        }

        void IObserver<T>.OnCompleted()
        {
            var observer = (IObserver<T>)this.reference.Target;
            if (observer != null) observer.OnCompleted();
            else this.Dispose();
        }

        void IObserver<T>.OnError(Exception error)
        {
            var observer = (IObserver<T>)this.reference.Target;
            if (observer != null) observer.OnError(error);
            else this.Dispose();
        }

        void IObserver<T>.OnNext(T value)
        {
            var observer = (IObserver<T>)this.reference.Target;
            if (observer != null) observer.OnNext(value);
            else this.Dispose();
        }

        public void Dispose()
        {
            if (this.disposed) return;

            this.disposed = true;
            this.subscription.Dispose();
        }
    }
}
