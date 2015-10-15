using System.ComponentModel;
using System.Windows;
using Loki.Common;
using Loki.IoC;

namespace Loki.UI.Wpf
{
    public class WpfBootStrapper<TMainViewModelType> : BaseObject, IPlatform
        where TMainViewModelType : class, IScreen
    {
        private object mainObject;

        public WpfBootStrapper()
        {
            if (Application.Current != null && !DesignMode)
            {
                Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

                Application.Current.MainWindow = new SplashView();
                mainObject = Application.Current.MainWindow;
                Application.Current.MainWindow.Show();

                BootStrapper = new CommonBootstrapper<TMainViewModelType, DefaultSplashModel>(this);

                Toolkit.Initialize();
                Toolkit.IoC.RegisterInstaller(UIInstaller.Wpf);
                Context = Toolkit.IoC.DefaultContext;
                Services = Context.Get<ICoreServices>();
                UI = Context.Get<IUIServices>();

                Application.Current.Startup += Application_Startup;
            }
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

        public ICoreServices Services { get; private set; }

        public IObjectContext Context { get; private set; }

        public IUIServices UI { get; private set; }

        protected CommonBootstrapper<TMainViewModelType, DefaultSplashModel> BootStrapper { get; private set; }

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
            var mainModel = Context.Get<TMainViewModelType>();
            EntryPoint = mainModel;
            Application.Current.MainWindow.Show();
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }
    }
}