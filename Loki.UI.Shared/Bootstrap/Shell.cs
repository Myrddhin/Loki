using System.Threading.Tasks;

using Loki.UI.Models;

namespace Loki.UI
{
    public class Shell
    {
        public IPlatform Platform { get; private set; }

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

            await Task.Delay(1);

            var mainModels = Platform.CompositionRoot.ResolveAll<IStartModel>();

            foreach (var startModel in mainModels)
            {
                var template = this.templateManager.GetBindedTemplate(startModel);
                ViewModelExtensions.TryActivate(startModel);
            }
        }
    }
}