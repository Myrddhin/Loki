using System;

namespace Loki.Commands
{
    /// <summary>
    /// Interface for loki command handlers.
    /// </summary>
    public interface ICommandHandler : IEquatable<ICommandHandler>
    {
        /// <summary>
        /// Gets the CanExecute command handler.
        /// </summary>
        EventHandler<CanExecuteCommandEventArgs> CanExecute { get; }

        /// <summary>
        /// Gets the command target.
        /// </summary>
        object Target { get; }

        /// <summary>
        /// Gets the Execute command handler.
        /// </summary>
        EventHandler<CommandEventArgs> Execute { get; }

        /// <summary>
        /// Gets the user confirmation delegate.
        /// </summary>
        Func<CommandEventArgs, bool> ConfirmDelegate { get; }
    }
}