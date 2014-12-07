using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Loki.Common;

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
                Toolkit.RegisterInstaller(Loki.UI.Wpf.UIInstaller.Wpf);

                Application.Current.MainWindow = new SplashView();
                mainObject = Application.Current.MainWindow;
                Application.Current.MainWindow.Show();

                BootStrapper = new CommonBootstrapper<TMainViewModelType, DefaultSplashModel>(this);

                Toolkit.Initialize();
                Toolkit.UI.Threading.OnUIThread(() => { });

                Application.Current.Startup += Application_Startup;
            }
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

        protected CommonBootstrapper<TMainViewModelType, DefaultSplashModel> BootStrapper { get; private set; }

        public void Run(string[] args)
        {
            Task.Factory.StartNew(() => { BootStrapper.Run(args); })
                .ContinueWith(this.ApplicationStart, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Application_Startup(object sender, System.Windows.StartupEventArgs e)
        {
            Task.Factory.StartNew(() => { BootStrapper.Run(e.Args); })
                .ContinueWith(this.ApplicationStart, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ApplicationStart(Task previous)
        {
            var mainModel = Toolkit.IoC.DefaultContext.Get<TMainViewModelType>();
            EntryPoint = mainModel;
            Application.Current.MainWindow.Show();
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }
    }
}