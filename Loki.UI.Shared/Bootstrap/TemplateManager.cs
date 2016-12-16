using System;
using System.Collections.Generic;
using System.Reflection;

using Loki.Common;

namespace Loki.UI.Bootstrap
{
    internal class TemplateManager : BaseService, ITemplateManager
    {
        private Dictionary<string, Func<object>> templateMapping = new Dictionary<string, Func<object>>();

        private IBinder binder;

        public TemplateManager(IInfrastructure infrastructure)
            : base(infrastructure)
        {
        }

        public void AddConventions(IConventionManager conventionManager, params Assembly[] graphicAssemblies)
        {
            this.templateMapping = new Dictionary<string, Func<object>>(conventionManager.ViewViewModel(graphicAssemblies));
        }

        public void AddBindings(IBinder applicationBinder)
        {
            this.binder = applicationBinder;
        }

        public object GetBindedTemplate(object model)
        {
            var key = model.GetType().Name;

            if (!this.templateMapping.ContainsKey(key))
            {
                return null;
            }

            var template = this.templateMapping[key]();
            this.binder.CreateBind(template, model);
            return template;
        }

        public void CreateBind(object template, object model)
        {
            this.binder.CreateBind(template, model);
        }

        public void BindWithTemplate(object model)
        {
            GetBindedTemplate(model);
        }
    }
}