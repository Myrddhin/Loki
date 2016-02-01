using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Grid;

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

        public static DependencyProperty DblClickCommandProperty =
            DependencyProperty.RegisterAttached(
                "DblClickCommand",
                typeof(object),
                typeof(GridBinder),
                new PropertyMetadata(null, DblClickCommandChanged));

        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="d">
        /// The element to attach the model to.
        /// </param>
        /// <param name="value">
        /// The model.
        /// </param>
        public static void SetRowCommands(DependencyObject d, object value)
        {
            d.SetValue(RowCommandsProperty, value);
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
        public static object GetRowCommands(DependencyObject d)
        {
            return d.GetValue(RowCommandsProperty);
        }

        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="d">
        /// The element to attach the model to.
        /// </param>
        /// <param name="value">
        /// The model.
        /// </param>
        public static void SetDblClickCommand(DependencyObject d, object value)
        {
            d.SetValue(DblClickCommandProperty, value);
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
        public static object GetDblClickCommand(DependencyObject d)
        {
            return d.GetValue(DblClickCommandProperty);
        }

        private static void RowCommandsChanged(DependencyObject targetLocation, DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue == args.NewValue || View.DesignMode)
            {
                return;
            }

            var commands = args.NewValue as IEnumerable<CommandElement>;
            var grid = targetLocation as GridControl;

            if (commands == null || grid == null)
            {
                return;
            }

            List<BarButtonItem> items = new List<BarButtonItem>();

            DataViewBase view = grid.View;
            foreach (var comm in commands)
            {
                BarButtonItem item = new BarButtonItem();

                // item.Name = "row_" + comm.Command.Name;
                item.Content = comm.DisplayName;
                item.CommandParameter = grid.SelectedItems;
                if (grid.SelectionMode == MultiSelectMode.None)
                {
                    grid.SelectionMode = MultiSelectMode.MultipleRow;
                }

                item.IsEnabled = comm.Command.CanExecute(item.CommandParameter);
                item.Command = comm.Command;
                var comm1 = comm;

                grid.SelectionChanged += (s, e) =>
                    {
                        item.CommandParameter = grid.SelectedItems;
                        item.IsEnabled = comm1.Command.CanExecute(item.CommandParameter);
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

        private static void DblClickCommandChanged(DependencyObject targetLocation,
            DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue == args.NewValue || View.DesignMode)
            {
                return;
            }

            var commands = args.NewValue as ICommand;
            var grid = targetLocation as GridControl;
            if (grid == null || commands == null)
            {
                return;
            }

            var tableView = grid.View as TableView;
            if (tableView != null)
            {
                tableView.RowDoubleClick += (s, e) =>
                {
                    var info = e.HitInfo;
                    if (!info.InRow)
                    {
                        return;
                    }

                    var row = tableView.GetRowElementByRowHandle(info.RowHandle);
                    if (row != null)
                    {
                        commands.Execute(row);
                    }
                };
            }
        }
    }
}