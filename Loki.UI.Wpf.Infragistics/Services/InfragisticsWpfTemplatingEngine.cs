using Infragistics.Windows.Controls;

using Loki.Common;
using Loki.IoC;
using Loki.UI.Wpf.Binds.Infragistics;

namespace Loki.UI.Wpf.Infragistics
{
    internal class InfragisticsWpfTemplatingEngine : WpfTemplatingEngine
    {
        public InfragisticsWpfTemplatingEngine(IObjectContext context, ICoreServices services, IThreadingContext threading)
            : base(context, services, threading)
        {
        }

        protected override object InternalCreateBind(object view, object viewModel)
        {
            var tabControl = view as XamTabControl;
            if (tabControl != null)
            {
                return new TabControlBind(Services, ThreadingContext, tabControl, viewModel);
            }

            return base.InternalCreateBind(view, viewModel);
        }
    }
}