using System.Drawing;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace Loki.UI.Win
{
    public class GridViewBind : ComponentBind<GridView>
    {
        public GridViewBind(GridView view, object viewModel)
            : base(view, viewModel)
        {
            // apparence
            ConfigureApparence(view);

            // dirty rule
            ConfigureIsChangedRule(view);

            if (view.GridControl != null)
            {
                view.GridControl.DataSource = viewModel;
            }
        }

        private static void ConfigureApparence(GridView view)
        {
            view.DataController.AllowIEnumerableDetails = true;
            view.OptionsSelection.MultiSelect = true;
            view.OptionsSelection.UseIndicatorForSelection = true;
            view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
            view.OptionsView.EnableAppearanceEvenRow = true;
            view.OptionsView.EnableAppearanceOddRow = true;
            view.OptionsView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            view.OptionsMenu.ShowGroupSummaryEditorItem = true;
            view.OptionsView.ShowFooter = true;
            view.OptionsFilter.UseNewCustomFilterDialog = true;
            if (!view.OptionsBehavior.ReadOnly)
            {
                view.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
                foreach (GridColumn column in view.Columns)
                {
                    if (column.OptionsColumn.ReadOnly)
                    {
                        column.AppearanceCell.BackColor = Color.LightSeaGreen;
                    }
                }
            }
            else
            {
                view.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
            }
        }

        private static void ConfigureIsChangedRule(GridView view)
        {
            if (view.OptionsBehavior.Editable)
            {
                GridColumn COL_IsChanged = new GridColumn();

                COL_IsChanged.Caption = "IsChanged";
                COL_IsChanged.FieldName = "IsChanged";
                COL_IsChanged.Name = view.Name + "_COL_IsChanged";
                COL_IsChanged.Width = 44;
                COL_IsChanged.Visible = false;
                COL_IsChanged.OptionsColumn.ShowInCustomizationForm = false;
                view.Columns.Add(COL_IsChanged);

                StyleFormatCondition condition = new StyleFormatCondition();
                condition.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
                condition.Appearance.Options.UseFont = true;
                condition.ApplyToRow = true;
                condition.Column = COL_IsChanged;
                condition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                condition.Value1 = true;
                view.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] { condition });
            }
        }
    }
}