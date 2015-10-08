using Loki.IoC;
using Loki.IoC.Registration;

namespace Loki.Core.Tests.IoC
{
    internal class DummyInstaller : LokiContextInstaller
    {
        public override void Install(IObjectContext context)
        {
            context.Register(Element.For<DummyClass>());
        }
    }
}