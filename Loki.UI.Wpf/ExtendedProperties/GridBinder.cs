using System.Collections.Generic;
using System.Windows;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Grid;
using Loki.Common;

namespace Loki.UI.Wpf
{
    public static class GridBinder
    {
        /// <summary>
        /// A dependency property for attaching a model to the UI.
        /// </summary>
        public static DependencyProperty RowCommandsProperty =
            DependencyProperty.RegisterAttached(
                "RowCommands",
                typeof(object),
                typeof(GridBinder),
                new PropertyMetadata(null, RowCommandsChanged));

        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="d">The element to attach the model to.</param>
        /// <param name="value">The model.</param>
        public static void SetRowCommands(DependencyObject d, object value)
        {
            d.SetValue(RowCommandsProperty, value);
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <param name="d">The element the model is attached to.</param>
        /// <returns>The model.</returns>
        public static object GetRowCommands(DependencyObject d)
        {
            return d.GetValue(RowCommandsProperty);
        }

        private static void RowCommandsChanged(DependencyObject targetLocation, DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue == args.NewValue || Toolkit.UI.Windows.DesignMode)
            {
                return;
            }

            var commands = args.NewValue as IEnumerable<CommandElement>;
            var grid = targetLocation as GridControl;

            if (commands != null && grid != null)
            {
                List<BarButtonItem> items = new List<BarButtonItem>();

                DataViewBase view = grid.View as DataViewBase;
                foreach (var comm in commands)
                {
                    BarButtonItem item = new BarButtonItem();
                    item.Name = "row_" + comm.DisplayName;
                    item.Content = comm.DisplayName;
                    item.CommandParameter = grid.SelectedItems;
                    if (grid.SelectionMode == MultiSelectMode.None)
                    {
                        grid.SelectionMode = MultiSelectMode.Row;
                    }

                    item.IsEnabled = comm.Command.CanExecute(item.CommandParameter);
                    item.Command = comm.Command;
                    grid.SelectionChanged += (s, e) =>
                        {
                            item.CommandParameter = grid.SelectedItems;
                            item.IsEnabled = comm.Command.CanExecute(item.CommandParameter);
                        };
                    items.Add(item);
                }

                view.ShowGridMenu += (s, e) =>
                    {
                        foreach (var item in items)
                        {
                            e.Customizations.Add(item);
                        }
                    };
            }
        }
    }
}