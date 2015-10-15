using System.Threading.Tasks;

using Loki.Common;

namespace Loki.UI.Win
{
    public class DefaultSplashModel : Screen, ISplashViewModel
    {
        public DefaultSplashModel(ICoreServices services, IUIServices uiServices)
            : base(services, uiServices)
        {
        }

        public async Task ApplicationInitialize()
        {
            await Task.Delay(500);
        }
    }
}