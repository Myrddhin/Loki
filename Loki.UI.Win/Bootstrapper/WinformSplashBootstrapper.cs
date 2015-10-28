using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Loki.Common;
using Loki.IoC;

namespace Loki.UI.Win
{
    public class WinformSplashBootstrapper<TMainModel, TSplashModel> : ApplicationContext, IPlatform
        where TMainModel : class, IScreen
        where TSplashModel : class, ISplashViewModel
    {
        private readonly CommonBootstrapper<TMainModel, TSplashModel> bootStrapper;

        public WinformSplashBootstrapper(Form splashView)
            : base(splashView)
        {
            splashScreen = MainForm;

            bootStrapper = new CommonBootstrapper<TMainModel, TSplashModel>(this);
        }

        private Form splashScreen;

        public void Run(params string[] args)
        {
            Toolkit.Initialize();
            Toolkit.IoC.RegisterInstaller(UIInstaller.Winform);
            Context = Toolkit.IoC.DefaultContext;
            this.Core = Context.Get<ICoreServices>();
            UI = Context.Get<IUIServices>();

            // Force synchronisation context from main thread.
            UI.Threading.OnUIThread(() => { });

            Task.Factory.StartNew(() => bootStrapper.Run(args));

            Application.Run(this);
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

        public ICoreServices Core { get; private set; }

        public IObjectContext Context { get; private set; }

        public IUIServices UI { get; private set; }
    }
}