using Loki.Common;
using Loki.UI.Commands;

namespace Loki.UI
{
    internal class UIServices : IUIServices
    {
        public UIServices(
            IWindowManager windows,
            ITemplatingEngine templates,
            ISignalManager signals,
           // ICommandComponent commands,
            //IEventComponent events,
            IThreadingContext threading,
            INavigationService navigation)
        {
            Windows = windows;
            Templates = templates;
            Signals = signals;
           // Commands = commands;
            //Events = events;
            Threading = threading;
            Navigation = navigation;
        }

        public IWindowManager Windows { get; private set; }

        public ITemplatingEngine Templates { get; private set; }

        public ISignalManager Signals { get; private set; }

       // public ICommandComponent Commands { get; private set; }

        //public IEventComponent Events { get; private set; }

        public IThreadingContext Threading { get; private set; }

        public INavigationService Navigation { get; private set; }
    }
}