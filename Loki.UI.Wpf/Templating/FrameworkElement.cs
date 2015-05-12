using System.Windows;
using System.Windows.Markup;

namespace Loki.UI.Wpf.Binds
{
    internal class FrameworkElementBind<TComponent> : DependencyObjectBind<TComponent> where TComponent : FrameworkElement
    {
        public static DataTemplate DefaultItemTemplate = (DataTemplate)
       XamlReader.Parse(
            "<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' " +
                          "xmlns:loki='clr-namespace:Loki.UI.Wpf;assembly=Loki.UI.Wpf'> " +
               "<loki:LoadingDecorator DisplayText=\"{Binding Status}\" IsLoading=\"{Binding IsBusy}\"><ContentControl loki:View.Model=\"{Binding}\" Background=\"Transparent\" VerticalContentAlignment=\"Stretch\" HorizontalContentAlignment=\"Stretch\" IsTabStop=\"False\" />" +
           "</loki:LoadingDecorator></DataTemplate>");

        public FrameworkElementBind(TComponent component, object viewModel)
            : base(component, viewModel)
        {
            Component.Dispatcher.Invoke(() => Component.SetValue(FrameworkElement.DataContextProperty, viewModel));

            var loadable = viewModel as ILoadable;

            if (loadable != null)
            {
                View.ExecuteOnLoad(component, (s, e) => loadable.Load());
            }
        }
    }
}