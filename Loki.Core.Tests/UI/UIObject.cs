using System.ComponentModel;

using Loki.Commands;

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