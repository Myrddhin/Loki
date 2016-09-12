using Loki.Common;
using Loki.UI.Commands;

namespace Loki.UI
{
    public interface IDisplayInfrastructure : IInfrastructure
    {
        ICommandManager CommandsManager { get; }
    }
}
