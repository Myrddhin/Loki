using Loki.UI;

namespace Loki.Core.Tests.UI
{
    public class UIInitializableObject : UIObject, IInitializable
    {
        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            IsInitialized = true;
        }
    }
}