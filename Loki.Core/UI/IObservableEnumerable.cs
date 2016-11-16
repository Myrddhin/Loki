using System.Collections;
using System.Collections.Specialized;

namespace Loki.UI
{
    /// <summary>
    /// Represents a collection that is observable.
    /// </summary>
    public interface IObservableEnumerable : IEnumerable, INotifyCollectionChanged
    {
    }
}