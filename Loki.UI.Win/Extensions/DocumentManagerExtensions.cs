using System;
using System.Linq.Expressions;

using DevExpress.XtraBars.Docking2010;

namespace Loki.UI.Win
{
    public static class DocumentManagerExtensions
    {
        public static void Bind<TModel>(this DocumentManager manager, Expression<Func<TModel, object>> propertyGetter) where TModel : class
        {
            var containerModel = Win.Bind.GetContainer(manager.ContainerControl, propertyGetter);
            if (containerModel == null)
            {
                return;
            }

            Win.Bind.CreateBind(manager, containerModel);
        }
    }
}