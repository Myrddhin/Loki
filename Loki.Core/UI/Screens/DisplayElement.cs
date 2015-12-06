using System;

using Loki.Common;
using Loki.UI.Commands;
using Loki.UI.Tasks;

namespace Loki.UI
{
    public class DisplayElement : TrackedObject
    {
        protected IDisplayServices Services { get; private set; }

        public DisplayElement(IDisplayServices coreServices)
            : base(coreServices.Core)
        {
            this.Services = coreServices;
        }

        public IMessageComponent CommonBus
        {
            get
            {
                return Services.Core.Messages;
            }
        }

        public ICommandComponent CommandService
        {
            get
            {
                return Services.UI.Commands;
            }
        }

        public IEventComponent EventService
        {
            get
            {
                return Services.Core.Events;
            }
        }

        public IWindowManager Windows
        {
            get
            {
                return Services.UI.Windows;
            }
        }

        public ITaskComponent Tasks
        {
            get
            {
                return Services.UI.Tasks;
            }
        }

        public IThreadingContext ThreadingContext
        {
            get
            {
                return Services.UI.Threading;
            }
        }

        public ISignalManager Signals
        {
            get
            {
                return Services.UI.Signals;
            }
        }

        protected void Information(string message)
        {
            ThreadingContext.OnUIThread(() => Signals.Message(message));
        }

        protected void Warning(string message)
        {
            ThreadingContext.OnUIThread(() => Signals.Message(message));
        }

        protected void Error(string message)
        {
            Error(new LokiException(message));
        }

        protected void Error(Exception exception)
        {
            ThreadingContext.OnUIThread(() => Signals.Error(exception, false));
        }
    }
}