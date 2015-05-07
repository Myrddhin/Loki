using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraBars.Docking2010;
using Loki.Common;

namespace Loki.UI.Win
{
    public static class DocumentManagerExtensions
    {
        public static void Bind<TModel>(this DocumentManager manager, Expression<Func<TModel, object>> propertyGetter) where TModel : class
        {
            var binder = new Binder();

            var containerModel = binder.GetContainer<TModel>(manager.ContainerControl, propertyGetter);
            if (containerModel == null)
            {
                return;
            }

            Toolkit.UI.Templating.CreateBind(manager, containerModel);
        }
    }
}