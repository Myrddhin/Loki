using System;

namespace Loki.UI.Commands
{
    /// <summary>
    /// Provides arguments for the CanExecute command event.
    /// </summary>
    [Serializable]
    public class CanExecuteCommandEventArgs : CommandEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the command could be executed.
        /// </summary>
        /// <value>
        /// Is <c>true</c> if the command could be executed; otherwise, <c>false</c>.
        /// </value>
        public bool CanExecute { get; set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CanExecuteCommandEventArgs"/> class.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public CanExecuteCommandEventArgs(ICommand command, object parameter)
            : base(command, parameter)
        {
        }

        #endregion Constructors
    }
}