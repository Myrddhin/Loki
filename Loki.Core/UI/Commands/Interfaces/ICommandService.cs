using System;
using System.Collections.Generic;

namespace Loki.Commands
{
    /// <summary>
    /// Common interface for command manager service.
    /// </summary>
    public interface ICommandComponent
    {
        /// <summary>
        /// Creates and register a command handler fo the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="canExecuteFunction">The CanExecute function.</param>
        /// <param name="executeFunction">The Execute function.</param>
        /// <returns>You must hold a reference on the returned command handler as long as the listener observes the command.</returns>
        ICommandHandler CreateHandler(ICommand command, Action<object, CanExecuteCommandEventArgs> canExecuteFunction, Action<object, CommandEventArgs> executeFunction);

        /// <summary>
        /// Creates and register a command handler fo the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="canExecuteFunction">The can execute function.</param>
        /// <param name="executeFunction">The execute function.</param>
        /// <param name="state">The state.</param>
        /// <returns>You must hold a reference on the returned command handler as long as the listener observes the command.</returns>
        ICommandHandler CreateHandler(ICommand command, Action<object, CanExecuteCommandEventArgs> canExecuteFunction, Action<object, CommandEventArgs> executeFunction, ICommandAware state);

        /// <summary>
        /// Creates and register a command handler fo the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="canExecuteFunction">The can execute function.</param>
        /// <param name="executeFunction">The execute function.</param>
        /// <param name="state">The state.</param>
        /// <param name="confirmDelegate">The confirm delegate.</param>
        /// <returns>You must hold a reference on the returned command handler as long as the listener observes the command.</returns>
        ICommandHandler CreateHandler(ICommand command, Action<object, CanExecuteCommandEventArgs> canExecuteFunction, Action<object, CommandEventArgs> executeFunction, ICommandAware state, Func<CommandEventArgs, bool> confirmDelegate);

        /// <summary>
        /// Adds the specified <see cref="ICommandHandler"/> for the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="handler">The command handler.</param>
        void AddHandler(ICommand command, ICommandHandler handler);

        /// <summary>
        /// Removes the specified <see cref="ICommandHandler"/> for the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="handler">The command handler.</param>
        void RemoveHandler(ICommand command, ICommandHandler handler);

        /// <summary>
        /// Creates and registers a new command for the specified command name.
        /// </summary>
        /// <param name="commandTag">Name of the command.</param>
        /// <returns>The registered command.</returns>
        ICommand CreateCommand(string commandTag);

        /// <summary>
        /// Creates and registers a new command for the specified command name.
        /// </summary>
        ICommand CreateCommand();

        /// <summary>
        /// Gets the handlers.
        /// </summary>
        /// <param name="command">The command.</param>
        IEnumerable<ICommandHandler> GetHandlers(ICommand command);
    }
}