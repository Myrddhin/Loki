using System;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Loki.Common.IoC;
using Loki.UI.Commands;

namespace Loki.UI.Tests
{
    public class CommonTest : IDisposable
    {
        protected IoCContainer Context { get; private set; }

        public class TestInstaller : IWindsorInstaller
        {
            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.Register(Component.For<ICommandManager>().ImplementedBy<CommandManager>());
            }
        }

        public CommonTest()
        {
            Context = new IoCContainer(true);
            Context.RegisterAssembly(typeof(IoCContainer).Assembly);
            Context.RegisterInstaller(new TestInstaller());
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed;

        protected virtual void Dispose(bool isDisposing)
        {
            if (disposed)
            {
                return;
            }

            Context.Dispose();

            disposed = true;
        }
    }
}