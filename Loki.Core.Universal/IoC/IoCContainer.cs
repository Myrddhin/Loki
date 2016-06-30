using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
