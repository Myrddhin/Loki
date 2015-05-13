using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using Loki.Common;

namespace Loki.UI.Win
{
    public static class GridColumnExtensions
    {
        public static void Bind<TItem>(
           this GridColumn column,
           Expression<Func<TItem, object>> property,
           RepositoryItem editor = null/*,
                ColumnRule<TDisplayable> P_Rule = null*/)
            where TItem : class
        {
            // Field name.
            PropertyInfo propertyInfo = ExpressionHelper.GetProperty(property);
            column.FieldName = propertyInfo.Name;

            // Specific mode for dates.
            if (propertyInfo.PropertyType == typeof(DateTime)
                || propertyInfo.PropertyType == typeof(DateTime?))
            {
                column.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.List;
            }

            // Editor standardisation.
            if (editor != null)
            {
                column.ColumnEdit = editor;
                column.DisplayFormat.Assign(editor.DisplayFormat);
            }

            // Summary
            if (column.SummaryItem != null && column.SummaryItem.SummaryType != DevExpress.Data.SummaryItemType.None)
            {
                column.SummaryItem.FieldName = propertyInfo.Name;
            }

            /*if (P_Rule != null)
            {
                P_Column.View.ValidateRow += delegate(object sender, ValidateRowEventArgs e)
                {
                    TDisplayable L_Item = e.Row as TDisplayable;
                    if (!P_Rule.Validator(L_Item))
                    {
                        e.Valid = false;
                        e.ErrorText = P_Rule.ErrorMessageBuilder(L_Item);
                        P_Column.View.SetColumnError(P_Column, e.ErrorText);
                        return;
                    }
                    else
                    {
                        P_Column.View.SetColumnError(P_Column, string.Empty);
                    }
                };
            }*/
        }
    }
}