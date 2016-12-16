using System;

#if WPF

using System.Windows.Input;

#endif

namespace Loki.UI.Commands
{
    /// <summary>
    /// Provides argument for command events.
    /// </summary>
    [Serializable]
    public class CommandEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the command.
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        /// Gets the command parameter.
        /// </summary>
        public object Parameter { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandEventArgs"/> class.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public CommandEventArgs(ICommand command, object parameter)
        {
            Command = command;
            Parameter = parameter;
        }
    }
}