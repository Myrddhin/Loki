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

        [Fact(DisplayName = "Not activated by default")]
        public void NotActivatedByDefault()
        {
            Assert.False(Screen.IsActive);
        }

        [Fact(DisplayName = "Activating raise refresh")]
        public void ActivatingRaiseRefresh()
        {
            bool raised = false;
            Screen.PropertyChanged += (s, e) => raised |= e.PropertyName == string.Empty;
            Screen.Activate();
            
            Assert.True(raised);
        }

        [Fact(DisplayName = "Activation raise property changed")]
        public void ActivatingRaisePropertyChanged()
        {
            bool raised = false;
            Screen.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(IActivable.IsActive);

            Screen.Activate();
            Assert.True(raised);
        }

        [Fact(DisplayName = "Activation change IsActive state and raise event")]
        public void ActivatingChangeFlagAndRaiseEvent()
        {
            bool raised = false;
            Screen.Activated += (s, e) => raised = true;
            Screen.Activate();
            Assert.True(Screen.IsActive);
            Assert.True(raised);
        }

        [Fact(DisplayName = "Activation does nothing on screen active")]
        public void ActivateOnlyWhenInactive()
        {
            Screen.Activate();
            bool raised = false;
            Screen.Activated += (s, e) => raised = true;
            Screen.Activate();

            Assert.False(raised);
        }

        [Fact(DisplayName = "Deactivation change IsActive state and raise event")]
        public void DesactivatesChangeFlag()
        {
            Screen.Activate();

            bool raised=false, raised2 = false;
            Screen.Desactivated += (s, e) => raised = true;
            Screen.AttemptingDesactivation += (s, e) => raised2 = true;
            Screen.Desactivate();

            Assert.False(Screen.IsActive);
            Assert.True(raised);
            Assert.True(raised2);
        }

        [Fact(DisplayName = "Deactivation does nothing on screen active")]
        public void DesactivationDoesNothingOnInactive()
        {
            bool raised = false, raised2 = false;
            Screen.Desactivated += (s, e) => raised = true;
            Screen.AttemptingDesactivation += (s, e) => raised2 = true;
            Screen.Desactivate();

            Assert.False(raised);
            Assert.False(raised2);
        }

        [Fact(DisplayName = "Message bus unsubscribe when close")]
        public void MessageBusUnSubscribeWhenClose()
        {
            Screen.TryClose(true);
            Assert.Equal(0, Infrastructure.MessageBus.Subscriptions);
        }

        [Fact(DisplayName = "Close remove tracking")]
        public void CloseRemoveTracking()
        {
            Screen.TryClose(true);
            Assert.False(Screen.Tracking);
        }

        [Fact(DisplayName = "Close raise closing and closed events")]
        public void CloseRaiseEvents()
        {
            bool closingRaised = false;
            bool closedRaised = false;
            Screen.Closed += (s, e) => closedRaised = true;
            Screen.Closing += (s, e) => closingRaised = true;
            Screen.TryClose(true);

            Assert.True(closingRaised);
            Assert.True(closedRaised);
        }

        [Fact(DisplayName = "Close Only once")]
        public void CloseOnlyOnce()
        {
            Screen.TryClose(true);
            bool closingRaised = false;
            bool closedRaised = false;
            Screen.Closed += (s, e) => closedRaised = true;
            Screen.Closing += (s, e) => closingRaised = true;

            Screen.TryClose(true);
            Assert.False(closingRaised);
            Assert.False(closedRaised);
        }
    }
}