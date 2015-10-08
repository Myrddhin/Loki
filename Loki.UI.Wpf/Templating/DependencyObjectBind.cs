using System.Windows;
using Loki.Common;

namespace Loki.UI.Wpf.Binds
{
    public class DependencyObjectBind<TComponent> : BaseObject where TComponent : DependencyObject
    {
        protected TComponent Component { get; private set; }

        protected object ViewModel { get; private set; }

        public DependencyObjectBind(TComponent component, object viewModel)
        {
            Component = component;
            ViewModel = viewModel;
        }
    }
}