using System.Threading.Tasks;

using Loki.Common.IoC;

namespace Loki.UI
{
    public class Shell
    {
        public IoCContainer CompositionRoot { get; private set; }

        public IPlatform Platform { get; private set; }

        public Shell(IPlatform platform)
        {
            CompositionRoot = new IoCContainer();
            Platform = platform;
        }

        public async Task Boot()
        {
            await Task.Delay(1);
        }
    }
}