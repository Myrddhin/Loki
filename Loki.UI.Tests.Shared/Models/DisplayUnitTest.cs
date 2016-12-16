using Loki.UI.Wpf.Test.Tools;

using Xunit;

namespace Loki.UI.Models
{
    [Trait("Category", "UI models")]
    public class DisplayUnitTest
    {
        public DisplayUnit Unit { get; set; }

        public TestDisplayInfrastructure Infrastructure { get; set; }

        public DisplayUnitTest()
        {
            Infrastructure = new TestDisplayInfrastructure();
            Unit = new DisplayUnit(Infrastructure);
        }

        [Fact(DisplayName = "Tracking is inactive by default")]
        public void TrackingInactiveByDefault()
        {
            Assert.False(Unit.Tracking);
        }

        [Fact(DisplayName = "DisplayName changed")]
        public void DisplayNameChanged()
        {
            Unit.Tracking = true;
            bool raised = false;
            Unit.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(IHaveDisplayName.DisplayName);

            Unit.DisplayName = "Aplha";
            Assert.True(raised);
            Assert.False(Unit.IsChanged);
        }

        [Fact(DisplayName = "Not activated by default")]
        public void NotActivatedByDefault()
        {
            Assert.False(Unit.IsActive);
        }

        [Fact(DisplayName = "Activating raise refresh")]
        public void ActivatingRaiseRefresh()
        {
            bool raised = false;
            Unit.PropertyChanged += (s, e) => raised |= string.IsNullOrEmpty(e.PropertyName);
            Unit.Activate();

            Assert.True(raised);
        }

        [Fact(DisplayName = "Activation raise property changed")]
        public void ActivatingRaisePropertyChanged()
        {
            bool raised = false;
            Unit.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(IActivable.IsActive);

            Unit.Activate();
            Assert.True(raised);
        }

        [Fact(DisplayName = "Activation change IsActive state and raise event")]
        public void ActivatingChangeFlagAndRaiseEvent()
        {
            bool raised = false;
            Unit.Activated += (s, e) => raised = true;
            Unit.Activate();
            Assert.True(Unit.IsActive);
            Assert.True(raised);
        }

        [Fact(DisplayName = "Activation does nothing on screen active")]
        public void ActivateOnlyWhenInactive()
        {
            Unit.Activate();
            bool raised = false;
            Unit.Activated += (s, e) => raised = true;
            Unit.Activate();

            Assert.False(raised);
        }

        [Fact(DisplayName = "Deactivation change IsActive state and raise event")]
        public void DesactivatesChangeFlag()
        {
            Unit.Activate();

            bool raised = false, raised2 = false;
            Unit.Desactivated += (s, e) => raised = true;
            Unit.Desactivating += (s, e) => raised2 = true;
            Unit.Desactivate();

            Assert.False(Unit.IsActive);
            Assert.True(raised);
            Assert.True(raised2);
        }

        [Fact(DisplayName = "Deactivation does nothing on screen active")]
        public void DesactivationDoesNothingOnInactive()
        {
            bool raised = false, raised2 = false;
            Unit.Desactivated += (s, e) => raised = true;
            Unit.Desactivating += (s, e) => raised2 = true;
            Unit.Desactivate();

            Assert.False(raised);
            Assert.False(raised2);
        }

        [Fact(DisplayName = "Message bus unsubscribe when close")]
        public void MessageBusUnsubscribeWhenClose()
        {
            Unit.TryClose(true);
            Assert.Equal(0, Infrastructure.MessageBus.Subscriptions);
        }

        [Fact(DisplayName = "Close remove tracking")]
        public void CloseRemoveTracking()
        {
            Unit.TryClose(true);
            Assert.False(Unit.Tracking);
        }

        [Fact(DisplayName = "Close raise closing and closed events")]
        public void CloseRaiseEvents()
        {
            bool closingRaised = false;
            bool closedRaised = false;
            Unit.Closed += (s, e) => closedRaised = true;
            Unit.Closing += (s, e) => closingRaised = true;
            Unit.TryClose(true);

            Assert.True(closingRaised);
            Assert.True(closedRaised);
        }

        [Fact(DisplayName = "Close Only once")]
        public void CloseOnlyOnce()
        {
            Unit.TryClose(true);
            bool closingRaised = false;
            bool closedRaised = false;
            Unit.Closed += (s, e) => closedRaised = true;
            Unit.Closing += (s, e) => closingRaised = true;

            Unit.TryClose(true);
            Assert.False(closingRaised);
            Assert.False(closedRaised);
        }
    }
}