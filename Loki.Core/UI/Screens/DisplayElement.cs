﻿using System;

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
            applicationCommands = new Lazy<ApplicationCommands>(GetApplicationCommands);
        }

        protected virtual ApplicationCommands GetApplicationCommands()
        {
            return new ApplicationCommands(this.Services.UI.Commands);
        }

        private readonly Lazy<ApplicationCommands> applicationCommands;

        public ApplicationCommands ApplicationCommands
        {
            get
            {
                return applicationCommands.Value;
            }
        }

        public IMessageComponent Bus
        {
            get
            {
                return Services.Core.Messages;
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