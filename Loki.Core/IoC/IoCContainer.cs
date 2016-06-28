using System;

namespace Loki.Core.IoC
{
    /// <summary>
    /// IoC Container class.
    /// </summary>
    public partial class IoCContainer
    {
        public static Func<IDependencyResolver> DependencyResolverFactory { get; set; } =
            () => new CastleWindsorDependencyResolver();


        protected void RegisterInfrastructure()
        {
            this.RegisterInstaller(new Loki.Common.Installers.InfrastructureInstaller());
        }
    }
}