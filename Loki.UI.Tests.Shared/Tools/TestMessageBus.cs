using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Loki.Common.Messages;

namespace Loki.Common
{
    public class TestMessageBus : IMessageBus
    {
        public bool HandlerExistsFor(Type messageType)
        {
            throw new NotImplementedException();
        }

        public void Publish(object message, Action<Action> marshal)
        {
            throw new NotImplementedException();
        }

        public int Subscriptions { get; set; }

        public void Subscribe(object subscriber)
        {
            Subscriptions++;
        }

        public void Unsubscribe(object subscriber)
        {
            Subscriptions--;
        }
    }
}
