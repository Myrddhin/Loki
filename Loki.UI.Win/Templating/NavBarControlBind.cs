using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraNavBar;
using Loki.Common;

namespace Loki.UI.Win
{
    public class NavBarControlBind : ControlBind<NavBarControl>
    {
        public NavBarControlBind(NavBarControl view, object viewModel)
            : base(view, viewModel)
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
                            item.Item.LinkClicked += (s, o) => Toolkit.Common.MessageBus.PublishOnUIThread(navMessage.Message);
                        }

                        var navCommand = navigationLink as ICommandElement;
                        if (navCommand != null)
                        {
                            item.Item.LinkClicked += (s, e) => navCommand.Command.Execute(navCommand.Parameter);

                            if (GlyphConverter != null)
                            {
                                item.Item.SmallImage = GlyphConverter.Convert(navCommand.Command, typeof(Image), true, Toolkit.UI.Windows.Culture) as Image;
                                item.Item.SmallImage = GlyphConverter.Convert(navCommand.Command, typeof(Image), false, Toolkit.UI.Windows.Culture) as Image;
                            }
                        }
                    }
                }
            }
        }
    }
}