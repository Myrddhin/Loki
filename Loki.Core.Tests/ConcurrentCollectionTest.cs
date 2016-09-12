using System.Linq;

using Loki.Common.IoC.Tests;

using Xunit;

namespace Loki.Common
{
    [Trait("Category", "Tools")]
    public class ConcurrentCollectionTest
    {
        public ConcurrentCollectionTest()
        {
            Component = new ConcurrentCollection<DummyClass>();
        }

        public ConcurrentCollection<DummyClass> Component { get; }

        [Fact(DisplayName = "Stable on add")]
        public void AddWhenIterating()
        {
            // Fill
            for (int i = 0; i < 10; i++)
            {
                Component.Add(new DummyClass());
            }

            int count = 0;
            foreach (var item in Component)
            {
                count++;
                Component.Add(new DummyClass());
            }

            Assert.Equal(count, 10);
            Assert.Equal(Component.Count(), 20);
        }

        [Fact(DisplayName = "Stable on delete")]
        public void DeleteWhenIterating()
        {
            // Fill
            for (int i = 0; i < 10; i++)
            {
                Component.Add(new DummyClass());
            }

            int count = 0;
            foreach (var item in Component)
            {
                count++;
                Component.Remove(item);
            }

            Assert.Equal(count, 10);
            Assert.True(Component.IsEmpty);
        }

        [Fact(DisplayName = "As usual collection")]
        public void AddAndRemoveOutsideOfIterating()
        {
            Component.Add(new DummyClass());
            var item = Component.First();
            Component.Remove(item);
            Assert.True(Component.IsEmpty);
        }
    }
}