using System;
using Loki.Commands;
using Loki.Common;
using Loki.UI.Tasks;

namespace Loki.UI
{
    public class DisplayElement : TrackedObject
    {
        public IMessageComponent CommonBus
        {
            get;
            set;
        }

        public ICommandComponent CommandService
        {
            get;
            set;
        }

        public IEventComponent EventService
        {
            get;
            set;
        }

        public IWindowManager Windows
        {
            get;
            set;
        }

        public ITaskComponent Tasks
        {
            get;
            set;
        }

        public IThreadingContext ThreadingContext
        {
            get;
            set;
        }

        public ISignalManager Signals
        {
            get;
            set;
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