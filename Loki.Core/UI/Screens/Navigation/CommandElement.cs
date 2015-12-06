using System.ComponentModel;

using Loki.Common;
using Loki.UI.Commands;

namespace Loki.UI
{
    public class CommandElement : NavigationElement, ICommandElement
    {
        public CommandElement(IDisplayServices coreServices)
            : base(coreServices)
        {
        }

        #region Command

        private static readonly PropertyChangedEventArgs argsCommandChanged =
            ObservableHelper.CreateChangedArgs<CommandElement>(x => x.Command);

        private static readonly PropertyChangingEventArgs argsCommandChanging =
            ObservableHelper.CreateChangingArgs<CommandElement>(x => x.Command);

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

        private static readonly PropertyChangedEventArgs argsParameterChanged =
            ObservableHelper.CreateChangedArgs<CommandElement>(x => x.Parameter);

        private static readonly PropertyChangingEventArgs argsParameterChanging =
            ObservableHelper.CreateChangingArgs<CommandElement>(x => x.Parameter);

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