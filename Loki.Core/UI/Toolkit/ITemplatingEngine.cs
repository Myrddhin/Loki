using System;
using System.Reflection;

namespace Loki.UI
{
    /// <summary>
    /// Common interface for templating engines.
    /// </summary>
    public interface ITemplatingEngine
    {
        object GetTemplate(object model);

        object CreateBind(object view, object viewModel);

        event EventHandler<BindingEventArgs> BindingRequired;

        void RegisterAssociation(Type modelType, Type viewType);

        void RegisterAssociation<TModel, TView>();

        void LoadByConvention(IConventionManager conventionManager, params Assembly[] assemblies);
    }
}