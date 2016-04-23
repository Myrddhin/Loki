using System.Drawing;
using System.Globalization;

using DevExpress.XtraNavBar;

using Loki.Common;

namespace Loki.UI.Win
{
    public class NavBarControlBind : ControlBind<NavBarControl>
    {
        public NavBarControlBind(ICoreServices services, IThreadingContext ctx, NavBarControl view, object viewModel)
            : base(services, ctx, view, viewModel)
        {
            var containerModel = ViewModel as IParent;
            if (containerModel == null)
            {
                return;
            }

            foreach (var navigationItem in containerModel.Children)
            {
                NavBarGroup group = new NavBarGroup();
                var navParent = navigationItem as IParent;
                if (navParent != null)
                {
                    group.Caption = GetName(navParent);

                    view.Groups.Add(group);
                    group.Expanded = true;

                    foreach (var navigationLink in navParent.Children)
                    {
                        var navLink = navigationLink as INavigationElement;
                        if (navLink == null)
                        {
                            continue;
                        }

                        var item = group.AddItem();
                        item.Item.Caption = GetName(navLink);

                        var navMessage = navigationLink as IMessageElement;
                        if (navMessage != null)
                        {
                            item.Item.LinkClicked += (s, o) => services.Messages.PublishOnUIThread(ctx, navMessage.Message);
                        }

                        var navCommand = navigationLink as ICommandElement;
                        if (navCommand != null)
                        {
                            item.Item.LinkClicked += (s, e) => navCommand.Command.Execute(navCommand.Parameter);

                            if (GlyphConverter != null)
                            {
                                item.Item.SmallImage = GlyphConverter.Convert(navCommand.Command, typeof(Image), true, CultureInfo.CurrentUICulture) as Image;
                                item.Item.SmallImage = GlyphConverter.Convert(navCommand.Command, typeof(Image), false, CultureInfo.CurrentUICulture) as Image;
                            }
                        }
                    }
                }
            }
        }
    }
}