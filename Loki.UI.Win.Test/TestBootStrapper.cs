using Loki.IoC;
using Loki.UI.Test;

namespace Loki.UI.Win.Test
{
    public class TestBootStrapper : WinformBootstrapper<MainViewModel>
    {
        public TestBootStrapper()
        {
            // bootStrapper.Install(Loki.UI.Test.UIInstaller.All);

            // ControlBinder.Navigation.GlyphConverter = new CommandToGlyphConverter();
        }
    }
}