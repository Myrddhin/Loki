using Loki.IoC;
using Loki.IoC.Registration;

namespace Loki.UI.Win
{
    public class UIInstaller : LokiContextInstaller
    {
        public override void Install(IObjectContext context)
        {
            context.Register(Element.For<IThreadingContext>().ImplementedBy<WindowsFormsThreadingContext>());
            context.Register(Element.For<ITemplatingEngine>().ImplementedBy<WinformTemplatingEngine>());
            context.Register(Element.For<IWindowManager>().ImplementedBy<WinformWindowManager>());
            context.Register(Element.For<DefaultSplashModel>());
        }

        private static UIInstaller winform = new UIInstaller();

        public static UIInstaller Winform
        {
            get
            {
                return winform;
            }
        }
    }
}