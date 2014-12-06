using Loki.IoC;
using Loki.IoC.Registration;

namespace Loki.Common
{
    public static class ServicesInstaller
    {
        private static CoreServicesInstaller coreServices = new CoreServicesInstaller();

        private static UIServicesInstaller uiServices = new UIServicesInstaller();

        public static IContextInstaller UI
        {
            get
            {
                return uiServices;
            }
        }

        public static IContextInstaller Core
        {
            get
            {
                return coreServices;
            }
        }

        public static IContextInstaller All
        {
            get
            {
                return LokiContextInstaller.Merge(Core, UI);
            }
        }
    }
}