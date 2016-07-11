using Loki.Common.Tests;

using Xunit;

namespace Loki.Common.Configuration
{
    [Trait("Category", "Diagnostics")]
    public class ConfigurationTest : CommonTest
    {
        public ConfigurationTest()
        {
            Component = Context.Resolve<IConfiguration>();
        }

        public IConfiguration Component { get; private set; }

        [Theory]
        [InlineData("Key1", "1")]
        [InlineData("Key2", "2")]
        public void GetString(string key, string value)
        {
            Assert.Equal(value, Component.GetValue(key));
        }

        [Theory]
        [InlineData("Key1", 1)]
        [InlineData("Key2", 2)]
        public void GetInt(string key, int value)
        {
            Assert.Equal(value, Component.GetValue<int>(key));
        }

        [Fact]
        public void GetNotFound()
        {
            Assert.Null(Component.GetValue("Pouet"));
        }
    }
}