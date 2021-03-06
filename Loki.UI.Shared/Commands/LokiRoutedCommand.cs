﻿using System;
using System.Collections.Generic;

using Loki.Common;
using Loki.Common.Diagnostics;
using Loki.Common.Messages;

namespace Loki.UI.Commands
{
    /// <summary>
    /// Loki default command type.
    /// </summary>
    public class LokiRoutedCommand : BaseObject, ILokiCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LokiRoutedCommand"/> class.
        /// </summary>
        /// <param name="name">
        /// Command Name.
        /// </param>
        /// <param name="diagnostics">
        /// Diagnostics component.
        /// </param>
        /// <param name="commands">
        /// Command service component.
        /// </param>
        /// <param name="messageBus">
        /// </param>
        public LokiRoutedCommand(
            string name, 
            IDiagnostics diagnostics, 
            ICommandManager commands, 
            IMessageBus messageBus) 
            : base(diagnostics)
        {
            commandService = commands;
            this.messageBus = messageBus;
            Name = name;
            lastCanExecute = null;
        }

        /// <summary>
        /// Gets or sets the command service.
        /// </summary>
        /// <value>
        /// The command service.
        /// </value>
        private readonly ICommandManager commandService;

        /// <summary>
        /// Gets or sets the message bus.
        /// </summary>
        /// <value>
        /// The message bus.
        /// </value>
        private readonly IMessageBus messageBus;

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Name { get; }

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
            // gets command handlers
            IEnumerable<ICommandBind> handlers = commandService.GetHandlers(this);

            // check handlers presence
            if (handlers == null)
            {
                UpdateCanExecute(false);
                return false;
            }

            // define parameter
            CanExecuteCommandEventArgs args = new CanExecuteCommandEventArgs(this, parameter);

            foreach (ICommandBind handler in handlers)
            {
                // check execution
                handler.CanExecute?.Invoke(this, args);

                // break if found
                if (args.CanExecute)
                {
                    break;
                }
            }

            UpdateCanExecute(args.CanExecute);

            Log.DebugFormat("Command {0} : can execute {1}", this.Name, args.CanExecute);
            return args.CanExecute;
        }

        protected void UpdateCanExecute(bool value)
        {
            // Command change
            if (!lastCanExecute.HasValue || lastCanExecute.Value != value)
            {
                lastCanExecute = value;
                OnCanExecuteChanged(EventArgs.Empty);
            }
        }

        private bool? lastCanExecute;

        /// <summary>
        /// Refreshes the state.
        /// </summary>
        public void RefreshState()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }

        #region CanExecuteChanged

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
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
            // gets command handlers
            IEnumerable<ICommandBind> handlers = commandService.GetHandlers(this);

            // check handlers presence
            if (handlers == null)
            {
                return;
            }

            // define parameter
            CanExecuteCommandEventArgs args = new CanExecuteCommandEventArgs(this, parameter);

            foreach (ICommandBind handler in handlers)
            {
                args.CanExecute = false;

                // check execution
                handler.CanExecute?.Invoke(this, args);

                if (args.CanExecute && handler.ConfirmDelegate != null)
                {
                    args.CanExecute &= handler.ConfirmDelegate(args);
                }

                // run if found
                if (!args.CanExecute)
                {
                    continue;
                }

                try
                {
                    handler.Execute?.Invoke(this, args);
                }
                catch (LokiException exception)
                {
                    messageBus.PublishOnCurrentThread(new ErrorMessage(exception));
                }
            }
        }
    }
}