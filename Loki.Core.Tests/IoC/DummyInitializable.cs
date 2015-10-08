using Loki.UI;

namespace Loki.Core.Tests.IoC
{
    public class DummyInitializable : IInitializable
    {
        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            IsInitialized = true;
        }
    }
}