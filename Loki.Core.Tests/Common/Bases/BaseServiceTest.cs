using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using Loki.Common.IoC;
using Loki.Common.Messages;

using Xunit;

namespace Loki.Common
{
    [Trait("Category", "Bases classes")]
    public class BaseServiceTest
    {
        public class Installer : IWindsorInstaller
        {
            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.Register(Component.For<DummyService>());
            }
        }

        private readonly DummyService service;

        private readonly IInfrastructure infra;

        public BaseServiceTest()
        {
            var container = new IoCContainer();
            container.RegisterInstaller(new Installer());
            this.service = container.Resolve<DummyService>();
            this.infra = container.Resolve<IInfrastructure>();
        }

        [Fact(DisplayName = "Connected to bus when created")]
        public void WithBusWhenInitilized()
        {
            int count = 0;
            this.service.Received += (s, e) => count++;
            this.infra.MessageBus.Publish(new SimpleMessage(), x => x());

            Assert.Equal(1, count);
        }

        [Fact(DisplayName = "Registered disposables are disposed")]
        public void RegisterdDisposablesAreDisposed()
        {
            Assert.False(this.service.SubDisposable.Disposed);
            this.service.Dispose();
            Assert.True(this.service.SubDisposable.Disposed);
        }

        [Fact(DisplayName = "Service is disposed only once")]
        public void OnlyOneDispose()
        {
            this.service.Dispose();
            this.service.Dispose();
        }

        [Fact(DisplayName = "Disconnected from bus when disposed")]
        public void NoReceiveWhenDisposed()
        {
            int count = 0;
            this.service.Received += (s, e) => count++;
            this.service.Dispose();
            this.infra.MessageBus.Publish(new SimpleMessage(), x => x());

            Assert.Equal(0, count);
        }
    }
}