using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Loki.Common;

namespace Loki.UI.Win
{
    public class ComponentBind<TComponent> : BaseObject
    {
        protected TComponent Component { get; private set; }

        protected object ViewModel { get; private set; }

        public ComponentBind(TComponent component, object viewModel)
        {
            Component = component;
            ViewModel = viewModel;
        }

        public IValueConverter GlyphConverter { get; set; }

        public IValueConverter NameConverter { get; set; }

        protected void BindName(PropertyInfo destinationProperty)
        {
            INotifyPropertyChanged displayNameSource = ViewModel as INotifyPropertyChanged;
            if (displayNameSource != null)
            {
                OneWay(Component, destinationProperty, displayNameSource, ExpressionHelper.GetProperty<IHaveDisplayName, string>(x => x.DisplayName), NameConverter);
                PropertySetter(destinationProperty)(Component, GetName(ViewModel));
            }
        }

        protected void OneWay(
                            object destination,
                            PropertyInfo destinationProperty,
                            INotifyPropertyChanged source,
                            PropertyInfo sourceProperty,
                            IValueConverter converter = null,
                            object converterParameter = null)
        {
            // check in control property is editable
            if (destinationProperty.CanWrite)
            {
                // get source property name
                Func<object, Action> eventFunctor = CreateValueToControlSetter(source, sourceProperty, converter, converterParameter, destinationProperty);

                // register in change manager service
                Toolkit.Common.Events.PropertyChanged.Register(source, sourceProperty.Name, destination, (context, sender, e) => Toolkit.UI.Threading.OnUIThread(eventFunctor(context)));
            }
        }

        private static Action<object, object> PropertySetter(PropertyInfo property)
        {
            var parameter = Expression.Parameter(typeof(object));
            var convertedParameter = Expression.TypeAs(parameter, property.DeclaringType);
            var value = Expression.Parameter(typeof(object));
            var convertedValue = Expression.TypeAs(value, property.PropertyType);

            var propSet = Expression.Property(convertedParameter, property);
            var assign = Expression.Assign(propSet, convertedValue);

            return Expression.Lambda<Action<object, object>>(assign, parameter, value).Compile();
        }

        private static Func<object, object> PropertyGetter(PropertyInfo property)
        {
            var parameter = Expression.Parameter(typeof(object));
            var convertedParameter = Expression.TypeAs(parameter, property.DeclaringType);
            var propGet = Expression.Property(convertedParameter, property);

            return Expression.Lambda<Func<object, object>>(propGet, parameter).Compile();
        }

        private Func<object, Action> CreateValueToControlSetter(object source, PropertyInfo sourceProperty, IValueConverter converter, object converterParameter, PropertyInfo propertyToSetDescriptor)
        {
            var valueGetter = PropertyGetter(sourceProperty);
            var valueSetter = PropertySetter(propertyToSetDescriptor);

            Func<object, Action> eventFunctor = delegate(object c)
            {
                return () =>
                {
                    object value = null;
                    if (converter == null)
                    {
                        value = valueGetter(source);
                    }
                    else
                    {
                        value = converter.Convert(valueGetter(source), propertyToSetDescriptor.PropertyType, converterParameter, Toolkit.UI.Windows.Culture);
                    }

                    valueSetter(c, value);
                };
            };

            return eventFunctor;
        }

        protected string GetName(object item)
        {
            var display = item as IHaveDisplayName;
            if (display != null)
            {
                if (NameConverter == null)
                {
                    return display.DisplayName;
                }
                else
                {
                    return NameConverter.Convert(display.DisplayName, typeof(string), null, Toolkit.UI.Windows.Culture).SafeToString();
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }
}