using System;
using System.Windows.Data;
using Loki.Common;

namespace Loki.UI.Wpf.Converters
{
    public class DisplayNameWithStateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length > 0)
            {
                string name = values[0].SafeToString();
                string appender = string.Empty;

                if (values.Length > 1 && values[1] is bool)
                {
                    appender = ((bool)values[1]) ? "*" : string.Empty;
                }

                return string.Format("{0} {1}", name, appender).Trim();
            }
            else
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}