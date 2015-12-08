using Loki.IoC.Registration;
using Loki.UI;

using Xunit;

namespace Loki.Core.Tests.UI
{
    public class TrackedObjectTest : UITest<TrackedObject>
    {
        public TrackedObjectTest()
        {
            Context.Register(Element.For<TrackedObject>());
        }

        [Fact]
        public void TestCreation()
        {
            Assert.NotNull(Component);
        }
    }
}