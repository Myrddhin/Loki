using System;
using System.Globalization;

namespace Loki.UI.Wpf.Converters
{
    public class BoolToValueConverter<T> : PredicateToValueConverter<T>
    {
        protected override bool GetValue(object val)
        {
            if (val is bool)
            {
                return (bool)val;
            }

            return false;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.Equals(this.TrueValue);
        }
    }
}