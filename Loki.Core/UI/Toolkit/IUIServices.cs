using Loki.UI.Commands;
using Loki.UI.Tasks;

namespace Loki.UI
{
    public interface IUIServices
    {
        IWindowManager Windows { get; }

        ITemplatingEngine Templates { get; }

        ISignalManager Signals { get; }

        ICommandComponent Commands { get; }

        IThreadingContext Threading { get; }

        ITaskComponent Tasks { get; }
    }
}