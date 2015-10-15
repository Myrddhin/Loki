using Loki.Commands;
using Loki.Common;
using Loki.UI.Tasks;

namespace Loki.UI
{
    internal class UIServices : IUIServices
    {
        public UIServices(
            IWindowManager windows,
            ITemplatingEngine templates,
            ISignalManager signals,
            ICommandComponent commands,
            IEventComponent events,
            IThreadingContext threading,
            ITaskComponent tasks)
        {
            Windows = windows;
            Templates = templates;
            Signals = signals;
            Commands = commands;
            Events = events;
            Threading = threading;
            Tasks = tasks;
        }

        public IWindowManager Windows { get; private set; }

        public ITemplatingEngine Templates { get; private set; }

        public ISignalManager Signals { get; private set; }

        public ICommandComponent Commands { get; private set; }

        public IEventComponent Events { get; private set; }

        public IThreadingContext Threading { get; private set; }

        public ITaskComponent Tasks { get; private set; }
    }
}