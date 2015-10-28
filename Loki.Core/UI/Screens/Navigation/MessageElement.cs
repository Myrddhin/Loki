using Loki.Common;
using System.ComponentModel;

namespace Loki.UI
{
    public class MessageElement : NavigationElement, IMessageElement
    {
        public MessageElement(IDisplayServices coreServices)
            : base(coreServices)
        {
        }

        #region Message

        private static PropertyChangedEventArgs argsMessageChanged = ObservableHelper.CreateChangedArgs<MessageElement>(x => x.Message);

        private static PropertyChangingEventArgs argsMessageChanging = ObservableHelper.CreateChangingArgs<MessageElement>(x => x.Message);

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
                    NotifyChanging(argsMessageChanging);
                    message = value;
                    NotifyChanged(argsMessageChanged);
                }
            }
        }

        #endregion Message
    }
}