using System;
using System.Reflection;
using System.Windows;

using Loki.Common.IoC;
using Loki.UI.Bootstrap;
using Loki.UI.Wpf.Binds;

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
            Binder = new WpfBinder();
            Conventions = new DefaultConventionManager();

            UiAssemblies = new[] {Assembly.GetEntryAssembly()};

            
            // Register UI Engine
            CompositionRoot.RegisterAssembly(this.GetType().GetTypeInfo().Assembly);

            Shell = new Shell(this);

            Application.Current.Startup += Application_Startup;
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await Shell.Boot();
        }

        public void Run()
        {
            var app = Application.Current ?? new Application();

            app.Run();
        }

        public void SetEntryPoint(object mainTemplate)
        {
            if (Application.Current != null && Application.Current.MainWindow == null)
            {
                Application.Current.MainWindow = mainTemplate as Window;
            }
        }

        public IoCContainer CompositionRoot { get; }

        public virtual Assembly[] UiAssemblies { get; }

        public virtual IConventionManager Conventions { get; }

        public virtual IBinder Binder { get; }

        public Shell Shell { get; }
    }
}