using Loki.Common;

namespace Loki.UI
{
    internal class DisplayServices : IDisplayServices
    {
        public DisplayServices(ICoreServices core, IUIServices ui)
        {
            Core = core;
            UI = ui;
        }

        public ICoreServices Core { get; private set; }

        public IUIServices UI { get; private set; }
    }
}