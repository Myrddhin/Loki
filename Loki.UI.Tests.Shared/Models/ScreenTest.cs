using Loki.UI.Wpf.Test.Tools;

using Xunit;

namespace Loki.UI.Models
{
    [Trait("Category", "UI models")]
    public class ScreenTest
    {
        public Screen Screen { get; set; }

        public TestDisplayInfrastructure Infrastructure { get; set; }

        public ScreenTest()
        {
            Infrastructure = new TestDisplayInfrastructure();
            Screen = new Screen(Infrastructure);
        }

        [Fact(DisplayName = "Can dispose multiple times")]
        public void DisposeAnyTimes()
        {
            Screen.Dispose();
            Screen.Dispose();
        }

        [Fact(DisplayName = "Tracking is inactive by default")]
        public void TrackingInactiveByDefault()
        {
            Assert.False(Screen.Tracking);
        }

        [Fact(DisplayName = "Message bus subscription by default")]
        public void MessageBusSubscribeByDefault()
        {
            Assert.Equal(1, Infrastructure.MessageBus.Subscriptions);
        }

        [Fact(DisplayName = "Not loaded by default")]
        public void NotLoadedByDefault()
        {
            Assert.False(Screen.IsLoaded);
        }

        [Fact(DisplayName = "Load activates tracking")]
        public void LoadActivatesTracking()
        {
            Screen.Load();
            Assert.True(Screen.IsLoaded);
            Assert.True(Screen.Tracking);
        }

        [Fact(DisplayName = "Load does not raise property changed")]
        public void LoadingNotRaisePropertyChanged()
        {
            bool raised = false;
            Screen.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(ILoadable.IsLoaded);

            Screen.Load();
            Assert.False(raised);
        }
    }
}