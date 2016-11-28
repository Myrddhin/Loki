using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Loki.UI.Wpf.Converters
{
    public abstract class PredicateToValueConverter<T> : IValueConverter
    {
        public T TrueValue { get; set; }

        public T FalseValue { get; set; }

        protected abstract bool GetValue(object val);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetValue((T)value) ? TrueValue : FalseValue;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}