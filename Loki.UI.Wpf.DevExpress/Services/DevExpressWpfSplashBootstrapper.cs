using System.Windows;

using Loki.IoC;

namespace Loki.UI.Wpf.DevExpress
{
    public class DevExpressWpfSplashBootstrapper<TMainModel, TSplashModel> : WpfSplashBootStrapper<TMainModel, TSplashModel>
        where TMainModel : class, IScreen
        where TSplashModel : class, ISplashViewModel
    {
        protected override void InitializeUIEngine(IObjectContext context)
        {
            context.Initialize(UIInstaller.Wpf);
        }

        public DevExpressWpfSplashBootstrapper(Window splashWindow)
            : base(splashWindow)
        {
        }
    }
}