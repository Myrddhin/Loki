using System.ComponentModel;
using System.Windows;
using Loki.Common;
using Loki.IoC;

namespace Loki.UI.Wpf
{
    public class WpfSplashBootStrapper<TMainModel, TSplashModel> : LoggableObject, IPlatform
        where TMainModel : class, IScreen
        where TSplashModel : class, ISplashViewModel
    {
        private object mainObject;

        public WpfSplashBootStrapper(Window splashWindow)
        {
            if (Application.Current != null && !DesignMode)
            {
                Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

                Application.Current.MainWindow = splashWindow;
                mainObject = Application.Current.MainWindow;
                BootStrapper = new CommonBootstrapper<TMainModel, TSplashModel>(this);
                Application.Current.MainWindow.Show();

                Toolkit.Initialize();
                Toolkit.IoC.RegisterInstaller(UIInstaller.Wpf);
                Context = Toolkit.IoC.DefaultContext;
                this.Core = Context.Get<ICoreServices>();
                UI = Context.Get<IUIServices>();

                UI.Threading.OnUIThread(() => { });

                Application.Current.Startup += Application_Startup;
            }

            BootStrapper = new CommonBootstrapper<TMainModel, TSplashModel>(this);
        }

        private bool? inDesignMode;

        public bool DesignMode
        {
            get
            {
                if (this.inDesignMode != null)
                {
                    return this.inDesignMode.GetValueOrDefault(false);
                }

                var descriptor = DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement));
                this.inDesignMode = (bool)descriptor.Metadata.DefaultValue;

                return inDesignMode.GetValueOrDefault(false);
            }
        }

        public object EntryPoint
        {
            get
            {
                return mainObject;
            }

            set
            {
                Application.Current.MainWindow = UI.Templates.GetTemplate(value) as Window;
                mainObject = value;
            }
        }

        public ICoreServices Core { get; private set; }

        public IObjectContext Context { get; private set; }

        public IUIServices UI { get; private set; }

        protected CommonBootstrapper<TMainModel, TSplashModel> BootStrapper { get; private set; }

        public async void Run(string[] args)
        {
            await BootStrapper.Run(args);

            ApplicationStart();
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await BootStrapper.Run(e.Args);

            ApplicationStart();
        }

        private void ApplicationStart()
        {
            var mainModel = Context.Get<TMainModel>();
            EntryPoint = mainModel;
            Application.Current.MainWindow.Show();
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }
    }
}