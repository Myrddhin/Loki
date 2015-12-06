using System;
using System.ComponentModel;

using Loki.UI.Commands;

namespace Loki.Core.Tests.UI
{
    public class UIObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged()
        {
            var propertyChanger = new PropertyChangedEventArgs(string.Empty);
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, propertyChanger);
            }
        }

        public bool CanExecuteReturn { get; set; }

        public int CanExecuteCount { get; private set; }

        public bool DirectCanExecute()
        {
            CanExecuteCount++;
            return CanExecuteReturn;
        }

        public void CanExecute(object sender, CanExecuteCommandEventArgs e)
        {
            CanExecuteCount++;
            e.CanExecute = CanExecuteReturn;
        }

        public int ExecuteCount { get; private set; }

        public void Execute(object sender, CommandEventArgs e)
        {
            ExecuteCount++;
        }

        public void DirectExecute()
        {
            ExecuteCount++;
        }

        public int EventCount { get; private set; }

        public void CanExecuteChanged(object sender, EventArgs e)
        {
            EventCount++;
        }
    }
}