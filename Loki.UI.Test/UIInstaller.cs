using System;
using Loki.IoC;
using Loki.IoC.Registration;

namespace Loki.UI.Test
{
    public class UIInstaller : LokiContextInstaller
    {
        private static Lazy<UIInstaller> all = new Lazy<UIInstaller>();

        public static UIInstaller All
        {
            get
            {
                return all.Value;
            }
        }

        public override void Install(IObjectContext context)
        {
            base.Install(context);

            context.Register(Element.ViewModel<MenuViewModel>());
            context.Register(Element.ViewModel<MainViewModel>());
            context.Register(Element.ViewModel<DocumentsViewModel>().Properties(Property<DocumentsViewModel>.ForKey(x => x.ActiveItem).Ignore()));
            context.Register(Element.ViewModel<NavigationMenuViewModel>());
            context.Register(Element.ViewModel<Screen>());
        }
    }
}