using System;

namespace Loki.UI.Navigation
{
    public interface INavigator
    {
        void NavigateTo(Uri route, object parameters);
    }
}