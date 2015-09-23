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

            context.Register(Element.For<MenuViewModel>().Lifestyle.Transient);
            context.Register(Element.For<MainViewModel>().Lifestyle.Transient);
            context.Register(Element.For<DocumentsViewModel>().Lifestyle.Transient.Properties(Property<DocumentsViewModel>.ForKey(x => x.ActiveItem).Ignore()));
            context.Register(Element.For<NavigationMenuViewModel>().Lifestyle.Transient);
            context.Register(Element.For<IScreenFactory>().AsFactory());

            context.Register(Element.For<SplashViewModel>().Lifestyle.Transient);
            context.Register(Element.For<Screen>().Lifestyle.Transient);
        }
    }
}