using Loki.UI.Commands;

namespace Loki.Core.Tests.UI
{
    public class RelayCommandTest : CommandTest
    {
        public RelayCommandTest()
        {
            Command = new LokiRelayCommand(State.DirectCanExecute, State.DirectExecute);
        }
    }
}