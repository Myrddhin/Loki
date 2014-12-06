using Loki.IoC;
using Loki.IoC.Registration;
using Loki.UI;

namespace Loki.Common
{
    public class UIServicesInstaller : LokiContextInstaller
    {
        public override void Install(IObjectContext context)
        {
            context.Register(Element.For<IThreadingContext>().ImplementedBy<DefaultThreadingContext>());
            context.Register(Element.For<ISignalManager>().ImplementedBy<ConsoleSignalManager>());
            /*context.Register(Element.Service<ICommandComponent, LokiCommandService>());
           */
            /*context.Register(Element.For<CommandManager>().Lifestyle.Transient);*/
        }
    }
}