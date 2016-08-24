using System.Windows;

using Loki.Common.IoC;

namespace Loki.UI.Platform
{
    public class WPFPlatform : IPlatform
    {
        public WPFPlatform()
        {
            if (View.DesignMode || Application.Current == null)
            {
                return;
            }

            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;

            CompositionRoot = new IoCContainer();
            Shell = new Shell(this);

            Application.Current.Startup += Application_Startup;
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await Shell.Boot();
        }

        public IoCContainer CompositionRoot { get; }

        public Shell Shell { get; }
    }
}