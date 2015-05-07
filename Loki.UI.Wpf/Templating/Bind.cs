using System.Windows;
using Loki.Common;

namespace Loki.UI.Wpf
{
    /// <summary>
    ///   Hosts dependency properties for binding.
    /// </summary>
    public static class Bind
    {
        /// <summary>
        ///   Allows binding on an existing view. Use this on root UserControls, Pages and Windows; not in a DataTemplate.
        /// </summary>
        public static DependencyProperty ModelProperty =
            DependencyProperty.RegisterAttached(
                "Model",
                typeof(object),
                typeof(Bind),
                new PropertyMetadata(null, ModelChanged));

        /// <summary>
        ///   Gets the model to bind to.
        /// </summary>
        /// <param name = "dependencyObject">The dependency object to bind to.</param>
        /// <returns>The model.</returns>
        public static object GetModel(DependencyObject dependencyObject)
        {
            return dependencyObject.GetValue(ModelProperty);
        }

        /// <summary>
        ///   Sets the model to bind to.
        /// </summary>
        /// <param name = "dependencyObject">The dependency object to bind to.</param>
        /// <param name = "value">The model.</param>
        public static void SetModel(DependencyObject dependencyObject, object value)
        {
            dependencyObject.SetValue(ModelProperty, value);
        }

        private static void ModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (Toolkit.UI.Windows.DesignMode || e.NewValue == null || e.NewValue == e.OldValue)
            {
                return;
            }

            var fe = d as FrameworkElement;
            if (fe == null)
            {
                var de = d as ContentElement;
                if (de != null)
                {
                    Toolkit.UI.Templating.CreateBind(d, e.NewValue);
                }

                return;
            }

            View.ExecuteOnLoad(
                fe,
                delegate
                {
                    Toolkit.UI.Templating.CreateBind(d, e.NewValue);
                });
        }
    }
}