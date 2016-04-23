using System;
using System.Linq.Expressions;
using DevExpress.XtraBars;

namespace Loki.UI.Win
{
    public static class BarExtensions
    {
        public static void Bind<TModel>(this Bar bar, Expression<Func<TModel, object>> propertyGetter) where TModel : class
        {
            var containerModel = Win.Bind.GetContainer<TModel>(bar.Manager.Form, propertyGetter);
            if (containerModel == null)
            {
                return;
            }

            Win.Bind.CreateBind(bar, containerModel);
        }
    }
}