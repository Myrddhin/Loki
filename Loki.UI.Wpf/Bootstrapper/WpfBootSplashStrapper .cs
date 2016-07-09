using System.Windows;

using Loki.Common;
using Loki.IoC;

namespace Loki.UI.Wpf
{
    public class WpfSplashBootStrapper<TMainModel, TSplashModel> : IPlatform
        where TMainModel : class, IScreen
        where TSplashModel : class, ISplashViewModel
    {
        private readonly IoCContext compositionRoot;

        private object mainObject;

        public WpfSplashBootStrapper(Window splashWindow)
        {
            if (Application.Current != null && !View.DesignMode)
            {
                Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

                Application.Current.MainWindow = splashWindow;
                mainObject = Application.Current.MainWindow;

                Application.Current.MainWindow.Show();
                compositionRoot = new IoCContext();
                Application.Current.Startup += Application_Startup;
            }

            BootStrapper = new CommonBootstrapper<TMainModel, TSplashModel>(this);
        }

        protected virtual void InitializeUIEngine(IObjectContext context)
        {
            context.Initialize(UIInstaller.Wpf);
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

        private ICoreServices core;

        public ICoreServices Core
        {
            get
            {
                return this.core ?? (this.core = this.Context.Get<ICoreServices>());
            }
        }

        public IObjectContext Context
        {
            get
            {
                return compositionRoot;
            }
        }

        private IUIServices ui;

        public IUIServices UI
        {
            get
            {
                return this.ui ?? (this.ui = this.Context.Get<IUIServices>());
            }
        }

        protected CommonBootstrapper<TMainModel, TSplashModel> BootStrapper { get; private set; }

        public void Run(string[] args)
        {
            ApplicationStart(args);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ApplicationStart(e.Args);
        }

        private async void ApplicationStart(string[] args)
        {
            // Initialize composition root;
            InitializeUIEngine(compositionRoot);

            UI.Threading.OnUIThread(() => { });

            // Initialize helper classes.
            View.InitializeViewHelper(UI.Templates, Core.Diagnostics.GetLog("View"), Core.Messages);
            Bind.InitializeEngine(UI.Templates);

            await BootStrapper.Run(args);

            var mainModel = Context.Get<TMainModel>();
            EntryPoint = mainModel;
            Application.Current.MainWindow.Show();
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }
    }
}