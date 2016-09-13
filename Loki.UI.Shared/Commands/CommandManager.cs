using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Loki.Common;
#if WPF
using System.Windows.Input;
#endif

namespace Loki.UI.Commands
{
    /// <summary>
    /// Loki default command service.
    /// </summary>
    public class CommandManager : BaseService, ICommandManager
    {
        #region Private storage

        private readonly ConcurrentDictionary<WeakKeyReference<ICommand>, ConcurrentCollection<ICommandBind>> commandBinds;

        private readonly ConcurrentDictionary<string, ILokiCommand> namedCommands;

        private readonly IInfrastructure services;

        private readonly WeakKeyComparer<ICommand> keyComparer;

        #endregion Private storage

        #region Handlers management

        /// <summary>
        /// Gets the command binds.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <returns>
        /// The command binds.
        /// </returns>
        public IEnumerable<ICommandBind> GetHandlers(ICommand command)
        {
            // remove dead references
            CleanReferences();

            // initialize return
            var buffer = new List<ICommandBind>();
            var virtualKey = new WeakKeyReference<ICommand>(command, keyComparer);

            // get handlers
            ConcurrentCollection<ICommandBind> currentBinds;
            if (!commandBinds.TryGetValue(virtualKey, out currentBinds))
            {
                return buffer;
            }

            foreach (var bindReference in currentBinds)
            {
                if (bindReference.Value.Active)
                {
                    buffer.Add(bindReference.Value);
                }
            }

            // return
            return buffer;
        }

        private void CleanReferences()
        {
            foreach (var item in commandBinds.Values)
            {
                var toRemove = new List<IListNode<ICommandBind>>();
                foreach (var handlerReference in item)
                {
                    if (!handlerReference.Value.Alive)
                    {
                        toRemove.Add(handlerReference);
                    }
                }

                foreach (var deadItem in toRemove)
                {
                    deadItem.Value.Dispose();
                    item.Remove(deadItem);
                }
            }

            var deadHandlersKeys = this.commandBinds.Where(x => !x.Value.Any()).Select(x => x.Key).ToList();
            foreach (var item in deadHandlersKeys)
            {
                ConcurrentCollection<ICommandBind> buffer;
                commandBinds.TryRemove(item, out buffer);
            }
        }

        #endregion Handlers management

        #region Handler registering

        public ICommandBind CreateBind<T>(
            ICommand command,
            T handler,
            Func<T, Action<object, CanExecuteCommandEventArgs>> canExecuteFunction,
            Func<T, Action<object, CommandEventArgs>> executeFunction)
            where T : class
        {
            return CreateBind(command, handler, canExecuteFunction, executeFunction, null);
        }

        public ICommandBind CreateBind<T>(
            ICommand command,
            T handler,
            Func<T, Action<object, CanExecuteCommandEventArgs>> canExecuteFunction,
            Func<T, Action<object, CommandEventArgs>> executeFunction,
            Func<T, Func<CommandEventArgs, bool>> confirmDelegate) where T : class
        {
            var bind = new CommandBind<T>(
                command,
                handler,
                canExecuteFunction,
                executeFunction,
                confirmDelegate);

            var virtualKey = new WeakKeyReference<ICommand>(command, keyComparer);
            var buffer = new ConcurrentCollection<ICommandBind> { bind };

            this.commandBinds.AddOrUpdate(virtualKey, k => buffer, (k, o) => { o.Add(bind); return o; });

            return bind;
        }

        #endregion Handler registering

        #region Command registering

        /// <summary>
        /// Creates and registers a new command for the specified command name.
        /// </summary>
        /// <param name="commandName">
        /// Name of the command.
        /// </param>
        /// <returns>
        /// The registered command.
        /// </returns>
        public ICommand GetCommand(string commandName)
        {
            var newCommand = new LokiRoutedCommand(commandName, services.Diagnostics, this, services.MessageBus);
            return namedCommands.GetOrAdd(commandName, newCommand);
        }

        #endregion Command registering

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandManager"/> class.
        /// </summary>
        /// <param name="infrastructure">
        /// Core services.
        /// </param>
        public CommandManager(IInfrastructure infrastructure)
            : base(infrastructure)
        {
            keyComparer = new WeakKeyComparer<ICommand>();
            this.commandBinds = new ConcurrentDictionary<WeakKeyReference<ICommand>, ConcurrentCollection<ICommandBind>>(keyComparer);
            namedCommands = new ConcurrentDictionary<string, ILokiCommand>();
            services = infrastructure;
        }
    }
}