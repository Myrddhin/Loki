using System;

using Loki.Common.IoC.Tests;
using Loki.Common.Messages;

namespace Loki.Common
{
    public class DummyService : BaseService, IHandle<SimpleMessage>
    {
        public DummyService(IInfrastructure infrastructure)
            : base(infrastructure)
        {
            SubDisposable = new DummyDisposable();
            RegisterDisposable(SubDisposable);
        }

        public DummyDisposable SubDisposable { get; private set; }

        public event EventHandler Received;

        public void Handle(SimpleMessage message)
        {
            Received?.Invoke(this, EventArgs.Empty);
        }
    }
}