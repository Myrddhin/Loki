using System.ComponentModel;
using System.Reflection;

using Loki.Common;
using Loki.UI.Commands;

namespace Loki.UI.Win
{
    public class ComponentBind<TComponent> : BaseObject
           where TComponent : Component
    {
        private readonly Binder binder;

        protected TComponent Component { get; }

        protected IInfrastructure Services { get; private set; }

        protected IThreadingContext Context { get; private set; }

        protected object ViewModel { get; }

        public ComponentBind(IInfrastructure services, IThreadingContext context, TComponent component, object viewModel) : base(services.Diagnostics)
        {
            binder = new Binder(services, context);
            Services = services;
            Component = component;
            Context = context;
            ViewModel = viewModel;
        }

        public IValueConverter GlyphConverter { get; set; }

        public IValueConverter NameConverter { get; set; }

        protected void BindName(PropertyInfo destinationProperty)
        {
            binder.BindName(Component, destinationProperty, ViewModel);
        }

        protected void BindCommandActivation(
            object component,
           PropertyInfo property,
           object bindingContext,
           ICommand command,
           object commandDefaultParameter = null)
        {
            binder.BindCommandActivation(component, property, bindingContext, command, commandDefaultParameter);
        }

        protected void OneWay(
                            object destination,
                            PropertyInfo destinationProperty,
                            INotifyPropertyChanged source,
                            PropertyInfo sourceProperty,
                            IValueConverter converter = null,
                            object converterParameter = null)
        {
            binder.OneWay(destination, destinationProperty, source, sourceProperty, converter, converterParameter);
        }

        protected string GetName(object item)
        {
            return binder.GetName(item);
        }
    }
}