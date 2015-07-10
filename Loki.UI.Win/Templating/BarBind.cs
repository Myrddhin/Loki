using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraBars;
using Loki.Common;

namespace Loki.UI.Win
{
    public class BarBind : ComponentBind<Bar>
    {
        public BarBind(Bar view, object viewModel)
            : base(view, viewModel)
        {
            var containerModel = viewModel as IParent;
            if (containerModel == null)
            {
                return;
            }

            foreach (var menuItem in containerModel.Children)
            {
                BarButtonItem button = new BarButtonItem();
                var menuName = menuItem as ICommandElement;
                if (menuName != null)
                {
                    button.Caption = GetName(menuName);
                    button.ItemClick += (s, e) =>
                    {
                        var baseForm = view.Manager.Form.FindForm();
                        if (baseForm != null && baseForm.Validate())
                        {
                            menuName.Command.Execute(menuName.Parameter);
                        }
                        else
                        {
                            menuName.Command.Execute(menuName.Parameter);
                        }
                    };

                    BindCommandActivation(button, ExpressionHelper.GetProperty<BarBaseButtonItem, bool>(x => x.Enabled), containerModel, menuName.Command, null);

                    if (GlyphConverter != null)
                    {
                        button.Glyph = GlyphConverter.Convert(menuName.Command, typeof(Image), true, Toolkit.UI.Windows.Culture) as Image;
                        button.GlyphDisabled = GlyphConverter.Convert(menuName.Command, typeof(Image), false, Toolkit.UI.Windows.Culture) as Image;
                    }

                    view.Manager.Items.Add(button);
                    view.ItemLinks.Add(button);
                }
            }
        }
    }
}