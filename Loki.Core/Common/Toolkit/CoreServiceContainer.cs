using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loki.IoC;

namespace Loki.Common
{
    public class CoreServiceContainer
    {
        public CoreServiceContainer(IObjectCreator context)
        {
            logger = new LazyResolver<ILoggerComponent>(context);
            errors = new LazyResolver<IErrorComponent>(context);
            events = new LazyResolver<IEventComponent>(context);
            messages = new LazyResolver<IMessageComponent>(context);
        }

        private Lazy<ILoggerComponent> logger;

        private Lazy<IErrorComponent> errors;

        private Lazy<IEventComponent> events;

        private Lazy<IMessageComponent> messages;

        /// <summary>
        /// Gets the error service.
        /// </summary>
        public IErrorComponent ErrorManager
        {
            get
            {
                return errors.Value;
            }
        }

        /// <summary>
        /// Gets the event service.
        /// </summary>
        public IEventComponent Events
        {
            get
            {
                return events.Value;
            }
        }

        /// <summary>
        /// Gets the logger service.
        /// </summary>
        /// <value>The configured logger service.</value>
        public ILoggerComponent Logger
        {
            get
            {
                return logger.Value;
            }
        }

        /// <summary>
        /// Gets the message bus.
        /// </summary>
        /// <value>
        /// The message bus.
        /// </value>
        public IMessageComponent MessageBus
        {
            get
            {
                return messages.Value;
            }
        }
    }
}