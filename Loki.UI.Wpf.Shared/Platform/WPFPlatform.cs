using System.Windows;

using Loki.Common.IoC;
using Loki.IoC;

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

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Shell.Boot();
        }

        public IoCContainer CompositionRoot { get; private set; }

        public Shell Shell { get; private set; }
    }
}