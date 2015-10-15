using System.ComponentModel;
using Loki.Common;

namespace Loki.UI
{
    public class MessageElement : NavigationElement, IMessageElement
    {
        public MessageElement(ICoreServices services, IUIServices uiServices)
            : base(services, uiServices)
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