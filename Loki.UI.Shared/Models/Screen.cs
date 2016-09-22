using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

using Loki.Common.Diagnostics;
using Loki.UI.Commands;

#if WPF
using System.Windows.Input;
#endif

namespace Loki.UI.Models
{
    public class Screen : DisplayUnit, IActivable, ILoadable, IGuardClose, IDisposable
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

        protected virtual Task LoadData()
        {
            return Task.Delay(0);
        }

        private object loaded;

        public bool IsLoaded => this.loaded != null;

        public void Load()
        {
            LazyInitializer.EnsureInitialized(
                ref loaded,
                () =>
                    {
                        this.InternalLoad();
                        return this;
                    });
        }

        private async void InternalLoad()
        {
            Log.DebugFormat("Loading {0}.", this);
            await this.LoadData();
            Log.DebugFormat(" End loading {0}.", this);
            this.AcceptChanges();
            Tracking = true;
            this.Refresh();
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

        protected void BindCommand<T>(
            ICommand command,
            Func<T, Action<object, CanExecuteCommandEventArgs>> canExecuteHandler,
            Func<T, Action<object, CommandEventArgs>> executeHandler)
            where T : Screen
        {
            var subscription = Infrastructure.CommandsManager.CreateBind(command, (T)this, canExecuteHandler, executeHandler);
            this.disposables.Add(subscription);
        }

        protected ICommand GetOrCreateCommand()
        {
            return this.GetOrCreateCommand(Guid.NewGuid().ToString());
        }

        protected ICommand GetOrCreateCommand(string commandName)
        {
            string name = $"{this.GetType().FullName}.{commandName}";
            return Infrastructure.CommandsManager.GetCommand(name);
        }

        #region IDisposable Support

        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                this.disposables.Dispose();
            }

            this.disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}