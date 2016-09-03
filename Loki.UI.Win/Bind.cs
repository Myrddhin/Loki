using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

using Loki.Common;
using Loki.Common.Diagnostics;

namespace Loki.UI.Win
{
    public static class Bind
    {
        private static ITemplatingEngine engine;

        private static ILog log;

        private static Binder binder;

        public static void InitializeEngine(ICoreServices services, IThreadingContext context, ITemplatingEngine templatingEngine)
        {
            engine = templatingEngine;
            log = services.Diagnostics.GetLog("Binder");
            binder = new Binder(services, context);
        }

        public static void CreateBind(object view, object viewmodel)
        {
            engine.CreateBind(view, viewmodel);
        }

        public static object GetTemplate(object viewmodel)
        {
            return engine.GetTemplate(viewmodel);
        }

        public static TBinded GetBindedObject<TModel, TBinded>(
            Control control,
            Expression<Func<TModel, TBinded>> propertyGetter) where TModel : class where TBinded : class
        {
            return binder.GetBindedObject(control, propertyGetter);
        }

        public static void TwoWay(
            object destination,
            PropertyInfo destinationProperty,
            INotifyPropertyChanged source,
            PropertyInfo sourceProperty,
            DataSourceUpdateMode mode = DataSourceUpdateMode.OnValidation,
            IValueConverter converter = null,
            object converterParameter = null)
        {
            binder.TwoWay(destination, destinationProperty, source, sourceProperty, mode, converter, converterParameter);
        }

        public static IConductor GetContainer<TModel>(Control control, Expression<Func<TModel, object>> propertyGetter) where TModel : class
        {
            TModel model = control.GetViewModel<TModel>();

            if (model == null)
            {
                log.WarnFormat("Form {0} is not binded to a {1} model", control, model);
                return null;
            }

            var bindingTarget = propertyGetter.Compile()(model);

            var containerModel = bindingTarget as IConductor;
            if (containerModel == null)
            {
                log.Warn("Navigation/Menu model must be a container");
                return null;
            }

            return containerModel;
        }
    }
}