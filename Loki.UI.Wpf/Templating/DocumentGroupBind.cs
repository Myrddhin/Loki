using System.Linq;
using System.Windows;
using System.Windows.Data;
using DevExpress.Xpf.Docking;

namespace Loki.UI.Wpf.Binds
{
    internal class DocumentGroupBind : FrameworkElementBind<DocumentGroup>
    {
        public DocumentGroupBind(DocumentGroup view, object viewModel)
            : base(view, viewModel)
        {
            IHaveActiveItem withActive = viewModel as IHaveActiveItem;
            if (withActive != null)
            {
                Binding binding = new Binding("ActiveItem");
                binding.Source = viewModel;
                binding.Mode = BindingMode.TwoWay;
                binding.NotifyOnSourceUpdated = true;
                binding.NotifyOnTargetUpdated = true;
                binding.Converter = new ActiveItemToActiveIndex();
                binding.ConverterParameter = viewModel;
                view.SetBinding(DocumentGroup.SelectedTabIndexProperty, binding);
            }
        }

        private class ActiveItemToActiveIndex : IValueConverter
        {
            public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                var source = parameter as IConductActiveItem;
                if (source != null)
                {
                    var index = source.Children.OfType<object>().ToList().IndexOf(value);
                    if (index >= 0)
                    {
                        return index;
                    }
                }

                return DependencyProperty.UnsetValue;
            }

            public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                var source = parameter as IConductActiveItem;
                if (source != null)
                {
                    var item = source.Children.OfType<object>().ToList();
                    if ((int)value < item.Count)
                    {
                        return item[(int)value];
                    }
                }

                return DependencyProperty.UnsetValue;
            }
        }
    }
}