using Loki.Common;
using Loki.IoC.Registration;

using Moq;

namespace Loki.Core.Tests.Common
{
    public class MessageBusTest : CommonTest
    {
        public MessageBusTest()
        {
            LogMock = new Mock<ILoggerComponent>();
            Log = new Mock<ILog>();
            LogMock.Setup(x => x.GetLog(It.IsAny<string>())).Returns(Log.Object);

            Context.Register(Element.For<ILoggerComponent>().Instance(LogMock.Object).AsDefault());

            ErrorMock = new Mock<IErrorComponent>();
            Context.Register(Element.For<IErrorComponent>().Instance(ErrorMock.Object).AsDefault());

            Component = Context.Get<IMessageComponent>();
        }

        public IMessageComponent Component { get; private set; }

        public Mock<ILoggerComponent> LogMock { get; private set; }

        public Mock<IErrorComponent> ErrorMock { get; private set; }

        public Mock<ILog> Log { get; private set; }
    }
}