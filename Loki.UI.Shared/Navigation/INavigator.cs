using Loki.UI;
using System;
using System.Collections.Generic;
using System.Text;

using Loki.UI.Models;

namespace Loki.UI.Navigation
{
   public  interface INavigator
    {
        void RegisterHost(INavigationHost host);

        void AddRoute<T>(Uri route) where T : Screen;

        void AddRoute<T>(Uri route, Func<T, NavigationMessage, bool> selector);
    }

   
}
