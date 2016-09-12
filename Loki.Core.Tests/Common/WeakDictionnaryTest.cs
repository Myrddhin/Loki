using System;
using System.Linq;
using System.Threading.Tasks;

using Loki.Common.Diagnostics;
using Loki.Common.IoC.Tests;

using Moq;

using Xunit;

namespace Loki.Common
{
    [Trait("Category", "Tools")]
    public class WeakDictionnaryTest
    {
        public WeakDictionnaryTest()
        {
            DiagnosticMock = new Mock<IDiagnostics>();
            Log = new Mock<ILog>();
            DiagnosticMock.Setup(x => x.GetLog(It.IsAny<string>())).Returns(Log.Object);
            Component = new WeakDictionary<DummyClass, DummyClass>(DiagnosticMock.Object);
        }

        public WeakDictionary<DummyClass, DummyClass> Component { get; }

        public Mock<IDiagnostics> DiagnosticMock { get; }

        public Mock<ILog> Log { get; }

        [Fact(DisplayName = "KVP are weak references")]
        public void RemoveCollectedEntries()
        {
            var runner = Task.Run(
                () =>
                {
                    Component.Add(new DummyClass(), new DummyClass());
                });

            runner.Wait();

            Assert.Equal(Component.Count, 1);
            GC.Collect();
            Component.RemoveCollectedEntries();
            Assert.Equal(Component.Count, 0);
        }

        [Fact]
        public void Enumerable()
        {
            Component.Add(new DummyClass(), new DummyClass());

            Assert.Equal(Component.Keys.Count, 1);
            Assert.Equal(Component.Values.Count, 1);
            Assert.Equal(Component.Keys.Count(), 1);
            Assert.Equal(Component.Values.Count(), 1);
            Assert.Equal(Component.Count, 1);
            Assert.Equal(Component.Count(), 1);

            int i=0;
            foreach (var kvp in Component)
            {
                i++;
            }

            Assert.Equal(i,1);

            Component.Clear();
            Assert.Equal(Component.Count, 0);
            Assert.Equal(Component.Keys.Count, 0);
            Assert.Equal(Component.Values.Count, 0);
        }

        [Fact]
        public void DictionnaryCheck()
        {
            var key = new DummyClass();
            var value = new DummyClass();
            Component[key] = value;

            Assert.True(Component.ContainsKey(key));
            DummyClass buffer;

            Assert.True(Component.TryGetValue(key, out buffer));
            Assert.Equal(buffer, value);
            Component.Remove(key);
            Assert.False(Component.ContainsKey(key));
        }
    }
}