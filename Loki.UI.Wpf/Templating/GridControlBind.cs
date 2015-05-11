using System.Windows.Data;
using System.Windows.Interactivity;
using DevExpress.Xpf.Grid;
using Loki.UI.Wpf.Behaviors;
using Loki.UI.Wpf.Binds;

namespace Loki.UI.Wpf.Templating
{
    internal class GridControlBind : DependencyObjectBind<GridControl>
    {
        public GridControlBind(GridControl view, object viewModel)
            : base(view, viewModel)
        {
            Binding binding = new Binding(".");
            binding.Source = viewModel;
            binding.Mode = BindingMode.TwoWay;
            binding.NotifyOnSourceUpdated = true;
            binding.NotifyOnTargetUpdated = true;
            view.SetBinding(GridControl.ItemsSourceProperty, binding);

            Interaction.GetBehaviors(view).Add(new GridExportBehaviour());
        }
    }
}