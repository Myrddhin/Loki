using Loki.IoC;
using Loki.UI.Test;

namespace Loki.UI.Wpf.Test
{
    public class TestBootStrapper : WpfBootStrapper<MainViewModel>
    {
        public TestBootStrapper()
        {
            //BootStrapper.UIInstallers = new IContextInstaller[] { UIInstaller.All };
        }
    }
}