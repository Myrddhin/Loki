using System.Windows;

namespace Loki.UI.Office
{
    /// <summary>
    ///   Hosts dependency properties for binding.
    /// </summary>
    public static class Bind
    {
        private static ITemplatingEngine engine;

        public static void InitializeEngine(ITemplatingEngine templatingEngine)
        {
            engine = templatingEngine;
        }

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
        /// Gets the model to bind to.
        /// </summary>
        /// <param name="dependencyObject">
        /// The dependency object to bind to.
        /// </param>
        /// <returns>
        /// The model.
        /// </returns>
        public static object GetModel(DependencyObject dependencyObject)
        {
            return dependencyObject.GetValue(ModelProperty);
        }

        /// <summary>
        /// Sets the model to bind to.
        /// </summary>
        /// <param name="dependencyObject">
        /// The dependency object to bind to.
        /// </param>
        /// <param name="value">
        /// The model.
        /// </param>
        public static void SetModel(DependencyObject dependencyObject, object value)
        {
            dependencyObject.SetValue(ModelProperty, value);
        }

        private static void ModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (View.DesignMode || e.NewValue == null || e.NewValue == e.OldValue)
            {
                return;
            }

            var fe = d as FrameworkElement;
            if (fe == null)
            {
                var de = d as ContentElement;
                if (de != null)
                {
                    engine.CreateBind(d, e.NewValue);
                }

                return;
            }

            View.ExecuteOnLoad(
                fe,
                delegate
                {
                    engine.CreateBind(d, e.NewValue);
                });
        }
    }
}