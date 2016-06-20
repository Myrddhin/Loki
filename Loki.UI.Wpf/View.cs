using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

using Loki.Common;

namespace Loki.UI.Wpf
{
    /// <summary>
    /// Hosts attached properties related to view models.
    /// </summary>
    public static class View
    {
        private static readonly ContentPropertyAttribute DefaultContentProperty = new ContentPropertyAttribute("Content");

        private static ITemplatingEngine engine;

        private static ILog log;

        private static IMessageBus bus;

        public static IMessageBus MessageBus
        {
            get
            {
                return bus;
            }
        }

        public static void InitializeViewHelper(ITemplatingEngine templateEngine, ILog logger, IMessageBus messageBus)
        {
            log = logger;
            engine = templateEngine;
            bus = messageBus;
        }

        /// <summary>
        /// A dependency property for attaching a model to the UI.
        /// </summary>
        public static DependencyProperty ModelProperty =
            DependencyProperty.RegisterAttached(
                "Model",
                typeof(object),
                typeof(View),
                new PropertyMetadata(null, OnModelChanged));

        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="d">
        /// The element to attach the model to.
        /// </param>
        /// <param name="value">
        /// The model.
        /// </param>
        public static void SetModel(DependencyObject d, object value)
        {
            d.SetValue(ModelProperty, value);
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <param name="d">
        /// The element the model is attached to.
        /// </param>
        /// <returns>
        /// The model.
        /// </returns>
        public static object GetModel(DependencyObject d)
        {
            return d.GetValue(ModelProperty);
        }

        /// <summary>
        /// Executes the handler immediately if the element is loaded, otherwise wires it to the Loaded event.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <returns>
        /// True if the handler was executed immediately; false otherwise.
        /// </returns>
        public static bool ExecuteOnLoad(FrameworkElement element, EventHandler handler)
        {
            if (element.IsLoaded)
            {
                handler(element, new RoutedEventArgs());
                return true;
            }

            RoutedEventHandler loaded = null;
            loaded = (s, e) =>
            {
                element.Loaded -= loaded;
                handler(s, e);
            };

            EventHandler contentLoader = null;
            contentLoader = (s, e) =>
            {
                ((Window)element).ContentRendered -= contentLoader;
                handler(s, e);
            };

            var window = element as Window;

            if (window != null)
            {
                window.ContentRendered += contentLoader;
            }
            else
            {
                element.Loaded += loaded;
            }

            return false;
        }

        /// <summary>
        /// Executes the handler when the element is unloaded.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        public static void ExecuteOnUnload(FrameworkElement element, RoutedEventHandler handler)
        {
            RoutedEventHandler unloaded = null;
            unloaded = (s, e) =>
            {
                element.Unloaded -= unloaded;
                handler(s, e);
            };

            element.Unloaded += unloaded;
        }

        /// <summary>
        /// Executes the handler the next time the elements's LayoutUpdated event fires.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        public static void ExecuteOnLayoutUpdated(FrameworkElement element, EventHandler handler)
        {
            EventHandler onLayoutUpdate = null;
            onLayoutUpdate = (s, e) =>
            {
                element.LayoutUpdated -= onLayoutUpdate;
                handler(s, e);
            };

            element.LayoutUpdated += onLayoutUpdate;
        }

        private static void OnModelChanged(DependencyObject targetLocation, DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue == args.NewValue || DesignMode)
            {
                return;
            }

            if (args.NewValue != null)
            {
                // get template
                var view = engine.GetTemplate(args.NewValue);

                // replace content property
                SetContentProperty(targetLocation, view);

                // create bind
                engine.CreateBind(view, args.NewValue);
            }
            else
            {
                SetContentProperty(targetLocation, args.NewValue);
            }
        }

        private static void SetContentProperty(object targetLocation, object view)
        {
            var fe = view as FrameworkElement;
            if (fe != null && fe.Parent != null)
            {
                SetContentPropertyCore(fe.Parent, null);
            }

            SetContentPropertyCore(targetLocation, view);
        }

        private static void SetContentPropertyCore(object targetLocation, object view)
        {
            try
            {
                var type = targetLocation.GetType();
                var contentProperty = type.GetAttributes<ContentPropertyAttribute>(true)
                                          .FirstOrDefault() ?? DefaultContentProperty;

                type.GetProperty(contentProperty.Name)
                    .SetValue(targetLocation, view, null);
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
            }
        }

        private static bool? inDesignMode;

        public static bool DesignMode
        {
            get
            {
                if (inDesignMode != null)
                {
                    return inDesignMode.GetValueOrDefault(false);
                }

                var descriptor = DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement));
                inDesignMode = (bool)descriptor.Metadata.DefaultValue;

                return inDesignMode.GetValueOrDefault(false);
            }
        }
    }
}