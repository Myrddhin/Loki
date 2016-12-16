using System.Windows;

using Loki.Common;
using Loki.Common.Diagnostics;

namespace Loki.UI.Wpf.Binds
{
    public class DependencyObjectBind<TComponent> : BaseObject
        where TComponent : DependencyObject
    {
        protected TComponent Component { get; private set; }

        protected object ViewModel { get; private set; }

        protected IDiagnostics Diagnostics { get; private set; }

        public DependencyObjectBind(IDiagnostics diagnostics, TComponent component, object viewModel)
            : base(diagnostics)
        {
            Diagnostics = diagnostics;
            Component = component;
            ViewModel = viewModel;
        }
    }
}