using System.Windows.Data;

using DevExpress.Xpf.NavBar;

using Loki.Common;
using Loki.UI.Wpf.Binds;

namespace Loki.UI.Wpf.Templating.DevExpress
{
    internal class NavBarItemBind : DependencyObjectBind<NavBarItem>
    {
        public NavBarItemBind(ICoreServices services, IThreadingContext threading, NavBarItem view, object viewModel)
            : base(services, threading, view, viewModel)
        {
            IHaveDisplayName withDisplay = viewModel as IHaveDisplayName;
            if (withDisplay != null)
            {
                Binding binding = new Binding(ExpressionHelper.GetProperty<IHaveDisplayName, string>(x => x.DisplayName).Name);
                binding.Source = viewModel;
                binding.Mode = BindingMode.OneWay;
                binding.NotifyOnSourceUpdated = true;
                binding.NotifyOnTargetUpdated = false;
                view.SetBinding(NavBarItem.ContentProperty, binding);
            }

            var navMessage = viewModel as IMessageElement;
            if (navMessage != null)
            {
                view.Click += (s, o) => services.Messages.PublishOnUIThread(threading, navMessage.Message);
            }

            var navCommand = viewModel as ICommandElement;
            if (navCommand != null)
            {
                view.Click += (s, e) => navCommand.Command.Execute(e);
            }
        }
    }
}