using Loki.IoC;
using Loki.IoC.Registration;

namespace Loki.UI.Office
{
    public class UIInstaller : LokiContextInstaller
    {
        public override void Install(IObjectContext context)
        {
            context.Register(Element.For<IThreadingContext>().ImplementedBy<WpfThreadingContext>());
            context.Register(Element.For<ITemplatingEngine>().ImplementedBy<WpfTemplatingEngine>());
            context.Register(Element.For<ISignalManager>().ImplementedBy<WpfSignalManager>());
            context.Register(Element.For<IWindowManager>().ImplementedBy<WpfWindowManager>());
            context.Register(Element.For<IMonitoringService>().ImplementedBy<MonitoringService>());
            context.Register(Element.For<DefaultSplashModel>());
        }

        private static readonly UIInstaller winform = new UIInstaller();

        public static UIInstaller Wpf
        {
            get
            {
                return winform;
            }
        }
    }
}