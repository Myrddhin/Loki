using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;

using Loki.Common;

namespace Loki.Commands
{
    /// <summary>
    /// Command manager class.
    /// </summary>
    public class CommandManager : IDisposable
    {
        /// <summary>
        /// The Always functor (for can execute test).
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="Loki.Commands.CanExecuteCommandEventArgs"/> instance containing
        /// the event data.
        /// </param>
        public static void Always(object sender, CanExecuteCommandEventArgs e)
        {
            e.CanExecute = true;
        }

        private ICommandAware target;

        public void SetTarget(ICommandAware specifiedTarget)
        {
            if (target != null)
            {
                Events.PropertyChanging.Unregister(target, "State", this);
                Events.PropertyChanged.Unregister(target, "State", this);
            }

            this.target = specifiedTarget;

            Events.PropertyChanging.Register(target, "State", this, (v, o, e) => v.StateChanging(o, e));
            Events.PropertyChanged.Register(target, "State", this, (v, o, e) => v.StateChanged(o, e));

            Events.Changed.Register(target.State, this, (v, o, ev) => v.StatePropertyChanged(o, ev));
            handlers.Keys.Apply(x => x.RefreshState());
        }

        public ICommandComponent Commands
        {
            get;
            set;
        }

        public IEventComponent Events
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandManager"/> class.
        /// </summary>
        public CommandManager()
        {
            handlers = new ConcurrentDictionary<ICommand, List<ICommandHandler>>();
        }

        private void StateChanging(object sender, PropertyChangingEventArgs e)
        {
            Events.Changed.Unregister(target.State, this);
        }

        private void StateChanged(object sender, PropertyChangedEventArgs e)
        {
            Events.Changed.Register(target.State, this, (v, o, ev) => v.StatePropertyChanged(o, ev));
        }

        private void StatePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            handlers.Keys.Apply(x => x.RefreshState());
        }

        public void RefreshState()
        {
            handlers.Keys.Apply(x => x.RefreshState());
        }

        #region Commands

        /// <summary>
        /// Creates the specified command.
        /// </summary>
        /// <returns>
        /// </returns>
        public ICommand Create()
        {
            return Create(Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Creates the command with the specified name.
        /// </summary>
        /// <param name="commandName">
        /// Name of the command.
        /// </param>
        /// <returns>
        /// </returns>
        public ICommand Create(string commandName)
        {
            if (Toolkit.UI.Windows.DesignMode)
            {
                return new LokiRelayCommand(() => { });
            }
            else
            {
                return Commands.CreateCommand(commandName);
            }
        }

        #endregion Commands

        #region Handlers

        public void Handle(ICommand command, Action<object, CanExecuteCommandEventArgs> actionCanExecute, Action<object, CommandEventArgs> actionExecute, Func<CommandEventArgs, bool> confirmDelegate)
        {
            if (!handlers.ContainsKey(command))
            {
                handlers[command] = new List<ICommandHandler>();
            }

            handlers[command].Add(Commands.CreateHandler(command, actionCanExecute, actionExecute, target, confirmDelegate));
        }

        public void Handle(ICommand command, Action<object, CanExecuteCommandEventArgs> actionCanExecute, Action<object, CommandEventArgs> actionExecute)
        {
            Handle(command, actionCanExecute, actionExecute, null);
        }

        public void Handle(ICommand command, Action<object, CommandEventArgs> actionExecute)
        {
            Handle(command, CommandManager.Always, actionExecute, null);
        }

        public void Handle(ICommand command, Action<object, CommandEventArgs> actionExecute, Func<CommandEventArgs, bool> confirmDelegate)
        {
            Handle(command, CommandManager.Always, actionExecute, confirmDelegate);
        }

        public void UnHandle(ICommand command)
        {
            if (handlers.ContainsKey(command))
            {
                foreach (ICommandHandler item in handlers[command])
                {
                    Commands.RemoveHandler(command, item);
                }

                List<ICommandHandler> removed = null;
                handlers.TryRemove(command, out removed);
            }
        }

        private readonly ConcurrentDictionary<ICommand, List<ICommandHandler>> handlers;

        private void ClearHandlers()
        {
            foreach (KeyValuePair<ICommand, List<ICommandHandler>> handler in handlers)
            {
                foreach (ICommandHandler item in handler.Value)
                {
                    Commands.RemoveHandler(handler.Key, item);
                }
            }

            handlers.Clear();
        }

        #endregion Handlers

        #region Dispose

        /// <summary>
        /// Releases all resources used by an instance of the <see cref="CommandManager" /> class.
        /// </summary>
        /// <remarks>
        /// This method calls the virtual <see cref="Dispose(bool)" /> method, passing in
        /// <strong>true</strong>, and then suppresses finalization of the instance.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CommandManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the unmanaged resources used by an instance of the
        /// <see cref="CommandManager"/> class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// Set <strong>true</strong> to release both managed and
        /// unmanaged resources; <strong>false</strong> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearHandlers();
                target = null;
            }
        }

        #endregion Dispose
    }
}