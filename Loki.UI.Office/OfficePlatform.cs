using System;

using Loki.Common;
using Loki.IoC;

namespace Loki.UI.Office
{
    /// <summary>
    /// Office platoform base.
    /// </summary>
    /// <typeparam name="TMainModel">
    /// </typeparam>
    public abstract class OfficePlatform<TMainModel> : IPlatform
        where TMainModel : OfficeShell
    {
        public object EntryPoint { get; private set; }

        private readonly IObjectContext compositionRoot;

        protected CommonBootstrapper<TMainModel, DefaultSplashModel> BootStrapper { get; private set; }

        private TMainModel mainController;

        private OfficePlatform(object rootInstance)
        {
            if (!View.DesignMode)
            {
                EntryPoint = rootInstance;
                compositionRoot = new IoCContext();
                compositionRoot.Initialize(UIInstaller.Wpf);

                BootStrapper = new CommonBootstrapper<TMainModel, DefaultSplashModel>(this);
                BootStrapper.AddTemplates(this.GetType().Assembly);
                BootStrapper.AddTemplates(EntryPoint.GetType().Assembly);

                // create common application bootstrapper.
                BootStrapper.Initialized += BootStrapper_Initialized;

                // Initialize helper classes.
                View.InitializeViewHelper(UI.Templates, Core.Logger.GetLog("View"));
                Bind.InitializeEngine(UI.Templates);
            }
        }

        protected OfficePlatform(object rootInstance, LokiRibbon ribbon)
            : this(rootInstance)
        {
            RibbonBinder = ribbon.SetModel;
        }

        protected Action<TMainModel> MainObjectBinder { get; set; }

        protected Action<TMainModel> RibbonBinder { get; set; }

        private void BootStrapper_Initialized(object sender, EventArgs e)
        {
            // Create bind between main object and ribbon.
            // could be created by calling templates.CreateBind and subscribe to the BindingRequired event.
            mainController = Context.Get<TMainModel>();

            // Create bind between ribbon and main object
            if (RibbonBinder != null)
            {
                RibbonBinder(mainController);
            }

            // Create bind between addin and main object.
            if (MainObjectBinder != null)
            {
                MainObjectBinder(mainController);
            }

            // Gets the monitoring service and initialize it with the addin
            var monitoring = Context.Get<IMonitoringService>();
            monitoring.Initialize(EntryPoint.GetType());

            // Listen the message bus.
            Core.Messages.Subscribe(this);

            // Try activating main object (if it is activable).
            ViewModelExtenstions.TryActivate(mainController);
        }

        public async void Start(params string[] parameters)
        {
            await BootStrapper.Run(parameters);
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
    }
}