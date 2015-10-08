using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Loki.Common;
using Loki.IoC;
using Loki.IoC.Registration;

namespace Loki.UI
{
    public class CommonBootstrapper<TMainModel, TSplashModel> : BaseObject, IHandle<WarningMessage>, IHandle<ErrorMessage>, IHandle<InformationMessage>
        where TMainModel : class, IScreen
        where TSplashModel : class, ISplashViewModel
    {
        private object splashView;

        private TSplashModel splashModel;

        private IPlatform entryPoint;

        public CommonBootstrapper(IPlatform platform)
        {
            if (platform.EntryPoint != null)
            {
                this.splashView = platform.EntryPoint;
            }

            entryPoint = platform;

            ConventionManager = new DefaultConventionManager();
        }

        private List<IContextInstaller> registeredInstallers = new List<IContextInstaller>();

        public IEnumerable<IContextInstaller> Installers
        {
            get { return registeredInstallers; }
        }

        public void Install(params IContextInstaller[] installers)
        {
            registeredInstallers.AddRange(installers);
        }

        protected IConventionManager ConventionManager { get; set; }

        protected IEnumerable<Assembly> SelectedAssemblies
        {
            get
            {
                return new Assembly[] { Assembly.GetEntryAssembly(), Assembly.GetExecutingAssembly() }.Distinct();
            }
        }

        public async Task Run(string[] startParameters)
        {
            Task initializeTask = null;
            try
            {
                initializeTask = Task.WhenAll(Task.Delay(5000), Initialize(startParameters));
                await initializeTask;
            }
            catch (AggregateException taskException)
            {
                foreach (var exception in taskException.InnerExceptions)
                {
                    Toolkit.UI.Signals.Error(exception, true);
                }
            }

            if (initializeTask == null || initializeTask.IsFaulted)
            {
                Exit(-1);
            }
            else
            {
                Start(startParameters);
            }
        }

        private async Task Initialize(string[] startParameters)
        {
            await InitializeFramework();
            await PreStart(startParameters);
        }

        private async Task InitializeFramework()
        {
            OnInitializing(EventArgs.Empty);

            await Task.Run(() =>
            {
                // Initialize ui engine
                if (!Installers.Any())
                {
                    // no specific configuration : configure default TMainViewModel
                    Toolkit.IoC.DefaultContext.Register(Element.For<TMainModel>());
                }
                else
                {
                    Toolkit.IoC.DefaultContext.Initialize(Installers.ToArray());
                }

                Toolkit.Common.MessageBus.Subscribe(this);
                Toolkit.Common.MessageBus.Subscribe(Toolkit.UI.Signals);

                Toolkit.UI.Templating.LoadByConvention(ConventionManager, SelectedAssemblies.ToArray());
            });

            OnInitialized(EventArgs.Empty);
        }

        public virtual void Exit(int returnCode)
        {
            Toolkit.Common.MessageBus.Unsubscribe(this);
            Toolkit.UI.Signals.ApplicationExit(returnCode);
        }

        protected async virtual Task PreStart(string[] startParameters)
        {
            // Creates main objects
            if (splashView != null)
            {
                splashModel = Toolkit.IoC.DefaultContext.Get<TSplashModel>();

                Toolkit.UI.Templating.CreateBind(splashView, splashModel);

                await splashModel.ApplicationInitialize();
            }
        }

        #region Initializing

        public event EventHandler Initializing;

        protected virtual void OnInitializing(EventArgs e)
        {
            EventHandler handler = Initializing;

            if (handler != null)
            {
                Toolkit.UI.Threading.OnUIThread(() => handler(this, e));
            }
        }

        #endregion Initializing

        #region Initialized

        public event EventHandler Initialized;

        protected virtual void OnInitialized(EventArgs e)
        {
            EventHandler handler = Initialized;

            if (handler != null)
            {
                Toolkit.UI.Threading.OnUIThread(() => handler(this, e));
            }
        }

        #endregion Initialized

        public virtual void Start(string[] startParameters)
        {
            Toolkit.Common.MessageBus.PublishOnUIThread(new StartMessage(startParameters));

            splashModel.TryClose();
        }

        public void Handle(WarningMessage message)
        {
            Toolkit.UI.Signals.Warning(message.Message);
        }

        public void Handle(ErrorMessage message)
        {
            Toolkit.UI.Signals.Error(message.Error, false);
        }

        public void Handle(InformationMessage message)
        {
            Toolkit.UI.Signals.Message(message.Message);
        }
    }
}