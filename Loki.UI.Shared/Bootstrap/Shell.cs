using System.Linq;
using System.Threading.Tasks;

using Loki.UI.Models;

namespace Loki.UI.Bootstrap
{
    public class Shell
    {
        public IPlatform Platform { get; }

        private ITemplateManager templateManager;

        public Shell(IPlatform platform)
        {
            Platform = platform;
        }

        public async Task Boot()
        {
            templateManager = Platform.CompositionRoot.Resolve<ITemplateManager>();
            templateManager.AddBindings(Platform.Binder);
            templateManager.AddConventions(Platform.Conventions, Platform.UiAssemblies);

            var splash = Platform.CompositionRoot.ResolveAll<ISplashModel>().FirstOrDefault();
            var mainModels = Platform.CompositionRoot.ResolveAll<IStartModel>();

            if (splash != null)
            {
                this.templateManager.BindWithTemplate(splash);
                ViewModelExtensions.TryActivate(splash);
                await splash.InitializeApplication();
            }

            foreach (var startModel in mainModels)
            {
                var template = this.templateManager.GetBindedTemplate(startModel);
                ViewModelExtensions.TryActivate(startModel);
                Platform.SetEntryPoint(template);
            }

            if (splash != null)
            {
                ViewModelExtensions.TryClose(splash);
                Platform.CompositionRoot.Release(splash);
            }
        }
    }
}