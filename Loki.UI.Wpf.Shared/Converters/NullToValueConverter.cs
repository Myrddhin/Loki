namespace Loki.UI.Wpf.Converters
{
    public class NullToValueConverter<T> : PredicateToValueConverter<T>
    {
        protected override bool GetValue(object val)
        {
            return val == null;
        }
    }
}