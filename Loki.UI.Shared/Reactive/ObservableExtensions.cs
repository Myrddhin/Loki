using Loki.UI.Reactive;

namespace System
{
    public static class ObservableExtensions
    {
        public static IDisposable WeakSubscribe<T>(this IObservable<T> observable, IObserver<T> observer)
        {
            return new WeakSubscription<T>(observable, observer);
        }
    }
}