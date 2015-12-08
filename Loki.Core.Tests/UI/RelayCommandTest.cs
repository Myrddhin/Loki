using Loki.Common;
using Loki.UI.Commands;

using Moq;

namespace Loki.Core.Tests.UI
{
    public class RelayCommandTest : CommandTest
    {
        public RelayCommandTest()
        {
            Command = new LokiRelayCommand(State.DirectCanExecute, State.DirectExecute);

            var logger = new Mock<ILoggerComponent>();
            var log = new Mock<ILog>();
            logger.Setup(x => x.GetLog(It.IsAny<string>())).Returns(log.Object);

            var messages = new Mock<IMessageComponent>();

            var commands = new Mock<ICommandComponent>();

            Command = new LokiRelayCommand(State.DirectCanExecute, State.DirectExecute);
            Handler = LokiCommandHandler.Create(State.CanExecute, State.Execute, State);

            commands.Setup(x => x.GetHandlers(Command)).Returns(new[] { Handler });
        }
    }
}