using System.Windows;

using Loki.Common;

namespace Loki.UI.Wpf.Binds
{
    public class FrameworkElementBind<TComponent> : DependencyObjectBind<TComponent> where TComponent : FrameworkElement
    {
        public FrameworkElementBind(ICoreServices services, IThreadingContext threading, TComponent component, object viewModel)
            : base(services, threading, component, viewModel)
        {
            if (Component.DataContext == null)
            {
                Component.Dispatcher.Invoke(() => Component.SetValue(FrameworkElement.DataContextProperty, viewModel));
            }

            var loadable = viewModel as ILoadable;

            if (loadable != null)
            {
                View.ExecuteOnLoad(component, (s, e) => loadable.Load());
            }
        }
    }
}