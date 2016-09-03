using System;

namespace Loki.Common.IoC
{
    /// <summary>
    /// IoC Container class.
    /// </summary>
    public partial class IoCContainer
    {
        public static Func<IDependencyResolver> DependencyResolverFactory { get; set; } =
            () => new LightInjectDependencyResolver();

        protected void RegisterInfrastructure()
        {
        }
    }
}