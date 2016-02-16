using System;

namespace Loki.UI.Commands
{
    /// <summary>
    /// Loki bridge command type.
    /// </summary>
    public class LokiRelayCommand : ICommand
    {
        public LokiRelayCommand(Func<bool> canExecuteCallBack, Action executeCallBack)
        {
            canExecute = canExecuteCallBack;
            execute = executeCallBack;
        }

        public LokiRelayCommand(Action execute)
            : this(() => true, execute)
        {
        }

        private readonly Func<bool> canExecute;

        private readonly Action execute;

        /// <summary>
        /// Définit la méthode qui détermine si la commande peut s'exécuter dans son état actuel.
        /// </summary>
        /// <param name="parameter">
        /// Données utilisées par la commande.Si la commande ne requiert
        /// pas que les données soient passées, cet objet peut avoir la valeur null.
        /// </param>
        /// <returns>
        /// True si cette commande peut être exécutée ; sinon false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            bool result = canExecute();
            if (lastCanExecute.HasValue && this.lastCanExecute.Value == result)
            {
                return lastCanExecute.Value;
            }

            this.lastCanExecute = result;
            this.OnCanExecuteChanged(EventArgs.Empty);

            return lastCanExecute.Value;
        }

        private bool? lastCanExecute;

        #region CanExecuteChanged

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            EventHandler handler = CanExecuteChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion CanExecuteChanged

        /// <summary>
        /// Définit la méthode à appeler lorsque la commande est appelée.
        /// </summary>
        /// <param name="parameter">
        /// Données utilisées par la commande. Si la commande ne requiert
        /// pas que les données soient passées, cet objet peut avoir la valeur null.
        /// </param>
        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                execute();
            }
        }

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        private readonly string name = Guid.NewGuid().ToString();

        /// <summary>
        /// Refreshes the state.
        /// </summary>
        public void RefreshState()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }
    }
}