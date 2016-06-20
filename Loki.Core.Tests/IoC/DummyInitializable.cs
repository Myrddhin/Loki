using System.ComponentModel;

namespace Loki.Core.Tests.IoC
{
    public class DummyInitializable : ISupportInitialize
    {
        public bool EndDone { get; private set; }

        public bool BeginDone { get; private set; }

        public void BeginInit()
        {
            BeginDone = true;
        }

        public void EndInit()
        {
            EndDone = true;
        }
    }
}