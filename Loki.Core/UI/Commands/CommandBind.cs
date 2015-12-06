using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;

using Loki.Commands;

namespace Loki.UI.Commands
{
    public class CommandBind
    {
        public CommandBind(INotifyPropertyChanged target, IDisplayServices services)
        {
            this.target = target;
            this.services = services;
            handlers = new ConcurrentDictionary<ICommand, List<ICommandHandler>>();

            services.Core.Events.Changed.Register(target, this, (v, o, ev) => v.RefreshState());
        }

        private readonly INotifyPropertyChanged target;

        private readonly IDisplayServices services;

        private readonly ConcurrentDictionary<ICommand, List<ICommandHandler>> handlers;

        public void Unbind()
        {
            foreach (KeyValuePair<ICommand, List<ICommandHandler>> handler in handlers)
            {
                foreach (ICommandHandler item in handler.Value)
                {
                    services.UI.Commands.RemoveHandler(handler.Key, item);
                }
            }

            handlers.Clear();
        }

        /// <summary>
        /// Creates the command with the specified name.
        /// </summary>
        /// <param name="commandName">
        /// Name of the command.
        /// </param>
        /// <returns>
        /// The command.
        /// </returns>
        public ICommand Get(string commandName)
        {
            return services.UI.Windows.DesignMode ? new LokiRelayCommand(() => { }) : this.services.UI.Commands.GetCommand(commandName);
        }

        public void UnHandle(ICommand command)
        {
            if (handlers.ContainsKey(command))
            {
                foreach (ICommandHandler item in handlers[command])
                {
                    services.UI.Commands.RemoveHandler(command, item);
                }

                List<ICommandHandler> removed;
                handlers.TryRemove(command, out removed);
            }
        }

        /// <summary>
        /// Creates the specified command.
        /// </summary>
        /// <returns>
        /// The command.
        /// </returns>
        public ICommand Create()
        {
            return Get(Guid.NewGuid().ToString());
        }

        public void Handle(ICommand command, Action<object, CanExecuteCommandEventArgs> actionCanExecute, Action<object, CommandEventArgs> actionExecute, Func<CommandEventArgs, bool> confirmDelegate)
        {
            if (!handlers.ContainsKey(command))
            {
                handlers[command] = new List<ICommandHandler>();
            }

            handlers[command].Add(services.UI.Commands.CreateHandler(command, actionCanExecute, actionExecute, target, confirmDelegate));
        }

        public void Handle(ICommand command, Action<object, CanExecuteCommandEventArgs> actionCanExecute, Action<object, CommandEventArgs> actionExecute)
        {
            Handle(command, actionCanExecute, actionExecute, null);
        }

        public void Handle(ICommand command, Action<object, CommandEventArgs> actionExecute)
        {
            Handle(command, Always, actionExecute, null);
        }

        public void Handle(ICommand command, Action<object, CommandEventArgs> actionExecute, Func<CommandEventArgs, bool> confirmDelegate)
        {
            Handle(command, Always, actionExecute, confirmDelegate);
        }

        public void RefreshState()
        {
            handlers.Keys.Apply(x => x.RefreshState());
        }

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
    }
}