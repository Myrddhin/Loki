using Loki.Common;
using Loki.UI.Commands;

using Moq;

namespace Loki.Core.Tests.UI
{
    public class RoutedCommandTest : CommandTest
    {
        public RoutedCommandTest()
        {
            var logger = new Mock<ILoggerComponent>();
            var log = new Mock<ILog>();
            logger.Setup(x => x.GetLog(It.IsAny<string>())).Returns(log.Object);

            var messages = new Mock<IMessageComponent>();

            var commands = new Mock<ICommandComponent>();

            Command = new LokiRoutedCommand("Test", logger.Object, commands.Object, messages.Object);
            var handler = LokiCommandHandler.Create(State.CanExecute, State.Execute, State);

            commands.Setup(x => x.GetHandlers(Command)).Returns(new[] { handler });
        }
    }
}