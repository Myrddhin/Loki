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
                container.Register(Component.For<DummyService>().OnCreate(x => x.Initialize()));
            }
        }

        private readonly DummyService service;

        private readonly IInfrastrucure infra;

        public BaseServiceTest()
        {
            var container = new IoCContainer();
            container.RegisterInstaller(new Installer());
            this.service = container.Resolve<DummyService>();
            this.infra = container.Resolve<IInfrastrucure>();
        }

        [Fact(DisplayName = "Not connected to bus when not initialized")]
        public void NoSubScribeBeforeInitialize()
        {
            int count = 0;
            var secondService= new DummyService(this.infra);
            secondService.Received += (s, e) => count++;

            this.infra.MessageBus.Publish(new SimpleMessage(), x => x());

            Assert.Equal(0, count);
        }

        [Fact(DisplayName = "Connected to bus when initialized")]
        public void WithBusWhenInitilized()
        {
            int count = 0;
            this.service.Received += (s, e) => count++;
            this.infra.MessageBus.Publish(new SimpleMessage(), x => x());

            Assert.Equal(1, count);
        }

        [Fact(DisplayName = "Service is initialized when resolved")]
        public void InitializedWhenCreated()
        {
            Assert.True(this.service.Initialized);
        }

        [Fact(DisplayName = "Service is initialized only once")]
        public void OnlyOneInitialization()
        {
            int count = 0;
            this.service.RaiseInitialized += (s, e) => count++;
            this.service.Initialize();
            Assert.Equal(0, count);
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