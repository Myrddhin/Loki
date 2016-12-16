using System;
using System.Collections.Generic;

#if WPF

using System.Windows.Input;

#endif

namespace Loki.UI.Commands
{
    /// <summary>
    /// Common interface for command manager service.
    /// </summary>
    public interface ICommandManager
    {
        /// <summary>
        /// Creates and register a command bind for the specified command.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="handler">
        /// The command handler
        /// </param>
        /// <param name="canExecuteFunction">
        /// The CanExecute function getter (from the handler).
        /// </param>
        /// <param name="executeFunction">
        /// The Execute function getter (from the handler).
        /// </param>
        /// <typeparamref name="T">Handler type</typeparamref>
        /// <returns>
        /// A disposable link between the command and the handler.
        /// </returns>
        /// <typeparam name="T">Handler type.</typeparam>
        ICommandBind CreateBind<T>(
            ICommand command,
            T handler,
            Func<T, Action<object, CanExecuteCommandEventArgs>> canExecuteFunction,
            Func<T, Action<object, CommandEventArgs>> executeFunction)
            where T : class;

        /// <summary>
        /// Creates and register a command bind for the specified command.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="handler">
        /// The command handler
        /// </param>
        /// <param name="canExecuteFunction">
        /// The CanExecute function getter (from the handler).
        /// </param>
        /// <param name="executeFunction">
        /// The Execute function getter (from the handler).
        /// </param>
        /// <param name="confirmDelegate">
        /// The confirm delegate getter (from the handler).
        /// </param>
        /// <returns>
        /// A disposable link between the command and the handler.
        /// </returns>
        /// <typeparam name="T">Handler type.</typeparam>
        ICommandBind CreateBind<T>(
            ICommand command,
            T handler,
            Func<T, Action<object, CanExecuteCommandEventArgs>> canExecuteFunction,
            Func<T, Action<object, CommandEventArgs>> executeFunction,
            Func<T, Func<CommandEventArgs, bool>> confirmDelegate)
            where T : class;

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
        IEnumerable<ICommandBind> GetHandlers(ICommand command);
    }
}