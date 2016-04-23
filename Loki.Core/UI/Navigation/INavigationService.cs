using System;

namespace Loki.UI
{
    public interface INavigationService
    {
        void Navigate(string route);

        void AddRoute(string route, Func<INavigationMessage> message);
    }
}