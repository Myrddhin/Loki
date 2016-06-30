using System;

using Loki.Common.IoC;

namespace Loki.Common.Tests
{
    public class CommonTest : IDisposable
    {
        protected IoCContainer Context { get; private set; }

        public CommonTest()
        {
            Context = new IoCContainer(true);
            Context.RegisterAssembly(typeof(IoCContainer).Assembly);
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
    }
}