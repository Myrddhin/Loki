using System.Windows.Forms;
using Loki.UI.Win.Views;

namespace Loki.UI.Win
{
    public class WinformBootstrapper<TMainModel> : WinformSplashBootstrapper<TMainModel, DefaultSplashModel>
        where TMainModel : class, IScreen
    {
        public WinformBootstrapper()
            : base(new SplashView())
        {
        }
    }
}