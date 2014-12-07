using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Loki.Common;
using Loki.UI.Win.Views;

namespace Loki.UI.Win
{
    public class WinformBootstrapper<TMainModel> : ApplicationContext, IPlatform
        where TMainModel : class, IScreen
    {
        protected CommonBootstrapper<TMainModel, DefaultSplashModel> bootStrapper;

        public WinformBootstrapper()
            : base(new SplashView())
        {
            splashScreen = MainForm;
        }

        private Form splashScreen;

        static WinformBootstrapper()
        {
            Application.SetCompatibleTextRenderingDefault(false);

            // If using EnableVisualStyles, always call DoEvents immediately afterwards. Failing to
            // do so can result in an unexpected SEHException.
            Application.EnableVisualStyles();
            Application.DoEvents();
        }

        public void Run(params string[] args)
        {
            Toolkit.RegisterInstaller(UIInstaller.Winform);

            bootStrapper = new CommonBootstrapper<TMainModel, DefaultSplashModel>(this);

            // Force synchronisation context from main thread.
            Toolkit.Initialize();
            Toolkit.UI.Threading.OnUIThread(() => { });

            Task.Factory.StartNew(() => bootStrapper.Run(args));

            Application.Run(this);
        }

        protected override void OnMainFormClosed(object sender, EventArgs e)
        {
            if (sender != splashScreen)
            {
                base.OnMainFormClosed(sender, e);
            }
            else
            {
                // Creates main objects
                var mainModel = Toolkit.IoC.DefaultContext.Get<TMainModel>();
                EntryPoint = Toolkit.UI.Templating.GetTemplate(mainModel);
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
    }
}