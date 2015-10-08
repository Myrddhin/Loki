using System;

using Loki.Common;
using Loki.IoC.Registration;

using Moq;

using Xunit;

namespace Loki.Core.Tests.Common
{
    public class ErrorComponentTest : CommonTest
    {
        public ErrorComponentTest()
        {
            LogMock = new Mock<ILoggerComponent>();
            Log = new Mock<ILog>();
            LogMock.Setup(x => x.GetLog(It.IsAny<string>())).Returns(Log.Object);

            Context.Register(Element.For<ILoggerComponent>().Instance(LogMock.Object).AsDefault());

            Component = Context.Get<IErrorComponent>();
        }

        public IErrorComponent Component { get; private set; }

        public Mock<ILoggerComponent> LogMock { get; private set; }

        public Mock<ILog> Log { get; private set; }

        [Fact]
        public void LogError()
        {
            string message = "Test";
            var exception = new ApplicationException();
            Component.LogError(message, exception);

            Log.Verify(x => x.Error(message, exception));
        }

        [Fact]
        public void BuildError()
        {
            string message = "Test";
            var raised = Component.BuildError<ApplicationException>(message);

            Assert.Equal(raised.Message, message);
            Log.Verify(x => x.Error(message, null));
        }

        [Fact]
        public void BuildErrorFormat()
        {
            string message = "Test {0}";
            var raised = Component.BuildErrorFormat<ApplicationException>(message, "1");

            Assert.Equal(raised.Message, string.Format(message, "1"));
            Log.Verify(x => x.Error(string.Format(message, "1"), null));
        }

        [Fact]
        public void BuildErrorWithException()
        {
            string message = "Test";
            var exception = new ApplicationException();
            var raised = Component.BuildError<ApplicationException>(message, exception);

            Assert.Equal(raised.Message, message);
            Log.Verify(x => x.Error(message, exception));
        }

        [Fact]
        public void BuildErrorFormatWithException()
        {
            string message = "Test {0}";
            var exception = new ApplicationException();
            var raised = Component.BuildErrorFormat<ApplicationException>(exception, message, "1");

            Assert.Equal(raised.Message, string.Format(message, "1"));
            Log.Verify(x => x.Error(string.Format(message, "1"), exception));
        }
    }
}