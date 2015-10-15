using System;
using Loki.Commands;
using Loki.Common;
using Loki.UI.Tasks;

namespace Loki.UI
{
    public class DisplayElement : TrackedObject
    {
        protected ICoreServices Services { get; private set; }

        protected IUIServices UI { get; private set; }

        public DisplayElement(ICoreServices services, IUIServices uiServices)
            : base(services)
        {
            Services = services;
            UI = uiServices;
        }

        public IMessageComponent CommonBus
        {
            get
            {
                return Services.Messages;
            }
        }

        public ICommandComponent CommandService
        {
            get
            {
                return UI.Commands;
            }
        }

        public IEventComponent EventService
        {
            get
            {
                return UI.Events;
            }
        }

        public IWindowManager Windows
        {
            get
            {
                return UI.Windows;
            }
        }

        public ITaskComponent Tasks
        {
            get
            {
                return UI.Tasks;
            }
        }

        public IThreadingContext ThreadingContext
        {
            get
            {
                return UI.Threading;
            }
        }

        public ISignalManager Signals
        {
            get
            {
                return UI.Signals;
            }
        }

        public CommandManager Commands
        {
            get;
            set;
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