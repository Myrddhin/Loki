using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Loki.UI.Commands
{
    /// <summary>
    /// Common interface for command manager service.
    /// </summary>
    public interface ICommandComponent
    {
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
        /// You must hold a reference on the returned command handler as long as the listener observes the command.
        /// </returns>
        ICommandHandler CreateHandler(ICommand command, Action<object, CanExecuteCommandEventArgs> canExecuteFunction, Action<object, CommandEventArgs> executeFunction);

        /// <summary>
        /// Creates and register a command handler fo the specified command.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="canExecuteFunction">
        /// The can execute function.
        /// </param>
        /// <param name="executeFunction">
        /// The execute function.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// You must hold a reference on the returned command handler as long as the listener observes the command.
        /// </returns>
        ICommandHandler CreateHandler(ICommand command, Action<object, CanExecuteCommandEventArgs> canExecuteFunction, Action<object, CommandEventArgs> executeFunction, INotifyPropertyChanged state);

        /// <summary>
        /// Creates and register a command handler fo the specified command.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="canExecuteFunction">
        /// The can execute function.
        /// </param>
        /// <param name="executeFunction">
        /// The execute function.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="confirmDelegate">
        /// The confirm delegate.
        /// </param>
        /// <returns>
        /// You must hold a reference on the returned command handler as long as the listener observes the command.
        /// </returns>
        ICommandHandler CreateHandler(ICommand command, Action<object, CanExecuteCommandEventArgs> canExecuteFunction, Action<object, CommandEventArgs> executeFunction, INotifyPropertyChanged state, Func<CommandEventArgs, bool> confirmDelegate);

        /// <summary>
        /// Removes the specified <see cref="ICommandHandler"/> for the specified command.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="handler">
        /// The command handler.
        /// </param>
        void RemoveHandler(ICommand command, ICommandHandler handler);

        /// <summary>
        /// Get the command with the specified command name.
        /// </summary>
        /// <param name="commandName">
        /// Name of the command.
        /// </param>
        /// <returns>
        /// The command.
        /// </returns>
        ICommand GetCommand(string commandName);

        /// <summary>
        /// Gets the handlers.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <returns>
        /// The handlers for the specified command.
        /// </returns>
        IEnumerable<ICommandHandler> GetHandlers(ICommand command);
    }
}