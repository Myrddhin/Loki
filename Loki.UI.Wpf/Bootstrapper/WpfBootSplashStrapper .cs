using System.ComponentModel;
using System.Windows;
using Loki.Common;

namespace Loki.UI.Wpf
{
    public class WpfSplashBootStrapper<TMainModel, TSplashModel> : BaseObject, IPlatform
        where TMainModel : class, IScreen
        where TSplashModel : class, ISplashViewModel
    {
        private object mainObject;

        public WpfSplashBootStrapper(Window splashWindow)
        {
            if (Application.Current != null && !DesignMode)
            {
                Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                Toolkit.RegisterInstaller(Loki.UI.Wpf.UIInstaller.Wpf);

                Application.Current.MainWindow = splashWindow;
                mainObject = Application.Current.MainWindow;
                BootStrapper = new CommonBootstrapper<TMainModel, TSplashModel>(this);
                Application.Current.MainWindow.Show();

                Toolkit.Initialize();
                Toolkit.UI.Threading.OnUIThread(() => { });

                Application.Current.Startup += Application_Startup;
            }

            BootStrapper = new CommonBootstrapper<TMainModel, TSplashModel>(this);
        }

        private bool? inDesignMode = null;

        public bool DesignMode
        {
            get
            {
                if (inDesignMode == null)
                {
                    var descriptor = DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement));
                    inDesignMode = (bool)descriptor.Metadata.DefaultValue;
                }

                return inDesignMode.GetValueOrDefault(false);
            }
        }

        //public strin

        public object EntryPoint
        {
            get
            {
                return mainObject;
            }

            set
            {
                Application.Current.MainWindow = Toolkit.UI.Templating.GetTemplate(value) as Window;
                mainObject = value;
            }
        }

        protected CommonBootstrapper<TMainModel, TSplashModel> BootStrapper { get; private set; }

        public async void Run(string[] args)
        {
            await BootStrapper.Run(args);

            ApplicationStart();
        }

        private async void Application_Startup(object sender, System.Windows.StartupEventArgs e)
        {
            await BootStrapper.Run(e.Args);

            ApplicationStart();
        }

        private void ApplicationStart()
        {
            var mainModel = Toolkit.IoC.DefaultContext.Get<TMainModel>();
            EntryPoint = mainModel;
            Application.Current.MainWindow.Show();
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }
    }
}