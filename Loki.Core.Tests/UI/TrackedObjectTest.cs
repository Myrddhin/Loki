using System.ComponentModel;

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

        [Fact]
        public void TestPropertyChangedNotDirty()
        {
            Component.AcceptChanges();
            Component.NotifyChanged(new PropertyChangedEventArgs("Test"));
            Assert.False(Component.IsChanged);
        }

        [Fact]
        public void TestPropertyChangedDirtySetDirty()
        {
            Component.AcceptChanges();
            Component.NotifyChangedAndDirty(new PropertyChangedEventArgs("Test"));
            Assert.True(Component.IsChanged);
        }

        [Fact]
        public void PropertyChangedEvent()
        {
            var evt = new PropertyChangedEventArgs("Test");
            bool check = false;
            Component.PropertyChanged += (s, e) => check = e.PropertyName == evt.PropertyName;
            Component.NotifyChanged(evt);
            Assert.True(check);
        }

        [Fact]
        public void PropertyChangingEvent()
        {
            var evt = new PropertyChangingEventArgs("Test");
            bool check = false;
            Component.PropertyChanging += (s, e) => check = e.PropertyName == evt.PropertyName;
            Component.NotifyChanging(evt);
            Assert.True(check);
        }

        [Fact]
        public void ChangedEvent()
        {
            var evt = new PropertyChangedEventArgs("Test");
            bool check = false;
            Component.StateChanged += (s, e) => check = true;
            Component.NotifyChangedAndDirty(evt);
            Assert.True(check);
        }
    }
}