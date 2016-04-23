using System;

using Loki.Castle;
using Loki.Core.Tests.IoC;

namespace Loki.Core.Tests
{
    public class CastleTests : IoCTest, IDisposable
    {
        public CastleTests()
        {
            Component = new CastleEngine();
        }

        public void Dispose()
        {
            Component.SafeDispose();
        }
    }
}