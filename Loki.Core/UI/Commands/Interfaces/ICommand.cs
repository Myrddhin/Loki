﻿namespace Loki.Commands
{
    /// <summary>
    /// Common interface for Loki commands.
    /// </summary>
    public interface ICommand : INotifyCanExecuteChanged, System.Windows.Input.ICommand
    {
        /// <summary>
        /// Gets the command name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Refreshes the state.
        /// </summary>
        void RefreshState();
    }
}