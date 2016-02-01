using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Loki.Common;

namespace Loki.UI.Commands
{
    /// <summary>
    /// Loki default command service.
    /// </summary>
    public class LokiCommandService : BaseObject, ICommandComponent
    {
        #region Private storage

        private readonly ConcurrentDictionary<string, ConcurrentCollection<WeakReference<ICommandHandler>>> commandHandlers;

        private readonly ConcurrentDictionary<string, ICommand> commands;

        private readonly ICoreServices services;

        private readonly IThreadingContext threading;

        #endregion Private storage

        #region Handlers management

        /// <summary>
        /// Gets the handlers.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <returns>
        /// The command handlers.
        /// </returns>
        public IEnumerable<ICommandHandler> GetHandlers(ICommand command)
        {
            // remove dead references
            CleanReferences();

            // initialize return
            var buffer = new List<ICommandHandler>();

            // get handlers
            ConcurrentCollection<WeakReference<ICommandHandler>> currentHandlers;
            if (!this.commandHandlers.TryGetValue(command.Name, out currentHandlers))
            {
                return buffer;
            }

            foreach (var handlerReference in currentHandlers)
            {
                ICommandHandler target;
                if (!handlerReference.Value.TryGetTarget(out target))
                {
                    continue;
                }

                // continue if initializable target is not initialized.
                var initializableTarget = target.Target as IInitializable;
                if (initializableTarget != null && !initializableTarget.IsInitialized)
                {
                    continue;
                }

                // continue if activable target is not active.
                var activableTarget = target.Target as IActivable;
                if (activableTarget != null && !activableTarget.IsActive)
                {
                    continue;
                }

                buffer.Add(target);
            }

            // return
            return buffer;
        }

        private void CleanReferences()
        {
            foreach (var item in commandHandlers.Values)
            {
                var toRemove = new List<IListNode<WeakReference<ICommandHandler>>>();
                foreach (var handlerReference in item)
                {
                    ICommandHandler target;
                    if (!handlerReference.Value.TryGetTarget(out target))
                    {
                        toRemove.Add(handlerReference);
                    }
                }

                foreach (var deadItem in toRemove)
                {
                    item.Remove(deadItem);
                }
            }

            List<string> deadHandlersKeys = commandHandlers.Where(x => !x.Value.Any()).Select(x => x.Key).ToList();
            foreach (var item in deadHandlersKeys)
            {
                ConcurrentCollection<WeakReference<ICommandHandler>> buffer;
                commandHandlers.TryRemove(item, out buffer);
            }
        }

        #endregion Handlers management

        #region Handler registering

        /// <summary>
        /// Adds the specified <see cref="ICommandHandler"/> for the specified command.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="handler">
        /// The command handler.
        /// </param>
        private void AddHandler(ICommand command, ICommandHandler handler)
        {
            var currentHandlerList = commandHandlers.GetOrAdd(command.Name, new ConcurrentCollection<WeakReference<ICommandHandler>>());
            var state = handler.Target as INotifyPropertyChanged;
            if (state != null)
            {
                services.Events.Changed.Register(state, command, RefreshCommandState);
            }

            currentHandlerList.Add(new WeakReference<ICommandHandler>(handler));
        }

        private static void RefreshCommandState(ICommand command, object sender, PropertyChangedEventArgs e)
        {
            command.RefreshState();
        }

        /// <summary>
        /// Creates and register a command handler fo the specified command.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="canExecuteFunction">
        /// The CanExecute function.
        /// </param>
        /// <param name="executeFunction">
        /// The Execute function.
        /// </param>
        /// <param name="state">
        /// The command state handler.
        /// </param>
        /// <param name="confirmDelegate">
        /// The confirm delegate.
        /// </param>
        /// <returns>
        /// You must hold a reference on the returned command handler as long as the
        /// listener observes the command.
        /// </returns>
        public ICommandHandler CreateHandler(ICommand command, Action<object, CanExecuteCommandEventArgs> canExecuteFunction, Action<object, CommandEventArgs> executeFunction, INotifyPropertyChanged state, Func<CommandEventArgs, bool> confirmDelegate)
        {
            LokiCommandHandler returnHandler;
            LokiCommandHandler creationHandler = null;
            try
            {
                creationHandler = LokiCommandHandler.Create(canExecuteFunction, executeFunction, state, confirmDelegate);
                AddHandler(command, creationHandler);
                returnHandler = creationHandler;
                creationHandler = null;
            }
            finally
            {
                if (creationHandler != null)
                {
                    creationHandler.Cut();
                }
            }

            return returnHandler;
        }

        /// <summary>
        /// Creates and register a command handler fo the specified command.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="canExecuteFunction">
        /// The CanExecute function.
        /// </param>
        /// <param name="executeFunction">
        /// The Execute function.
        /// </param>
        /// <param name="state">
        /// The command state handler.
        /// </param>
        /// <returns>
        /// You must hold a reference on the returned command handler as long as the
        /// listener observes the command.
        /// </returns>
        public ICommandHandler CreateHandler(ICommand command, Action<object, CanExecuteCommandEventArgs> canExecuteFunction, Action<object, CommandEventArgs> executeFunction, INotifyPropertyChanged state)
        {
            return CreateHandler(command, canExecuteFunction, executeFunction, state, null);
        }

        /// <summary>
        /// Creates and register a command handler fo the specified command.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="canExecuteFunction">
        /// The CanExecute function.
        /// </param>
        /// <param name="executeFunction">
        /// The Execute function.
        /// </param>
        /// <returns>
        /// You must hold a reference on the returned command handler as long as the
        /// listener observes the command.
        /// </returns>
        public ICommandHandler CreateHandler(ICommand command, Action<object, CanExecuteCommandEventArgs> canExecuteFunction, Action<object, CommandEventArgs> executeFunction)
        {
            return CreateHandler(command, canExecuteFunction, executeFunction, null);
        }

        /// <summary>
        /// Removes the specified <see cref="ICommandHandler"/> for the specified command.
        /// </summary>
        /// <param name="command">
        /// The command name.
        /// </param>
        /// <param name="handler">
        /// The command handler.
        /// </param>
        public void RemoveHandler(ICommand command, ICommandHandler handler)
        {
            ConcurrentCollection<WeakReference<ICommandHandler>> currentHandlerList;
            if (commandHandlers.TryGetValue(command.Name, out currentHandlerList))
            {
                IListNode<WeakReference<ICommandHandler>> reference = null;
                foreach (var handlerReference in currentHandlerList)
                {
                    ICommandHandler target;
                    if (!handlerReference.Value.TryGetTarget(out target))
                    {
                        continue;
                    }

                    if (target != handler)
                    {
                        continue;
                    }

                    var state = target.Target as INotifyPropertyChanged;
                    if (state != null)
                    {
                        services.Events.Changed.Unregister(state, command);
                    }

                    reference = handlerReference;
                    break;
                }

                if (reference != null)
                {
                    currentHandlerList.Remove(reference);
                }
            }

            command.RefreshState();
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
            var newCommand = new LokiRoutedCommand(commandName, this.services.Logger, this, this.services.Messages, this.threading);
            return commands.GetOrAdd(commandName, newCommand);
        }

        #endregion Command registering

        /// <summary>
        /// Initializes a new instance of the <see cref="LokiCommandService"/> class.
        /// </summary>
        /// <param name="services">
        /// Core services.
        /// </param>
        /// <param name="threading">
        /// </param>
        public LokiCommandService(ICoreServices services, IThreadingContext threading)
            : base(services.Logger, services.Error)
        {
            commandHandlers = new ConcurrentDictionary<string, ConcurrentCollection<WeakReference<ICommandHandler>>>();
            commands = new ConcurrentDictionary<string, ICommand>();
            this.services = services;
            this.threading = threading;
        }
    }
}