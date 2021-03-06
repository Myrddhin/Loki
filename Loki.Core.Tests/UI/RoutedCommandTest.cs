﻿using Loki.Common;
using Loki.UI;
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

            var messages = new Mock<IMessageBus>();

            var commands = new Mock<ICommandComponent>();

            var threading = new Mock<IThreadingContext>();

            Command = new LokiRoutedCommand("Test", logger.Object, commands.Object, messages.Object, threading.Object);
            Handler = LokiCommandHandler.Create(State.CanExecute, State.Execute, State);

            commands.Setup(x => x.GetHandlers(Command)).Returns(new[] { Handler });
        }
    }
}