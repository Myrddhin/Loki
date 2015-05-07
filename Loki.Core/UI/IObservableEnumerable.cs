using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loki.UI
{
    /// <summary>
    /// Represents a collection that is observable.
    /// </summary>
    public interface IObservableEnumerable : IEnumerable, INotifyCollectionChanged
    {
    }
}