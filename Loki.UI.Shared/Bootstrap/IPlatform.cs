using System.Reflection;

using Loki.Common.IoC;

namespace Loki.UI.Bootstrap
{
    public interface IPlatform
    {
        IoCContainer CompositionRoot { get; }

        Assembly[] UIAssemblies { get; }

        IConventionManager Conventions { get; }

        IBinder Binder { get; }

        void SetEntryPoint(object mainTemplate);
    }
}