using Loki.Common.IoC;

namespace Loki.UI
{
    public interface IPlatform
    {
        IoCContainer CompositionRoot { get; }
    }
}