using System;

using Loki.IoC;

namespace Loki.Core.Tests.Common
{
    public class CommonTest : IDisposable
    {
        protected IObjectContext Context { get; private set; }

        public CommonTest()
        {
            Context = new IoCContext();
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