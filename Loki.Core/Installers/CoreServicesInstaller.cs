using Loki.IoC;
using Loki.IoC.Registration;
using Loki.UI;

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
            context.Register(Element.Service<IMessageComponent, MessageBus>());
            context.Register(Element.Service<ILoggerComponent, Log4NetLogger>().AsFallback());

            // Services groups
            context.Register(Element.For<ICoreServices>().ImplementedBy<CoreServices>());
            context.Register(Element.For<IUIServices>().ImplementedBy<UIServices>());
        }
    }
}