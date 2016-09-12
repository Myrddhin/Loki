using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.NavBar;

using Loki.Common;
using Loki.IoC;
//using Loki.UI.Wpf.Binds.DevExpress;
//using Loki.UI.Wpf.Templating.DevExpress;

namespace Loki.UI.Wpf.DevExpress
{
    internal class DevExpressWpfTemplatingEngine : WpfTemplatingEngine
    {
        public DevExpressWpfTemplatingEngine(IObjectContext context, ICoreServices coreServices, IThreadingContext threading)
            : base(context, coreServices, threading)
        {
        }

        protected override object InternalCreateBind(object view, object viewModel)
        {
            //var documentManager = view as DocumentGroup;
            //if (documentManager != null)
            //{
            //    return new DocumentGroupBind(Services, ThreadingContext, documentManager, viewModel);
            //}

            //var document = view as DocumentPanel;
            //if (document != null)
            //{
            //    return new DocumentPanelBind(Services, ThreadingContext, document, viewModel);
            //}

            //var tabControl = view as DXTabControl;
            //if (tabControl != null)
            //{
            //    return new TabControlBind(Services, ThreadingContext, tabControl, viewModel);
            //}

            //var tabItem = view as DXTabItem;
            //if (tabItem != null)
            //{
            //    return new TabItemBind(Services, ThreadingContext, tabItem, viewModel);
            //}

            //var navBarItem = view as NavBarItem;
            //if (navBarItem != null)
            //{
            //    return new NavBarItemBind(Services, ThreadingContext, navBarItem, viewModel);
            //}

            //var gridControl = view as GridControl;
            //if (gridControl != null)
            //{
            //    return new GridControlBind(Services, ThreadingContext, gridControl, viewModel);
            //}

            return base.InternalCreateBind(view, viewModel);
        }
    }
}