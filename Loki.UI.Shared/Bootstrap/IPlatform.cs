using System.Reflection;

using Loki.Common.IoC;

namespace Loki.UI
{
    public interface IPlatform
    {
        IoCContainer CompositionRoot { get; }

        Assembly[] UiAssemblies { get; }

        IConventionManager Conventions { get; }

        IBinder Binder { get; }
    }
}