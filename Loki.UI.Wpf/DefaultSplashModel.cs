using System.Threading.Tasks;

namespace Loki.UI.Wpf
{
    public class DefaultSplashModel : Screen, ISplashViewModel
    {
        public DefaultSplashModel(IDisplayServices coreServices)
            : base(coreServices)
        {
        }

        public async Task ApplicationInitialize()
        {
            await Task.Delay(500);
        }
    }
}