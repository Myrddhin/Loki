using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraGrid.Views.Grid;
using Loki.Common;

namespace Loki.UI.Win
{
    public static class GridViewExtensions
    {
        public static void Bind<TModel, TItem>(this GridView view, Expression<Func<TModel, IObservableCollection<TItem>>> propertyGetter)
            where TModel : class
            where TItem : class
        {
            var binder = new Binder();

            var containerModel = binder.GetBindedObject(view.GridControl, propertyGetter);
            if (containerModel == null)
            {
                return;
            }

            Toolkit.UI.Templating.CreateBind(view, containerModel);
        }
    }
}