using System.Windows;

using Loki.Common;

namespace Loki.UI.Office.Binds
{
    public class DependencyObjectBind<TComponent> : LoggableObject where TComponent : DependencyObject
    {
        protected TComponent Component { get; private set; }

        protected object ViewModel { get; private set; }

        protected ICoreServices Services { get; private set; }

        protected IThreadingContext Threading { get; private set; }

        public DependencyObjectBind(ICoreServices services, IThreadingContext threading, TComponent component, object viewModel) : base(services.Logger)
        {
            Services = services;
            Threading = threading;
            Component = component;
            ViewModel = viewModel;
        }
    }
}