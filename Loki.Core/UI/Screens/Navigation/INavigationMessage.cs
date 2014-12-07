using System;

namespace Loki.UI
{
    public interface INavigationMessage
    {
        Type NavigateTo { get; }

        bool Match(object existingViewModel);

        void Initialize(object newViewModel);
    }
}