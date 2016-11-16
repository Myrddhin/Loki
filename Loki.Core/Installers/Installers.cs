using Loki.IoC;

namespace Loki.Common
{
    /// <summary>
    /// Core services installers.
    /// </summary>
    public static class ServicesInstaller
    {
        private static readonly CoreServicesInstaller CoreServices = new CoreServicesInstaller();

        //private static readonly UIServicesInstaller UIServices = new UIServicesInstaller();

        /// <summary>
        /// Gets the UI services installer.
        /// </summary>
        //public static IContextInstaller UI
        //{
        //    get
        //    {
        //        return UIServices;
        //    }
        //}

        /// <summary>
        /// Gets the core services installer.
        /// </summary>
        public static IContextInstaller Core
        {
            get
            {
                return CoreServices;
            }
        }

        /// <summary>
        /// Gets all loki services installer.
        /// </summary>
        public static IContextInstaller All
        {
            get
            {
                return LokiContextInstaller.Merge(Core, UI);
            }
        }
    }
}