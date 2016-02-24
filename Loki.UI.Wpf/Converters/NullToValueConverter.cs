using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Loki.UI.Wpf.Converters
{
    public class NullToValueConverter<T> : IValueConverter
    {
        public T NullValue { get; set; }

        public T NotNullValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? this.NullValue : this.NotNullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}