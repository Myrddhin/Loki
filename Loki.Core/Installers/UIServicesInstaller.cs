using Loki.IoC;
using Loki.IoC.Registration;
using Loki.UI;
using Loki.UI.Commands;
using Loki.UI.Tasks;

namespace Loki.Common
{
    public class UIServicesInstaller : LokiContextInstaller
    {
        public override void Install(IObjectContext context)
        {
            context.Register(Element.For<IThreadingContext>().ImplementedBy<DefaultThreadingContext>());
            context.Register(Element.For<ISignalManager>().ImplementedBy<ConsoleSignalManager>());
            context.Register(Element.For<ICommandComponent>().ImplementedBy<LokiCommandService>());
            context.Register(Element.For<ITaskComponent>().ImplementedBy<TaskComponent>());

            context.Register(Element.For<IScreenFactory>().AsFactory());
        }
    }
}