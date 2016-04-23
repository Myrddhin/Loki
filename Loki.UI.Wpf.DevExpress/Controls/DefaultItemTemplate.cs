using System.Windows;
using System.Windows.Markup;

namespace Loki.UI.Wpf.DevExpress.Controls
{
    public static class DefaultTemplates
    {
        public static DataTemplate DefaultItemTemplate
        {
            get
            {
                return (DataTemplate)
                       XamlReader.Parse(
                       "<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' "
                       +
                       "xmlns:loki='clr-namespace:Loki.UI.Wpf;assembly=Loki.UI.Wpf'> "
                       +
                       "<loki:LoadingDecorator DisplayText=\"{Binding Status}\" IsLoading=\"{Binding IsBusy}\"><ContentControl loki:View.Model=\"{Binding}\" Background=\"Transparent\" VerticalContentAlignment=\"Stretch\" HorizontalContentAlignment=\"Stretch\" IsTabStop=\"False\" />"
                       +
                       "</loki:LoadingDecorator></DataTemplate>");
            }
        }
    }
}