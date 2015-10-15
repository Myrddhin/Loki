using System;
using System.Threading.Tasks;

using Loki.Common;
using Loki.Core.Tests.IoC;

using Moq;

using Xunit;

namespace Loki.Core.Tests.Common
{
    public class WeakDictionnaryTest
    {
        public WeakDictionnaryTest()
        {
            LogMock = new Mock<ILoggerComponent>();
            Log = new Mock<ILog>();
            LogMock.Setup(x => x.GetLog(It.IsAny<string>())).Returns(Log.Object);

            ErrorMock = new Mock<IErrorComponent>();
        }

        public Mock<ILoggerComponent> LogMock { get; private set; }

        public Mock<IErrorComponent> ErrorMock { get; private set; }

        public Mock<ILog> Log { get; private set; }

        [Fact]
        public void RemoveCollectedEntries()
        {
            var dict = new WeakDictionary<DummyClass, DummyClass>(LogMock.Object, ErrorMock.Object);

            var runner = Task.Run(
                () =>
                {
                    dict.Add(new DummyClass(), new DummyClass());
                });

            runner.Wait();

            Assert.Equal(dict.Count, 1);
            GC.Collect();
            dict.RemoveCollectedEntries();
            Assert.Equal(dict.Count, 0);
        }

        [Fact]
        public void Enumerable()
        {
            var dict = new WeakDictionary<DummyClass, DummyClass>(LogMock.Object, ErrorMock.Object);

            dict.Add(new DummyClass(), new DummyClass());

            Assert.Equal(dict.Keys.Count, 1);
            Assert.Equal(dict.Values.Count, 1);
            Assert.Equal(dict.Count, 1);
        }
    }
}