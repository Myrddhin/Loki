using System.ComponentModel;
using System.Windows.Input;
using Loki.Common;

namespace Loki.UI
{
    public class CommandElement : NavigationElement, ICommandElement
    {
        #region Command

        private static PropertyChangedEventArgs argsCommandChanged = ObservableHelper.CreateChangedArgs<CommandElement>(x => x.Command);

        private static PropertyChangingEventArgs argsCommandChanging = ObservableHelper.CreateChangingArgs<CommandElement>(x => x.Command);

        private ICommand command;

        public ICommand Command
        {
            get
            {
                return command;
            }

            set
            {
                if (value != command)
                {
                    NotifyChanging(argsCommandChanging);
                    command = value;
                    NotifyChanged(argsCommandChanged);
                }
            }
        }

        #endregion Command
    }
}