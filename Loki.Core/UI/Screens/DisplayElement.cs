using System;
using Loki.Commands;
using Loki.Common;
using Loki.IoC;

namespace Loki.UI
{
    public class DisplayElement : TrackedObject, IContextAware
    {
        private IObjectCreator context;

        protected IObjectCreator Context
        {
            get
            {
                if (context == null)
                {
                    return Toolkit.IoC.DefaultContext;
                }
                else
                {
                    return context;
                }
            }
        }

        public void SetContext(IObjectContext externalContext)
        {
            context = externalContext;
            OnContextInitialized(EventArgs.Empty);
        }

        public void Release()
        {
        }

        #region ContextInitialized

        public event EventHandler ContextInitialized;

        protected virtual void OnContextInitialized(EventArgs e)
        {
            EventHandler handler = ContextInitialized;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion ContextInitialized

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

        public CommandManager Commands
        {
            get;
            set;
        }
    }
}