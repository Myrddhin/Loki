using System;

namespace Loki.UI
{
    public class NavigationMessage<TViewModel> : INavigationMessage where TViewModel : class
    {
        public NavigationMessage()
        {
            Selector = v => v is TViewModel;
        }

        public string FunctionName { get; set; }

        public Predicate<TViewModel> Selector { get; set; }

        public Action<TViewModel> Initializer { get; set; }

        public Type NavigateTo
        {
            get { return typeof(TViewModel); }
        }

        public bool Match(TViewModel vm)
        {
            if (vm == null || Selector == null)
            {
                return false;
            }

            return this.Selector(vm);
        }

        public void Initialize(TViewModel vm)
        {
            if (vm != null && Initializer != null)
            {
                Initializer(vm);
            }
        }

        bool INavigationMessage.Match(object existingViewModel)
        {
            TViewModel vm = existingViewModel as TViewModel;
            return this.Match(vm);
        }

        void INavigationMessage.Initialize(object newViewModel)
        {
            TViewModel vm = newViewModel as TViewModel;
            this.Initialize(vm);
        }
    }
}