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

            Messages = new Mock<IMessageBus>();
            Context.Register(Element.For<IMessageBus>().Instance(Messages.Object).AsDefault());
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

        private T component;

        public T Component
        {
            get
            {
                if (component == default(T))
                {
                    component = Context.Get<T>();
                }

                return component;
            }
        }

        public Mock<ILoggerComponent> Logger { get; private set; }

        public Mock<IErrorComponent> Error { get; private set; }

        public Mock<IMessageBus> Messages { get; private set; }

        public Mock<ILog> Log { get; private set; }
    }
}