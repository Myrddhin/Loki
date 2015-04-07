using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loki.IoC;
using Loki.IoC.Registration;

namespace Loki.UI.Wpf
{
    public class UIInstaller : LokiContextInstaller
    {
        public override void Install(IObjectContext context)
        {
            context.Register(Element.For<IThreadingContext>().ImplementedBy<WpfThreadingContext>());
            context.Register(Element.For<ITemplatingEngine>().ImplementedBy<WpfTemplatingEngine>());
            context.Register(Element.For<IWindowManager>().ImplementedBy<WpfWindowManager>());
            context.Register(Element.For<DefaultSplashModel>());
        }

        private static UIInstaller winform = new UIInstaller();

        public static UIInstaller Wpf
        {
            get
            {
                return winform;
            }
        }
    }
}