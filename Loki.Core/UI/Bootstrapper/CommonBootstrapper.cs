using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Loki.Common;
using Loki.IoC;
using Loki.IoC.Registration;

namespace Loki.UI
{
    public class CommonBootstrapper<TMainModel, TSplashModel> : BaseObject
        where TMainModel : class, IScreen
        where TSplashModel : class, IScreen
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

        async public void Run(string[] startParameters)
        {
            var timerTask = new Task(() => Thread.Sleep(5000));

            var initializeTask = new Task(() =>
            {
                Initialize();
            });

            try
            {
                timerTask.Start();
                initializeTask.Start();

                await Task.WhenAll(timerTask, initializeTask);
            }
            catch (AggregateException taskException)
            {
                foreach (var exception in taskException.InnerExceptions)
                {
                    Toolkit.UI.Signals.Error(exception, true);
                }
            }

            if (initializeTask.IsFaulted)
            {
                Exit(-1);
            }
            else
            {
                PreStart(startParameters);
                Start(startParameters);
            }
        }

        private void Initialize()
        {
            OnInitializing(EventArgs.Empty);

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

            Toolkit.UI.Templating.LoadByConvention(ConventionManager, SelectedAssemblies.ToArray());

            OnInitialized(EventArgs.Empty);
        }

        public virtual void Exit(int returnCode)
        {
            Toolkit.Common.MessageBus.Unsubscribe(this);
            Toolkit.UI.Signals.ApplicationExit(returnCode);
        }

        protected virtual void PreStart(string[] startParameters)
        {
            // Creates main objects
            if (splashView != null)
            {
                splashModel = Toolkit.IoC.DefaultContext.Get<TSplashModel>();

                Toolkit.UI.Templating.CreateBind(splashView, splashModel);
            }

            /*if (Application.SplashModel == null)
            {*/

            /*}
            else
            {
                var splashScreen = ToolKit.Templates.GetView(Application.SplashModel as IViewModel);
                splashScreen.Show();
                bool buffer = Application.SplashModel.StartLoading();
                if (buffer)
                {
                    ApplicationCommands.START.Execute(null);
                }
                else
                {
                    ApplicationCommands.EXIT.Execute(null);
                }
            }*/
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
    }
}