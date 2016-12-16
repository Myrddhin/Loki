using System;
using System.ComponentModel;

using Loki.UI.Models;

namespace Loki.UI.Navigation
{
    public interface INavigationHost : INotifyPropertyChanged
    {
        Screen ActiveItem { get; set; }

        IObservableCollection<Screen> Items { get; }

        void HandleRoute<T>(Uri route, Func<T, object, bool> matcher, Action<T, object> initializer)
            where T : Screen;
    }
}