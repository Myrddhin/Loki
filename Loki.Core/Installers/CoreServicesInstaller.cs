using Loki.IoC;
using Loki.IoC.Registration;

namespace Loki.Common
{
    public class CoreServicesInstaller : LokiContextInstaller
    {
        public override void Install(IObjectContext context)
        {
            context.Register(Element.Service<IErrorComponent, ErrorService>());
            context.Register(Element.Service<IEventComponent, LokiEventService>());
            /* context.Register(Element.Service<ITaskComponent, LokiTaskService>());
             context.Register(Element.Service<ISettingsComponent, LokiSettingsService>());*/
            context.Register(Element.Service<IMessageBus, MessageBus>());
            context.Register(Element.Service<ILoggerComponent, Log4NetLogger>().AsFallback());

            // Core groups
            context.Register(Element.For<ICoreServices>().ImplementedBy<CoreServices>());
        }
    }
}