using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using Loki.Common;
using Loki.Common.Diagnostics;
using Loki.UI.Commands;

namespace Loki.UI.Win
{
    public class Binder : BaseObject
    {
        public IValueConverter GlyphConverter { get; set; }

        public IValueConverter NameConverter { get; set; }

        private readonly IInfrastructure services;

        private readonly IThreadingContext context;

        public Binder(IInfrastructure services, IThreadingContext context) : base(services.Diagnostics)
        {
            this.services = services;
            this.context = context;
        }

        internal void BindCommandActivation(
            object component, 
            PropertyInfo property, 
            object bindingContext, 
            ICommand command, 
            object commandDefaultParameter = null)
        {
            // check in control property is editable
            if (property.CanWrite)
            {
                var valueSetter = PropertySetter(property);

                Func<object, Action> functor = c =>
                {
                    return () =>
                    {
                        var value = command.CanExecute(commandDefaultParameter);
                        valueSetter(c, value);
                    };
                };

                Action<object, object, EventArgs> handler = (c, sender, e) => context.OnUIThread(functor(c));

                // register in change manager service
                //services.Events.CanExecuteChanged.Register(command, component, handler);

                ICloseable closable = bindingContext as ICloseable;
                if (closable != null)
                {
                    //closable.Closing += (s, e) => services.Events.CanExecuteChanged.Unregister(command, component);
                }

                context.OnUIThread(functor(component));
            }
        }

        protected internal IConductor GetContainer<TModel>(Control control, Expression<Func<TModel, object>> propertyGetter) where TModel : class
        {
            TModel model = control.GetViewModel<TModel>();

            if (model == null)
            {
                Log.WarnFormat("Form {0} is not binded to a {1} model", control, model);
                return null;
            }

            var bindingTarget = propertyGetter.Compile()(model);

            var containerModel = bindingTarget as IConductor;
            if (containerModel == null)
            {
                Log.Warn("Navigation/Menu model must be a container");
                return null;
            }

            return containerModel;
        }

        protected internal TBinded GetBindedObject<TModel, TBinded>(Control control, Expression<Func<TModel, TBinded>> propertyGetter)
            where TModel : class
            where TBinded : class
        {
            TModel model = control.GetViewModel<TModel>();

            if (model == null)
            {
                Log.WarnFormat("Form {0} is not binded to a {1} model", control, model);
                return null;
            }

            var bindingTarget = propertyGetter.Compile()(model);

            var containerModel = bindingTarget;
            if (containerModel == null)
            {
                Log.WarnFormat("Binding is done on a wrong type {0} instead of {1}", bindingTarget.GetType(), typeof(TBinded));
                return null;
            }

            return containerModel;
        }

        public void BindName(object destination, PropertyInfo destinationProperty, object source)
        {
            INotifyPropertyChanged displayNameSource = source as INotifyPropertyChanged;
            if (displayNameSource != null)
            {
                OneWay(destination, destinationProperty, displayNameSource, ExpressionHelper.GetProperty<IHaveDisplayName, string>(x => x.DisplayName), NameConverter);
                PropertySetter(destinationProperty)(destination, GetName(source));
            }
        }

        public void OneWay(
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
                Func<object, Action> eventFunctor = CreateGetValueAndSetFunctor(source, sourceProperty, converter, converterParameter, destinationProperty);

                // register in change manager service
                //services.Events.PropertyChanged.Register(source, sourceProperty.Name, destination, (c, sender, e) => context.OnUIThread(eventFunctor(c)));

                var valueGetter = PropertyGetter(sourceProperty);
                var valueSetter = PropertySetter(destinationProperty);
                if (converter == null)
                {
                    valueSetter(destination, valueGetter(source));
                }
                else
                {
                    valueSetter(destination, converter.Convert(valueGetter(source), destinationProperty.PropertyType, converterParameter, Thread.CurrentThread.CurrentUICulture));
                }
            }
        }

        public Action<object, object> PropertySetter(PropertyInfo property)
        {
            var parameter = Expression.Parameter(typeof(object));
            var convertedParameter = Expression.TypeAs(parameter, property.DeclaringType);
            var value = Expression.Parameter(typeof(object));
            var convertedValue = Expression.Convert(value, property.PropertyType);

            var propSet = Expression.Property(convertedParameter, property);
            var assign = Expression.Assign(propSet, convertedValue);

            return Expression.Lambda<Action<object, object>>(assign, parameter, value).Compile();
        }

        public Func<object, object> PropertyGetter(PropertyInfo property)
        {
            var parameter = Expression.Parameter(typeof(object));
            var convertedParameter = Expression.Convert(parameter, property.DeclaringType);
            var propGet = Expression.Property(convertedParameter, property);
            var boxed = Expression.Convert(propGet, typeof(object));

            return Expression.Lambda<Func<object, object>>(boxed, parameter).Compile();
        }

        public Func<object, Action> CreateGetValueAndSetFunctor(object source, PropertyInfo sourceProperty, IValueConverter converter, object converterParameter, PropertyInfo propertyToSetDescriptor)
        {
            var valueGetter = PropertyGetter(sourceProperty);
            var valueSetter = PropertySetter(propertyToSetDescriptor);

            Func<object, Action> eventFunctor = delegate (object c)
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
                        value = converter.Convert(valueGetter(source), propertyToSetDescriptor.PropertyType, converterParameter, Thread.CurrentThread.CurrentUICulture);
                    }

                    valueSetter(c, value);
                };
            };

            return eventFunctor;
        }

        public string GetName(object item)
        {
            var display = item as IHaveDisplayName;
            if (display != null)
            {
                if (NameConverter == null)
                {
                    return display.DisplayName;
                }

                return this.NameConverter.Convert(display.DisplayName, typeof(string), null, Thread.CurrentThread.CurrentUICulture).SafeToString();
            }

            return string.Empty;
        }

        public void TwoWay(
            object destination, 
            PropertyInfo destinationProperty, 
            INotifyPropertyChanged source, 
            PropertyInfo sourceProperty, 
            DataSourceUpdateMode mode = DataSourceUpdateMode.OnValidation, 
            IValueConverter converter = null, 
            object converterParameter = null)
        {
            // check in control property is editable
            if (destinationProperty.CanWrite)
            {
                Func<object, Action> eventFunctor = CreateGetValueAndSetFunctor(source, sourceProperty, converter, converterParameter, destinationProperty);

                // register in change manager service
                //services.Events.PropertyChanged.Register(source, sourceProperty.Name, destination, (c, sender, e) => context.OnUIThread(eventFunctor(c)));
            }

            if (sourceProperty.CanWrite && mode != DataSourceUpdateMode.Never)
            {
                // wrap to catch property changed event
                NotifyPropertyChangedWrapper wrapper = new NotifyPropertyChangedWrapper(destinationProperty);

                // select event to wrap witch DataSourceUpdateMode
                if (mode == DataSourceUpdateMode.OnPropertyChanged)
                {
                    EventInfo propertyChangedEvent = destination.GetType().GetEvent(destinationProperty.Name + "Changed");
                    if (propertyChangedEvent == null)
                    {
                        Log.ErrorFormat("Type {0} has not {1}Changed event", destinationProperty.DeclaringType, destinationProperty.Name);
                    }
                    else
                    {
                        Delegate handler = Delegate.CreateDelegate(propertyChangedEvent.EventHandlerType, wrapper, "EventBridge");
                        propertyChangedEvent.AddEventHandler(destination, handler);
                    }
                }
                else if (mode == DataSourceUpdateMode.OnValidation)
                {
                    EventInfo propertyChangedEvent = destination.GetType().GetEvent("Validated");
                    if (propertyChangedEvent == null)
                    {
                        Log.ErrorFormat("Type {0} has not Validated event", destinationProperty.DeclaringType);
                    }
                    else
                    {
                        Delegate handler = Delegate.CreateDelegate(propertyChangedEvent.EventHandlerType, wrapper, "EventBridge");
                        propertyChangedEvent.AddEventHandler(destination, handler);
                    }
                }

                // Create getter
                Func<object, object> valueGetter = PropertyGetter(destinationProperty);

                //services.Events.PropertyChanged.Register(
                //    wrapper, 
                //    destinationProperty.Name, 
                //    source, 
                //    delegate (object c, object s, PropertyChangedEventArgs e)
                //    {
                //        if (converter == null)
                //        {
                //            sourceProperty.SetValue(c, valueGetter(destination), null);
                //        }
                //        else
                //        {
                //            sourceProperty.SetValue(c, converter.ConvertBack(valueGetter(destination), sourceProperty.PropertyType, converterParameter, Application.CurrentCulture), null);
                //        }
                //    });
            }

            var valueInitGetter = PropertyGetter(sourceProperty);
            var valueInitSetter = PropertySetter(destinationProperty);

            if (converter == null)
            {
                valueInitSetter(destination, valueInitGetter(source));
            }
            else
            {
                valueInitSetter(destination, converter.Convert(valueInitGetter(source), destinationProperty.PropertyType, converterParameter, Thread.CurrentThread.CurrentUICulture));
            }
        }
    }
}