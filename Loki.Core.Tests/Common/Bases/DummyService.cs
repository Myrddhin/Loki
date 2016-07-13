using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Loki.Common.Messages;

namespace Loki.Common
{
    public class DummyService : BaseService, IHandle<SimpleMessage>
    {
        public DummyService(IInfrastrucure infrastructure)
            : base(infrastructure)
        {
        }

        protected override bool InitializeService()
        {
            RaiseInitialized?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public event EventHandler Received;

        public event EventHandler RaiseInitialized;

        public void Handle(SimpleMessage message)
        {
            Received?.Invoke(this, EventArgs.Empty);
        }
    }
}
