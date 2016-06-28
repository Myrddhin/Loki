using System;
using System.Collections.Generic;
using System.Text;

namespace Loki.Common
{
    public interface IInfrastrucure
    {
        IMessageBus MessageBus { get; }
    }
}
