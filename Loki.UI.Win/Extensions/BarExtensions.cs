using System;
using System.Linq.Expressions;
using DevExpress.XtraBars;
using Loki.Common;

namespace Loki.UI.Win
{
    public static class BarExtensions
    {
        public static void Bind<TModel>(this Bar bar, Expression<Func<TModel, object>> propertyGetter) where TModel : class
        {
            Binder binder = new Binder();

            var containerModel = binder.GetContainer<TModel>(bar.Manager.Form, propertyGetter);
            if (containerModel == null)
            {
                return;
            }

            Toolkit.UI.Templating.CreateBind(bar, containerModel);
        }
    }
}