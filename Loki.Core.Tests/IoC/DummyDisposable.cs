using System;

namespace Loki.Common.IoC.Tests
{
    public class DummyDisposable : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Disposed = true;
        }
    }
}