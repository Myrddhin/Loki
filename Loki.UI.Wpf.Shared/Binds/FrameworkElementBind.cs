using System.Windows;

using Loki.Common;
using Loki.UI.Models;
using Loki.Common.Diagnostics;

namespace Loki.UI.Wpf.Binds
{
    public class FrameworkElementBind<TComponent> : DependencyObjectBind<TComponent> where TComponent : FrameworkElement
    {
        public FrameworkElementBind(IDiagnostics diagnostics, TComponent component, object viewModel)
            : base(diagnostics,  component, viewModel)
        {
            if (Component.DataContext != viewModel)
            {
                Component.Dispatcher.Invoke(() => Component.DataContext = viewModel);
            }

            var loadable = viewModel as ILoadable;

            if (loadable != null)
            {
                View.ExecuteOnLoad(component, (s, e) => loadable.Load());
            }

            component.Unloaded += Component_Unloaded;
        }

        private void Component_Unloaded(object sender, RoutedEventArgs e)
        {
            this.DoCleanup();
        }

        protected virtual void DoCleanup()
        {
        }
    }
}