using Loki.Common;

namespace Loki.UI.Office.Tests
{
    public class MainViewModel : OfficeShell, IHandle<TestMessage>
    {
        public MainViewModel(IDisplayServices services, IScreenFactory factory) : base(services, factory)
        {
        }

        public void Handle(TestMessage message)
        {
            ShowAsDialog(message);
        }
    }
}