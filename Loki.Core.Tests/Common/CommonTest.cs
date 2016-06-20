using System;

using Loki.Core.IoC;

namespace Loki.Core.Tests.Common
{
    public class CommonTest : IDisposable
    {
        protected IDependencyResolver Context { get; private set; }

        public CommonTest()
        {
            Context = IoCContainer.DependencyResolverFactory();
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