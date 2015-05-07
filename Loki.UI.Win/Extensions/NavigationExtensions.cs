using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraBars;
using DevExpress.XtraNavBar;
using Loki.Common;

namespace Loki.UI.Win
{
    public static class NavigationExtensions
    {
        public static void Bind<TModel>(this NavBarControl navBar, Expression<Func<TModel, object>> propertyGetter) where TModel : class
        {
            var binder = new Binder();

            var containerModel = binder.GetContainer<TModel>(navBar, propertyGetter);
            if (containerModel == null)
            {
                return;
            }

            Toolkit.UI.Templating.CreateBind(navBar, containerModel);
        }

        /*
                public static void Bind<TModel>(BarItem item, Expression<Func<TModel, ICommandElement>> commandGetter) where TModel : class
                {
                    var form = item.Manager.Form;
                    if (form == null)
                    {
                        Log.Warn("Bar manager is not associated with a form");
                        return;
                    }

                    var containerModel = form.GetViewModel<TModel>();
                    if (containerModel == null)
                    {
                        return;
                    }

                    ICommandElement command = commandGetter.Compile()(containerModel);

                    item.Caption = GetName(command);
                    item.ItemClick += (s, e) => command.Command.Execute(command.Parameter);

                    ControlBinder.Command.BindCommandActivation(item, ExpressionHelper.GetProperty<BarBaseButtonItem, bool>(x => x.Enabled), containerModel, command.Command, null);

                    if (GlyphConverter != null)
                    {
                        item.Glyph = GlyphConverter.Convert(command.Command, typeof(Image), true, UIContext.Windows.Culture) as Image;
                        item.GlyphDisabled = GlyphConverter.Convert(command.Command, typeof(Image), false, UIContext.Windows.Culture) as Image;
                    }
                }

                public static void Bind<TModel>(BarManager manager, Bar bar, Expression<Func<TModel, object>> propertyGetter) where TModel : class
                {
                    var form = manager.Form;
                    if (form == null)
                    {
                        Log.Warn("Bar manager is not associated with a form");
                        return;
                    }

                    var containerModel = GetContainer<TModel>(form, propertyGetter);
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
                                var baseForm = form.FindForm();
                                if (baseForm != null && baseForm.Validate())
                                {
                                    menuName.Command.Execute(menuName.Parameter);
                                }
                                else
                                {
                                    menuName.Command.Execute(menuName.Parameter);
                                }
                            };

                            ControlBinder.Command.BindCommandActivation(button, ExpressionHelper.GetProperty<BarBaseButtonItem, bool>(x => x.Enabled), containerModel, menuName.Command, null);

                            if (GlyphConverter != null)
                            {
                                button.Glyph = GlyphConverter.Convert(menuName.Command, typeof(Image), true, UIContext.Windows.Culture) as Image;
                                button.GlyphDisabled = GlyphConverter.Convert(menuName.Command, typeof(Image), false, UIContext.Windows.Culture) as Image;
                            }

                            manager.Items.Add(button);
                            bar.ItemLinks.Add(button);
                        }
                    }
                }*/
    }
}