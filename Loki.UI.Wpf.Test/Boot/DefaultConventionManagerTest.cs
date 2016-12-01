using System.Linq;
using System.Reflection;

using Loki.UI.Bootstrap;
using Loki.UI.Wpf;

using Xunit;

namespace Loki.UI
{
    [Trait("Category", "UI Boot")]
    public class DefaultConventionManagerTest
    {
        [WpfFact(DisplayName = "Only one View - ViewModel association")]
        public void CheckAssotiations()
        {
            var manager = new DefaultConventionManager();
            var associations = manager.ViewViewModel(Assembly.GetExecutingAssembly());

            Assert.Equal(1, associations.Count);
            var first = associations.First();

            Assert.Equal("TestViewModel", first.Key);
            var value = first.Value();
            Assert.Equal(typeof(TestView), value.GetType());
        }
    }
}