using System.ComponentModel;
using Loki.Commands;
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

        #region Parameter

        private static PropertyChangedEventArgs argsParameterChanged = ObservableHelper.CreateChangedArgs<CommandElement>(x => x.Parameter);

        private static PropertyChangingEventArgs argsParameterChanging = ObservableHelper.CreateChangingArgs<CommandElement>(x => x.Parameter);

        private object parameter;

        public object Parameter
        {
            get
            {
                return parameter;
            }

            set
            {
                if (value != parameter)
                {
                    NotifyChanging(argsParameterChanging);
                    parameter = value;
                    NotifyChanged(argsParameterChanged);
                }
            }
        }

        #endregion Parameter
    }
}