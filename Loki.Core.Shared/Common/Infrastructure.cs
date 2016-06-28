using System;
using System.Collections.Generic;
using System.Text;

namespace Loki.Common
{
    internal class Infrastructure : IInfrastrucure
    {
        public Infrastructure(
            IMessageBus bus)
        {
            MessageBus = bus;
        }

        public IMessageBus MessageBus { get; private set; }
    }
}
