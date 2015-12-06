using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

using DevExpress.Export;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPrinting;

using Loki.Common;
using Loki.UI.Commands;

namespace Loki.UI.Win
{
    public static class GridViewExtensions
    {
        public static void Bind<TModel, TItem>(
            this GridView view,
            Expression<Func<TModel, IObservableCollection<TItem>>> propertyGetter)
            where TModel : class
            where TItem : class
        {
            var binder = new Binder();

            var containerModel = binder.GetBindedObject(view.GridControl, propertyGetter);
            if (containerModel == null)
            {
                return;
            }

            ConfigureGridCommands<TModel, TItem>(view, null, containerModel);

            Toolkit.UI.Templating.CreateBind(view, containerModel);
        }

        public static void BindCommandOnDoubleClick<TModel, TItem>(
            this GridView view,
            Expression<Func<TModel, ICommand>> commandGetter)
            where TModel : class
            where TItem : class
        {
            TModel vm = view.GridControl.GetViewModel<TModel>();
            view.DoubleClick += (s, e) =>
                {
                    Point pt = view.GridControl.PointToClient(Control.MousePosition);
                    GridHitInfo info = view.CalcHitInfo(pt);
                    if (info.InRow || info.InRowCell)
                    {
                        var row = view.GetRow(info.RowHandle) as TItem;
                        if (row != null && vm != null)
                        {
                            var command = commandGetter.Compile()(vm);
                            command.Execute(row);
                        }
                    }
                };
        }

        private static DXMenuItem CreateItem(string label, Action handler, bool beginGroup = false)
        {
            var item = new DXMenuItem(label, (s, e) => handler());
            item.BeginGroup = beginGroup;
            return item;
        }

        public static void ConfigureGridCommands<TVM, TItem>(
            this GridView grid,
            Expression<Func<TVM, IEnumerable<ICommandElement>>> rowCommands,
            IObservableCollection<TItem> dataSource)
            where TItem : class
            where TVM : class
        {
            // menu
            if (grid.GridControl != null)
            {
                grid.PopupMenuShowing += (s, e) =>
                    {
                        if (e.Menu == null)
                        {
                            return;
                        }

                        if (e.Menu.MenuType == GridMenuType.Row || e.Menu.MenuType == GridMenuType.User
                            || e.HitInfo.HitTest == GridHitTest.EmptyRow)
                        {
                            e.Menu.Items.Add(CreateItem("Export to excel", () => ExportToExcel(grid)));

                            if (grid.GroupedColumns != null && grid.GroupedColumns.Count > 0)
                            {
                                e.Menu.Items.Add(CreateItem("Expand all", grid.ExpandAllGroups, true));
                                e.Menu.Items.Add(CreateItem("Collapse all", grid.CollapseAllGroups));
                            }

                            if (grid.OptionsBehavior.Editable && !grid.OptionsBehavior.ReadOnly)
                            {
                                /*L_Commands["Paste"] = delegate()
                            {
                                P_Grid.PasteValues();
                            };*/
                                if (grid.OptionsBehavior.AllowDeleteRows != DefaultBoolean.False)
                                {
                                    var item = CreateItem(
                                        "Clear",
                                        () =>
                                        {
                                            if (Toolkit.UI.Windows.Confirm("Delete all lines ?"))
                                            {
                                                dataSource.Clear();
                                            }
                                        });
                                    e.Menu.Items.Add(item);
                                }
                            }

                            if (rowCommands != null)
                            {
                                var viewModel = View.GetViewModel<TVM>(grid.GridControl);
                                if (viewModel == null)
                                {
                                    return;
                                }

                                var commands = new List<ICommandElement>();

                                commands.AddRange(rowCommands.Compile()(viewModel));

                                if (commands.Any() && e.HitInfo.InDataRow)
                                {
                                    bool first = true;
                                    foreach (var item in commands)
                                    {
                                        string name = item.Command.Name;
                                        var displayName = item as IHaveDisplayName;
                                        if (displayName != null)
                                        {
                                            name = displayName.DisplayName;
                                        }

                                        var row = grid.GetRow(e.HitInfo.RowHandle) as TItem;
                                        var menuItem = CreateItem(name, () => item.Command.Execute(row), first);
                                        menuItem.Enabled = item.Command.CanExecute(row);
                                        first = false;
                                        e.Menu.Items.Add(menuItem);
                                    }
                                }
                            }
                        }
                    };
            }
        }

        public static void ExportToExcel(this GridView grid)
        {
            ILog log = Toolkit.Common.Logger.GetLog("GridViewExtensions");
            try
            {
                string tempfile = string.Format(
                    "{0}export_excel_{1}.xlsx",
                    Path.GetTempPath(),
                    System.DateTime.Now.Ticks);
                grid.OptionsPrint.AutoWidth = false;
                log.InfoFormat("Exporting to {0}", tempfile);
                using (Stream str = new MemoryStream())
                {
                    grid.SaveLayoutToStream(str);
                    str.Seek(0, System.IO.SeekOrigin.Begin);
                    grid.BestFitColumns();
                    grid.OptionsView.ColumnAutoWidth = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx(TextExportMode.Value);
                    options.ExportMode = XlsxExportMode.SingleFile;
                    options.ExportType = ExportType.DataAware;
                    grid.ExportToXlsx(tempfile, options);
                    ProcessStartInfo infos = new System.Diagnostics.ProcessStartInfo();
                    infos.UseShellExecute = false;
                    infos.CreateNoWindow = true;
                    infos.FileName = "excel.exe";
                    infos.Arguments = tempfile;

                    try
                    {
                        System.Diagnostics.Process.Start(infos);
                    }
                    catch (Win32Exception)
                    {
                        // no open excel ; use shell. Upgrade by check if an excel adapter is available in object context.
                        System.Diagnostics.Process.Start(tempfile);
                    }

                    grid.RestoreLayoutFromStream(str);
                }
            }
            catch (Exception ex)
            {
                log.Error("Internal error", ex);
                Toolkit.Common.MessageBus.PublishOnUIThread(new WarningMessage("Internal error while exporting"));
            }
        }
    }
}