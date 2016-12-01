using System;
using System.Threading;
using System.Threading.Tasks;

using Loki.UI.Commands;

#if WPF
using System.Windows.Input;
#endif

namespace Loki.UI.Models
{
    public class Screen : DisplayUnit, ILoadable, IGuardClose
    {
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
            this.SubscribtionStore.Add(subscription);
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

        public Screen(IDisplayInfrastructure infrastructure)
            : base(infrastructure)
        {
        }
    }
}