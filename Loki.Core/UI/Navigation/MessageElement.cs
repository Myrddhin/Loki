using System.ComponentModel;

using Loki.Common;

namespace Loki.UI
{
    public class MessageElement : NavigationElement, IMessageElement
    {
        public MessageElement(IDisplayServices coreServices)
            : base(coreServices)
        {
        }

        #region Message

        private static readonly PropertyChangedEventArgs ArgsMessageChanged = ObservableHelper.CreateChangedArgs<MessageElement>(x => x.Message);

        private static readonly PropertyChangingEventArgs ArgsMessageChanging = ObservableHelper.CreateChangingArgs<MessageElement>(x => x.Message);

        private INavigationMessage message;

        public INavigationMessage Message
        {
            get
            {
                return message;
            }

            set
            {
                if (value != message)
                {
                    NotifyChanging(ArgsMessageChanging);
                    message = value;
                    NotifyChanged(ArgsMessageChanged);
                }
            }
        }

        #endregion Message
    }
}