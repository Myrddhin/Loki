using System;
using System.Reflection;
using System.Windows.Forms;

using Loki.Common;
using Loki.Common.IoC;
using Loki.IoC;

namespace Loki.UI.Win
{
    public class WinformSplashBootstrapper<TMainModel, TSplashModel> : ApplicationContext, IPlatform
        where TMainModel : class, IScreen
        where TSplashModel : class, ISplashViewModel
    {
       // private readonly CommonBootstrapper<TMainModel, TSplashModel> bootStrapper;

        public WinformSplashBootstrapper(Form splashView)
            : base(splashView)
        {
            splashScreen = MainForm;
            compositionRoot = new IoCContext();

           // bootStrapper = new CommonBootstrapper<TMainModel, TSplashModel>(this);
        }

        private readonly Form splashScreen;

        private readonly IoCContext compositionRoot;

        public async void Run(params string[] args)
        {
            // Initialize composition root;
            InitializeUIEngine(compositionRoot);

            // Force synchronisation context from main thread.
            UI.Threading.OnUIThread(() => { });

            // Initialize helper classes.
            //Bind.InitializeEngine(Core, UI.Threading, UI.Templates);

           // await bootStrapper.Run(args);

            Application.Run(this);
        }

        protected virtual void InitializeUIEngine(IObjectContext context)
        {
            context.Initialize(UIInstaller.Winform);
        }

        protected override void OnMainFormClosed(object sender, EventArgs e)
        {
            if (sender != splashScreen)
            {
                Context.Release(this.Core);
                base.OnMainFormClosed(sender, e);
            }
            else
            {
                // Creates main objects
                var mainModel = Context.Get<TMainModel>();
                EntryPoint = UI.Templates.GetTemplate(mainModel);
                MainForm.Show();
            }
        }

        public object EntryPoint
        {
            get
            {
                return MainForm;
            }

            set
            {
                MainForm = value as Form;
            }
        }

        private IInfrastructure core;

        public IInfrastructure Core
        {
            get
            {
                return this.core ?? (this.core = this.Context.Get<IInfrastructure>());
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

        public IoCContainer CompositionRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Assembly[] UiAssemblies
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IConventionManager Conventions
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IBinder Binder
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}