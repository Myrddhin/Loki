using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Loki.Common;
using Loki.Common.Messages;
using Loki.UI.Commands;

namespace Loki.UI.Wpf.Test.Tools
{
    public class TestDisplayInfrastructure : TestInfrastructure, IDisplayInfrastructure
    {
        public ICommandManager CommandsManager
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IMessageBus IInfrastructure.MessageBus
        {
            get
            {
                return base.MessageBus;
            }
        }
    }
}
