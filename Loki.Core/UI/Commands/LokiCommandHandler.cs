using System;

namespace Loki.Commands
{
    /// <summary>
    /// Loki command handlers.
    /// </summary>
    /// <remarks>Command handlers are stored as weak references in command service. Use this class and hold a reference on it in your command listener class.</remarks>
    public sealed class LokiCommandHandler : ICommandHandler
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="LokiCommandHandler"/> class from being created.
        /// </summary>
        private LokiCommandHandler()
        {
        }

        /// <summary>
        /// Creates the specified can execute function.
        /// </summary>
        /// <param name="canExecuteFunction">The can execute function.</param>
        /// <param name="executeFunction">The execute function.</param>
        public static LokiCommandHandler Create(
            Action<object, CanExecuteCommandEventArgs> canExecuteFunction,
            Action<object, CommandEventArgs> executeFunction)
        {
            return Create(canExecuteFunction, executeFunction, null, null);
        }

        /// <summary>
        /// Creates the specified can execute function.
        /// </summary>
        /// <param name="canExecuteFunction">The can execute function.</param>
        /// <param name="executeFunction">The execute function.</param>
        /// <param name="state">The state.</param>
        public static LokiCommandHandler Create(
            Action<object, CanExecuteCommandEventArgs> canExecuteFunction,
            Action<object, CommandEventArgs> executeFunction,
            object state)
        {
            return Create(canExecuteFunction, executeFunction, state, null);
        }

        /// <summary>
        /// Creates a new command handler with the specified delegates.
        /// </summary>
        /// <param name="canExecuteFunction">The can execute functor.</param>
        /// <param name="executeFunction">The execute functor.</param>
        /// <param name="state">The state callback.</param>
        /// <param name="confirmDelegate">The confirm functor.</param>
        public static LokiCommandHandler Create(
            Action<object, CanExecuteCommandEventArgs> canExecuteFunction,
            Action<object, CommandEventArgs> executeFunction,
            object state,
            Func<CommandEventArgs, bool> confirmDelegate)
        {
            LokiCommandHandler tempHandler = new LokiCommandHandler();
            try
            {
                tempHandler.CanExecute = new EventHandler<CanExecuteCommandEventArgs>(canExecuteFunction);
                tempHandler.Execute = new EventHandler<CommandEventArgs>(executeFunction);
                tempHandler.Target = state;
                tempHandler.ConfirmDelegate = confirmDelegate;
                return tempHandler;
            }
            catch
            {
                tempHandler.Cut();
                throw;
            }
        }

        /// <summary>
        /// Gets the CanExecute command handler.
        /// </summary>
        public EventHandler<CanExecuteCommandEventArgs> CanExecute
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Execute command handler.
        /// </summary>
        public EventHandler<CommandEventArgs> Execute
        {
            get;
            private set;
        }

        public object Target
        {
            get;
            private set;
        }

        public Func<CommandEventArgs, bool> ConfirmDelegate
        {
            get;
            private set;
        }

        public void Cut()
        {
            Execute = null;
            CanExecute = null;
            Target = null;
        }

        #region IEquatable<ICommandHandler> Members

        public bool Equals(ICommandHandler other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                return other.CanExecute == CanExecute
                    && other.Execute == Execute
                    && other.Target == Target
                    && other.ConfirmDelegate == ConfirmDelegate;
            }
        }

        #endregion IEquatable<ICommandHandler> Members
    }
}