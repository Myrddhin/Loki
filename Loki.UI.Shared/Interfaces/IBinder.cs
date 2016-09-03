using System;
using System.Collections.Generic;

namespace Loki.UI
{
    public interface IBinder
    {
        SortedDictionary<Predicate<object>, Func<object, object, object>> Bindings();

        object CreateBind(object component, object model);
    }
}