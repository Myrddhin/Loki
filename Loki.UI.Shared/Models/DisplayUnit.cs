using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Threading;

using Loki.Common.Diagnostics;

namespace Loki.UI.Models
{
    public class DisplayUnit : StateUnit, IHaveDisplayName, IActivable, ICloseable
    {
        private static readonly PropertyChangedEventArgs ArgsDisplayNameChanged = new PropertyChangedEventArgs(nameof(DisplayName));

        private readonly Lazy<ILog> log;
        private int closingSemaphore;
        private string displayName;

        public DisplayUnit(IDisplayInfrastructure infrastructure)
        {
            Tracking = false;

            Infrastructure = infrastructure;

            SubscribtionStore = new CompositeDisposable();

            // Log.
            this.log = new Lazy<ILog>(() => infrastructure.Diagnostics.GetLog(this.GetType().FullName));

            // Message bus subscription.
            Infrastructure.MessageBus.Subscribe(this);
        }

        public event EventHandler Activating;

        public event EventHandler Activated;

        public event EventHandler Desactivating;

        public event EventHandler Closed;

        public event EventHandler Closing;

        public event EventHandler Desactivated;

        public Action<bool?> DialogResultSetter { get; set; }

        public string DisplayName
        {
            get
            {
                return this.displayName;
            }

            set
            {
                if (value == this.displayName)
                {
                    return;
                }

                this.displayName = value;
                this.NotifyChanged(ArgsDisplayNameChanged);
            }
        }

        protected IDisplayInfrastructure Infrastructure { get; }

        protected ILog Log => this.log.Value;

        public virtual void Activate()
        {
            if (IsActive)
            {
                return;
            }

            OnActivating(EventArgs.Empty);

            IsActive = true;
            this.Refresh();

            OnActivated(EventArgs.Empty);
        }

        public void Desactivate()
        {
            if (!this.IsActive)
            {
                return;
            }

            var args = EventArgs.Empty;

            this.OnDesactivating(args);

            this.IsActive = false;

            OnDesactivated(args);
        }

        public void TryClose(bool? dialogResult = null)
        {
            //// var conductor = Parent as IConductor;
            //// if (conductor != null)
            //// {
            ////    conductor.CloseItem(this);
            //// }
            //// else
            //// {
            var closing = Interlocked.CompareExchange(ref this.closingSemaphore, 1, 0) == 1;

            if (closing)
            {
                return;
            }

            OnClosing(EventArgs.Empty);

            Tracking = false;

            ((IActivable)this).Desactivate();

            // Unsubscribe to message bus
            Infrastructure.MessageBus.Unsubscribe(this);

            //// Commands.SafeDispose();

            Log.DebugFormat("Closed {0}.", this);

            this.DialogResultSetter?.Invoke(dialogResult);

            OnClosed(EventArgs.Empty);
            ////}
        }

        protected virtual void OnActivating(EventArgs e)
        {
            var handler = Activating;
            handler?.Invoke(this, e);
        }

        protected virtual void OnActivated(EventArgs e)
        {
            var handler = Activated;
            handler?.Invoke(this, e);
        }

        protected virtual void OnDesactivating(EventArgs e)
        {
            var handler = Desactivating;
            handler?.Invoke(this, e);
        }

        protected virtual void OnClosed(EventArgs e)
        {
            var handler = Closed;
            handler?.Invoke(this, e);
        }

        protected virtual void OnClosing(EventArgs e)
        {
            var handler = Closing;
            handler?.Invoke(this, e);
        }

        #region IDisposable Support

        private bool disposedValue;

        protected CompositeDisposable SubscribtionStore { get; }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                this.SubscribtionStore.Dispose();
            }

            this.disposedValue = true;
        }

        #endregion IDisposable Support

        #region IsActive

        private static readonly PropertyChangedEventArgs ArgsIsActiveChanged = new PropertyChangedEventArgs(nameof(IsActive));

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
                this.NotifyChanged(ArgsIsActiveChanged);
            }
        }

        #endregion IsActive

        protected virtual void OnDesactivated(EventArgs e)
        {
            var handler = Desactivated;
            handler?.Invoke(this, e);
        }
    }
}