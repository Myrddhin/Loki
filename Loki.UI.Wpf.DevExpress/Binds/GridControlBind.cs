using System.Windows.Data;
using System.Windows.Interactivity;

using DevExpress.Xpf.Grid;

using Loki.Common;
using Loki.UI.Wpf.Binds;
using Loki.UI.Wpf.DevExpress.Behaviors;

namespace Loki.UI.Wpf.Templating.DevExpress
{
    internal class GridControlBind : DependencyObjectBind<GridControl>
    {
        public GridControlBind(ICoreServices services, IThreadingContext threading, GridControl view, object viewModel)
            : base(services, threading, view, viewModel)
        {
            Binding binding = new Binding(".")
            {
                Source = viewModel,
                Mode = BindingMode.TwoWay,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true
            };

            view.SetBinding(DataControlBase.ItemsSourceProperty, binding);

            Interaction.GetBehaviors(view).Add(new GridExportBehaviour());
        }
    }
}