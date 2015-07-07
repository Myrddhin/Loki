using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
            Binder binder = new Binder();

            var viewModel = View.GetViewModel<TVM>(editor);
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

            binder.TwoWay(editor, destinationDescriptor, viewModel, sourceDescriptior, mode, converter, converterParameter);
        }
    }
}