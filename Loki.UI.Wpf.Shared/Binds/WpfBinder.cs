using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Loki.UI.Wpf.Binds
{
   public class WpfBinder : IBinder
    {
       public SortedDictionary<Predicate<object>, Func<object, object, object>> Bindings()
       {
           throw new NotImplementedException();
       }

       public object CreateBind(object component, object model)
       {
           var window = component as Window;
           if (window != null)
           {
               return new WindowBind(null, window, model);
           }

           return null;
       }
    }
}
