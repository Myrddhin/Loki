using Loki.UI.Commands;

namespace Loki.UI
{
    public interface IUIServices
    {
        IWindowManager Windows { get; }

        ITemplatingEngine Templates { get; }

        ISignalManager Signals { get; }

        ICommandComponent Commands { get; }

        IThreadingContext Threading { get; }

        INavigationService Navigation { get; }

        // ITaskComponent Tasks { get; }
    }
}