using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using DevExpress.Xpf.Core;

using Loki.Common;
using Loki.UI.Wpf.Binds;

namespace Loki.UI.Wpf.Templating.DevExpress
{
    internal class TabControlBind : FrameworkElementBind<DXTabControl>
    {
        public TabControlBind(
            ICoreServices services,
            IThreadingContext threading,
            DXTabControl view,
            object viewModel)
            : base(services, threading, view, viewModel)
        {
            IHaveActiveItem withActive = viewModel as IHaveActiveItem;
            if (withActive == null)
            {
                return;
            }

            Binding binding = new Binding("ActiveItem");
            binding.Source = viewModel;
            binding.Mode = BindingMode.TwoWay;
            binding.NotifyOnSourceUpdated = true;
            binding.Converter = new ActiveItemToActiveIndex();
            binding.ConverterParameter = viewModel;
            view.SetBinding(DXTabControl.SelectedIndexProperty, binding);

            IConductor container = viewModel as IConductor;

            if (container == null)
            {
                return;
            }

            Binding sourceBinding = new Binding("Children");
            sourceBinding.Source = container;
            sourceBinding.Mode = BindingMode.OneWay;
            sourceBinding.NotifyOnSourceUpdated = true;
            sourceBinding.NotifyOnTargetUpdated = true;

            view.SetBinding(ItemsControl.ItemsSourceProperty, sourceBinding);
        }

        private class ActiveItemToActiveIndex : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var source = parameter as IConductActiveItem;
                if (source == null)
                {
                    return DependencyProperty.UnsetValue;
                }

                var index = source.Children.OfType<object>().ToList().IndexOf(value);

                return index >= 0 ? index : DependencyProperty.UnsetValue;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var source = parameter as IConductActiveItem;
                if (source == null)
                {
                    return DependencyProperty.UnsetValue;
                }

                var item = source.Children.OfType<object>().ToList();
                var index = (int)value;
                if (index < item.Count && index >= 0)
                {
                    return item[index];
                }

                return DependencyProperty.UnsetValue;
            }
        }
    }
}