using System.Reflection;

namespace Loki.UI
{
    public interface ITemplateManager
    {
        void AddConventions(IConventionManager conventionManager, params Assembly[] uiAssemblies);

        void AddBindings(IBinder applicationBinder);

        object GetBindedTemplate(object model);

        void CreateBind(object template, object model);

        void BindWithTemplate(object model);
    }
}