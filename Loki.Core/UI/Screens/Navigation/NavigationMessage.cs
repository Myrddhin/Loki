using System;

namespace Loki.UI
{
    public class NavigationMessage<TViewModel> : INavigationMessage where TViewModel : class
    {
        public NavigationMessage()
        {
            Selector = v => v is TViewModel;
        }

        public Predicate<TViewModel> Selector { get; set; }

        public Action<TViewModel> Initializer { get; set; }

        public Type NavigateTo
        {
            get { return typeof(TViewModel); }
        }

        bool INavigationMessage.Match(object existingViewModel)
        {
            TViewModel vm = existingViewModel as TViewModel;
            if (vm == null || Selector == null)
            {
                return false;
            }
            else
            {
                return Selector(vm);
            }
        }

        void INavigationMessage.Initialize(object newViewModel)
        {
            TViewModel vm = newViewModel as TViewModel;
            if (vm != null && Initializer != null)
            {
                Initializer(vm);
            }
        }
    }
}