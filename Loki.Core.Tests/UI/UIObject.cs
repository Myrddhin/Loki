using System;
using System.ComponentModel;

using Loki.Commands;
using Loki.UI;

namespace Loki.Core.Tests.UI
{
    public class UIObject : INotifyPropertyChanged, IInitializable, IActivable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            IsInitialized = true;
        }

        public void NotifyPropertyChanged()
        {
            var propertyChanger = new PropertyChangedEventArgs(string.Empty);
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, propertyChanger);
            }
        }

        public event EventHandler<ActivationEventArgs> Activated;

        public bool IsActive { get; private set; }

        public void Activate()
        {
            IsActive = true;
        }

        public int CanExecuteCount { get; private set; }

        public void CanExecute(object sender, CanExecuteCommandEventArgs e)
        {
            CanExecuteCount++;
        }

        public int ExecuteCount { get; private set; }

        public void Execute(object sender, CommandEventArgs e)
        {
            ExecuteCount++;
        }
    }
}