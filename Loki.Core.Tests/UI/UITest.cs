using System;

using Loki.Common;
using Loki.IoC;
using Loki.IoC.Registration;

using Moq;

namespace Loki.Core.Tests.UI
{
    public class UITest<T> : IDisposable where T : class
    {
        protected IObjectContext Context { get; private set; }

        public UITest()
        {
            Context = new IoCContext();

            Logger = new Mock<ILoggerComponent>();
            Log = new Mock<ILog>();
            Logger.Setup(x => x.GetLog(It.IsAny<string>())).Returns(Log.Object);
            Context.Register(Element.For<ILoggerComponent>().Instance(Logger.Object).AsDefault());

            Error = new Mock<IErrorComponent>();
            Context.Register(Element.For<IErrorComponent>().Instance(Error.Object).AsDefault());

            Messages = new Mock<IMessageComponent>();
            Context.Register(Element.For<IMessageComponent>().Instance(Messages.Object).AsDefault());

            Component = Context.Get<T>();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed;

        protected virtual void Dispose(bool isDisposing)
        {
            if (disposed)
            {
                return;
            }

            Context.Dispose();

            disposed = true;
        }

        public T Component { get; private set; }

        public Mock<ILoggerComponent> Logger { get; private set; }

        public Mock<IErrorComponent> Error { get; private set; }

        public Mock<IMessageComponent> Messages { get; private set; }

        public Mock<ILog> Log { get; private set; }
    }
}