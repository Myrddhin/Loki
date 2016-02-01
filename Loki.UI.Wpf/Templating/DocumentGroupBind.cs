using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

using DevExpress.Xpf.Docking;

using Loki.Common;

namespace Loki.UI.Wpf.Binds
{
    internal class DocumentGroupBind : FrameworkElementBind<DocumentGroup>
    {
        public DocumentGroupBind(ICoreServices services, IThreadingContext threading, DocumentGroup view, object viewModel)
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
            binding.NotifyOnTargetUpdated = true;
            binding.Converter = new ActiveItemToActiveIndex();
            binding.ConverterParameter = viewModel;
            view.SetBinding(LayoutGroup.SelectedTabIndexProperty, binding);
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
                if ((int)value < item.Count)
                {
                    return item[(int)value];
                }

                return DependencyProperty.UnsetValue;
            }
        }
    }
}