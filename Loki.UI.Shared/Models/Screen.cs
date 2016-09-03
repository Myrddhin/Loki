using System;
using System.ComponentModel;
using System.Threading;

using Loki.Common.Diagnostics;

namespace Loki.UI.Models
{
    public class Screen : DisplayUnit, IActivable, ILoadable, IGuardClose
    {
        protected IDisplayInfrastructure Infrastructure { get; }

        private readonly Lazy<ILog> log;

        protected ILog Log => this.log.Value;

        public Screen(IDisplayInfrastructure infrastructure)
        {
            Infrastructure = infrastructure;

            // Log.
            this.log = new Lazy<ILog>(() => infrastructure.Diagnostics.GetLog(this.GetType().FullName));

            // Message bus subscription.
            Infrastructure.MessageBus.Subscribe(this);
        }

        public event EventHandler Activated;

        #region IsActive

        private static readonly PropertyChangedEventArgs argsIsActiveChanged = new PropertyChangedEventArgs(nameof(IsActive));

        private bool isActive;

        public bool IsActive
        {
            get
            {
                return isActive;
            }

            private set
            {
                if (value == this.isActive)
                {
                    return;
                }

                this.isActive = value;
                this.NotifyChanged(argsIsActiveChanged);
            }
        }

        #endregion IsActive

        public void Activate()
        {
            if (IsActive)
            {
                return;
            }

            Load();

            IsActive = true;
            this.Refresh();

            OnActivated(EventArgs.Empty);
        }

        protected virtual void OnActivated(EventArgs e)
        {
            var handler = Activated;
            handler?.Invoke(this, e);
        }

        public event EventHandler<DesactivationEventArgs> AttemptingDesactivation;

        public void Desactivate()
        {
            ((IActivable)this).Desactivate(false);
        }

       void IActivable.Desactivate(bool close)
        {
            if (!this.IsActive && !close)
            {
                return;
            }

            var args = close ? closeDesactivationArgs : hideDesactivationArgs;

            OnAttemptingDesactivation(args);

            this.IsActive = false;

            OnDesactivated(args);
        }

        private static readonly DesactivationEventArgs closeDesactivationArgs = new DesactivationEventArgs { WasClosed = true };
        private static readonly DesactivationEventArgs hideDesactivationArgs = new DesactivationEventArgs { WasClosed = true };

        protected virtual void OnAttemptingDesactivation(DesactivationEventArgs e)
        {
            var handler = AttemptingDesactivation;
            handler?.Invoke(this, e);
        }

        protected virtual void OnDesactivated(DesactivationEventArgs e)
        {
            var handler = Desactivated;
            handler?.Invoke(this, e);
        }

        public event EventHandler<DesactivationEventArgs> Desactivated;

        protected virtual void LoadData()
        {
            Log.DebugFormat("Loading {0}.", this);
        }

        private object loaded;

        public bool IsLoaded => this.loaded != null;

        public void Load()
        {
            LazyInitializer.EnsureInitialized(
                ref loaded,
                () =>
                    {
                        this.LoadData();
                        Tracking = true;
                        return this;
                    });
        }

        private int closingSemaphore;

        public void TryClose(bool? dialogResult = null)
        {
            //var conductor = Parent as IConductor;
            //if (conductor != null)
            //{
            //    conductor.CloseItem(this);
            //}
            //else
            //{
            var closing = Interlocked.CompareExchange(ref this.closingSemaphore, 1, 0) == 1;

            if (closing)
            {
                return;
            }

            OnClosing(EventArgs.Empty);

            Tracking = false;

            ((IActivable)this).Desactivate(true);

            // Unsubscribe to message bus
            Infrastructure.MessageBus.Unsubscribe(this);

            //Commands.SafeDispose();

            Log.DebugFormat("Closed {0}.", this);

            this.DialogResultSetter?.Invoke(dialogResult);

            OnClosed(EventArgs.Empty);
            // }
        }

        protected virtual void OnClosing(EventArgs e)
        {
            var handler = Closing;
            handler?.Invoke(this, e);
        }

        protected virtual void OnClosed(EventArgs e)
        {
            var handler = Closed;
            handler?.Invoke(this, e);
        }

        public event EventHandler Closing;

        public event EventHandler Closed;

        public Action<bool?> DialogResultSetter { get; set; }

        public virtual void CanClose(Action<bool> callback)
        {
            callback(true);
        }
    }
}