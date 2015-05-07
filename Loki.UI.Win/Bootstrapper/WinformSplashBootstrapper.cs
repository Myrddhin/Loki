﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Loki.Common;
using Loki.UI.Win.Views;

namespace Loki.UI.Win
{
    public class WinformSplashBootstrapper<TMainModel, TSplashModel> : ApplicationContext, IPlatform
        where TMainModel : class, IScreen
        where TSplashModel : class, ISplashViewModel
    {
        protected CommonBootstrapper<TMainModel, TSplashModel> bootStrapper;

        public WinformSplashBootstrapper(Form splashView)
            : base(splashView)
        {
            splashScreen = MainForm;

            bootStrapper = new CommonBootstrapper<TMainModel, TSplashModel>(this);
        }

        private Form splashScreen;

        public void Run(params string[] args)
        {
            Toolkit.RegisterInstaller(UIInstaller.Winform);

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