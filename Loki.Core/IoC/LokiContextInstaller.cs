using System.Collections.Generic;

namespace Loki.IoC
{
    public class LokiContextInstaller : IContextInstaller
    {
        private List<IContextInstaller> internalInstallers = new List<IContextInstaller>();

        /// <summary>
        /// Do the install tasks in the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void Install(IObjectContext context)
        {
            foreach (var installer in internalInstallers)
            {
                installer.Install(context);
            }
        }

        /// <summary>
        /// Merges the specified installers.
        /// </summary>
        /// <param name="installers">The installers.</param>
        /// <returns>The merged installer.</returns>
        protected IContextInstaller MergeWith(params IContextInstaller[] installers)
        {
            internalInstallers.AddRange(installers);
            return this;
        }

        /// <summary>
        /// Merges the specified installers.
        /// </summary>
        /// <param name="installers">The installers.</param>
        /// <returns>The merged installer.</returns>
        public static IContextInstaller Merge(params IContextInstaller[] installers)
        {
            return new LokiContextInstaller().MergeWith(installers);
        }
    }
}