using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Loki.UI.Wpf.Infragistics.Controls
{
    public static class DefaultTemplates
    {
        public static DataTemplate DefaultItemTemplate =>
             (DataTemplate)XamlReader.Parse(
                "<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' "
                +
                "xmlns:loki='clr-namespace:Loki.UI.Wpf;assembly=Loki.UI.Wpf'> "
                +
                "<ContentControl loki:View.Model=\"{Binding}\" Background=\"Transparent\" VerticalContentAlignment=\"Stretch\" HorizontalContentAlignment=\"Stretch\" IsTabStop=\"False\" />"
                +
                "</DataTemplate>");

        public static ContentControl DefaultTabItemContent =>
            (ContentControl)XamlReader.Parse(@"<ContentControl xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                              xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                                              xmlns:igWindow=""http://infragistics.com/Windows""
                                              xmlns:ig=""http://schemas.infragistics.com/xaml""
                                              xmlns:loki=""http://loki/core"">

                                <ig:XamBusyIndicator IsBusy= ""{Binding IsBusy}"" BusyContent=""{Binding Status}"" DisplayAfter=""{Binding BusyAfter}"">
                                    <ContentControl loki:View.Model=""{Binding}"" />
                                </ig:XamBusyIndicator>
                            </ContentControl>
                       ");
    }
}