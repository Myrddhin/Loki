using System.ComponentModel;
using System.Reflection;

using Loki.Common;
using Loki.UI.Commands;

namespace Loki.UI.Win
{
    public class ComponentBind<TComponent> : LoggableObject
           where TComponent : Component
    {
        private readonly Binder binder = new Binder();

        protected TComponent Component { get; }

        protected object ViewModel { get; }

        public ComponentBind(TComponent component, object viewModel)
        {
            Component = component;
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