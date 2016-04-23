using System.Drawing;
using System.Threading;

using DevExpress.XtraBars;

using Loki.Common;

namespace Loki.UI.Win
{
    public class BarBind : ComponentBind<Bar>
    {
        public BarBind(ICoreServices services, IThreadingContext ctx, Bar view, object viewModel)
            : base(services, ctx, view, viewModel)
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
                        button.Glyph = GlyphConverter.Convert(menuName.Command, typeof(Image), true, Thread.CurrentThread.CurrentUICulture) as Image;
                        button.GlyphDisabled = GlyphConverter.Convert(menuName.Command, typeof(Image), false, Thread.CurrentThread.CurrentUICulture) as Image;
                    }

                    view.Manager.Items.Add(button);
                    view.ItemLinks.Add(button);
                }
            }
        }
    }
}