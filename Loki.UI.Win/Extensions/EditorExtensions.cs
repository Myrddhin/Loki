using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;

using Loki.Common;

namespace Loki.UI.Win
{
    public static class EditorExtensions
    {
        public static void BindValue<TVM>(
           this BaseEdit editor,
           Expression<Func<TVM, object>> property,
           RepositoryItem format = null,
           DataSourceUpdateMode mode = DataSourceUpdateMode.OnPropertyChanged,
           IValueConverter converter = null,
           object converterParameter = null) where TVM : class, INotifyPropertyChanged
        {
            var viewModel = editor.GetViewModel<TVM>();
            if (viewModel == null)
            {
                return;
            }

            if (format != null)
            {
                editor.Properties.Assign(format);
            }

            // get editor property info
            PropertyInfo destinationDescriptor = ExpressionHelper.GetProperty<BaseEdit, object>(x => x.EditValue);

            // set model property info
            PropertyInfo sourceDescriptior = ExpressionHelper.GetProperty(property);

            Bind.TwoWay(editor, destinationDescriptor, viewModel, sourceDescriptior, mode, converter, converterParameter);
        }
    }
}