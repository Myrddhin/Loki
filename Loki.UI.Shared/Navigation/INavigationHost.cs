using System;
using System.Collections.Generic;
using System.Text;

using Loki.UI.Models;

namespace Loki.UI.Navigation
{
    public interface INavigationHost
    {
        Screen ActiveItem { get; set; }

        IObservableCollection<Screen> Items { get; }
    }
}
