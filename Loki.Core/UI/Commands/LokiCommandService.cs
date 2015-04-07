using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Loki.Common;

namespace Loki.Commands
{
    /// <summary>
    /// Loki default command service.
    /// </summary>
    public class LokiCommandService : BaseObject, ICommandComponent
    {
        #region Private storage

        private ConcurrentDictionary<string, ConcurrentCollection<WeakReference<ICommandHandler>>> commandHandlers = null;

        #endregion Private storage

        #region Handlers management

        /// <summary>
        /// Gets the handlers.
        /// </summary>
        /// <param name="command">The command.</param>
        public IEnumerable<ICommandHandler> GetHandlers(ICommand command)
        {
            // initialize return
            List<ICommandHandler> buffer = null;

            // remove dead references
            CleanReferences();

            // get handlers
            ConcurrentCollection<WeakReference<ICommandHandler>> currentHandlers = null;
            if (commandHandlers.TryGetValue(command.Name, out currentHandlers))
            {
                buffer = new List<ICommandHandler>();
                foreach (var handlerReference in currentHandlers)
                {
                    ICommandHandler target = null;
                    if (handlerReference.Value.TryGetTarget(out target))
                    {
                        buffer.Add(target);
                    }
                }
            }

            // return
            return buffer;
        }

        private void CleanReferences()
        {
            foreach (ConcurrentCollection<WeakReference<ICommandHandler>> item in commandHandlers.Values)
            {
                var toRemove = new List<IListNode<WeakReference<ICommandHandler>>>();
                foreach (var handlerReference in item)
                {
                    ICommandHandler target = null;
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

            List<string> deadHandlersKeys = commandHandlers.Where(x => x.Value.Count() == 0).Select(x => x.Key).ToList();
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
        /// <param name="command">The command.</param>
        /// <param name="handler">The command handler.</param>
        public void AddHandler(ICommand command, ICommandHandler handler)
        {
            var currentHandlerList = commandHandlers.GetOrAdd(command.Name, new ConcurrentCollection<WeakReference<ICommandHandler>>());

            currentHandlerList.Add(new WeakReference<ICommandHandler>(handler));
        }

        /// <summary>
        /// Creates and register a command handler fo the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="canExecuteFunction">The CanExecute function.</param>
        /// <param name="executeFunction">The Execute function.</param>
        /// <param name="state">The command state handler.</param>
        /// <param name="confirmDelegate">The confirm delegate.</param>
        /// <returns>You must hold a reference on the returned command handler as long as the
        /// listener observes the command.</returns>
        public ICommandHandler CreateHandler(ICommand command, Action<object, CanExecuteCommandEventArgs> canExecuteFunction, Action<object, CommandEventArgs> executeFunction, ICommandAware state, Func<CommandEventArgs, bool> confirmDelegate)
        {
            LokiCommandHandler returnHandler = null;
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

        public ICommandHandler CreateHandler(ICommand command, Action<object, CanExecuteCommandEventArgs> canExecuteFunction, Action<object, CommandEventArgs> executeFunction, ICommandAware state)
        {
            return CreateHandler(command, canExecuteFunction, executeFunction, state, null);
        }

        public ICommandHandler CreateHandler(ICommand command, Action<object, CanExecuteCommandEventArgs> canExecuteFunction, Action<object, CommandEventArgs> executeFunction)
        {
            return CreateHandler(command, canExecuteFunction, executeFunction, null);
        }

        /// <summary>
        /// Removes the specified <see cref="ICommandHandler"/> for the specified command.
        /// </summary>
        /// <param name="command">The command name.</param>
        /// <param name="handler">The command handler.</param>
        public void RemoveHandler(ICommand command, ICommandHandler handler)
        {
            ConcurrentCollection<WeakReference<ICommandHandler>> currentHandlerList = null;
            if (commandHandlers.TryGetValue(command.Name, out currentHandlerList))
            {
                IListNode<WeakReference<ICommandHandler>> reference = null;
                foreach (var handlerReference in currentHandlerList)
                {
                    ICommandHandler target = null;
                    if (handlerReference.Value.TryGetTarget(out target))
                    {
                        if (target == handler)
                        {
                            reference = handlerReference;
                            break;
                        }
                    }
                }

                if (reference != null)
                {
                    currentHandlerList.Remove(reference);
                }
            }

            command.RefreshState();
        }

        #endregion Handler registering

        public IEventComponent Events { get; set; }

        public IMessageComponent MessageBus { get; set; }

        #region Command registering

        /// <summary>
        /// Creates and registers a new command for the specified command name.
        /// </summary>
        /// <param name="commandTag">Name of the command.</param>
        /// <returns>The registered command.</returns>
        public ICommand CreateCommand(string commandTag)
        {
            var command = new LokiRoutedCommand();
            command.CommandService = this;
            command.MessageBus = MessageBus;
            command.Name = commandTag;
            return command;
        }

        /// <summary>
        /// Creates and registers a new command for the specified command name.
        /// </summary>
        /// <returns>The registered command.</returns>
        public ICommand CreateCommand()
        {
            return CreateCommand(Guid.NewGuid().ToString());
        }

        #endregion Command registering

        /// <summary>
        /// Initializes a new instance of the <see cref="LokiCommandService"/> class.
        /// </summary>
        public LokiCommandService()
        {
            commandHandlers = new ConcurrentDictionary<string, ConcurrentCollection<WeakReference<ICommandHandler>>>();
        }
    }
}