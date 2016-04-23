using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;

using Loki.Common;

namespace Loki.UI.Commands
{
    public class CommandBind
    {
        public CommandBind(INotifyPropertyChanged target, ICommandComponent commands, IEventComponent events, IWindowManager windows)
        {
            this.target = target;
            this.commands = commands;
            this.windows = windows;
            handlers = new ConcurrentDictionary<ICommand, List<ICommandHandler>>();

            events.Changed.Register(target, this, (v, o, ev) => v.RefreshState());
        }

        private readonly INotifyPropertyChanged target;

        private readonly ICommandComponent commands;

        private readonly IWindowManager windows;

        private readonly ConcurrentDictionary<ICommand, List<ICommandHandler>> handlers;

        public void Unbind()
        {
            foreach (KeyValuePair<ICommand, List<ICommandHandler>> handler in handlers)
            {
                foreach (ICommandHandler item in handler.Value)
                {
                    commands.RemoveHandler(handler.Key, item);
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
            return windows.DesignMode ? new LokiRelayCommand(() => { }) : commands.GetCommand(commandName);
        }

        public void UnHandle(ICommand command)
        {
            if (handlers.ContainsKey(command))
            {
                foreach (ICommandHandler item in handlers[command])
                {
                    commands.RemoveHandler(command, item);
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

            handlers[command].Add(commands.CreateHandler(command, actionCanExecute, actionExecute, target, confirmDelegate));
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
        /// The <see cref="CanExecuteCommandEventArgs"/> instance containing
        /// the event data.
        /// </param>
        public static void Always(object sender, CanExecuteCommandEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}