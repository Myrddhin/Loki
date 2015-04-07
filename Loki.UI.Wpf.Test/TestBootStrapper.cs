using Loki.IoC;
using Loki.UI.Test;

namespace Loki.UI.Wpf.Test
{
    public class TestBootStrapper : WpfSplashBootStrapper<MainViewModel, SplashViewModel>
    {
        public TestBootStrapper()
            : base(new SplashScreenView())
        {
            BootStrapper.Install(Loki.UI.Test.UIInstaller.All);
        }
    }
}