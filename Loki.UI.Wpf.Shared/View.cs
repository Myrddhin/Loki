using System;
using System.ComponentModel;
using System.Windows;

namespace Loki.UI
{
    public class View
    {
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
    }
}