using System.Diagnostics;
using System.IO;
using System.Windows.Interactivity;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Grid;

namespace Loki.UI.Wpf.Behaviors
{
    public class GridExportBehaviour : Behavior<GridControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            TableView view = AssociatedObject.View as TableView;

            view.ShowGridMenu += View_ShowGridMenu;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            TableView view = AssociatedObject.View as TableView;
            view.ShowGridMenu -= View_ShowGridMenu;
        }

        private void Export_ItemClick(object sender, ItemClickEventArgs e)
        {
            TableView view = AssociatedObject.View as TableView;
            string fileName = Path.GetTempFileName() + ".xlsx";
            view.ExportToXlsx(fileName);
            Process.Start(fileName);
        }

        private void View_ShowGridMenu(object sender, GridMenuEventArgs e)
        {
            BarItemLinkSeparator separator = new BarItemLinkSeparator();

            BarButtonItem export = new BarButtonItem();
            export.Name = "Export";
            export.Content = "Export excel";
            //export.Glyph = AssociatedObject.FindResource("MNI_excel") as ImageSource;
            //export.BarItemDisplayMode = BarItemDisplayMode.ContentAndGlyph;
            export.ItemClick += Export_ItemClick;

            e.Customizations.Add(separator);
            e.Customizations.Add(export);
        }
    }
}