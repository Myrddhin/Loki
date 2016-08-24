using System.ComponentModel;

using Xunit;

namespace Loki.UI
{
    [Trait("Category", "UI models")]
    public class StateUnitTest
    {
        public StateUnitTest()
        {
            Unit = new StateUnit();
            Unit.PropertyChanging += (s, e) => { PropertyChangedRaised = true; };
            Unit.StateChanged += (s, e) => { ++StateChangedCount; };
        }

        protected bool PropertyChangedRaised { get; set; }

        protected int StateChangedCount { get; set; }

        protected StateUnit Unit { get; private set; }

        [Fact(DisplayName = "Property changing raised when tracking active")]
        public void WithTrackingActive()
        {
            Unit.Tracking = true;
            Unit.NotifyChanging(new PropertyChangingEventArgs("Property"));
            Assert.True(PropertyChangedRaised);
        }

        [Fact(DisplayName = "Tracking is active by defauyt")]
        public void TrackingActiveByDefault()
        {
            Assert.True(Unit.Tracking);
        }

        [Fact(DisplayName = "Property changing not raised when tracking active")]
        public void WithTrackingInactive()
        {
            Unit.Tracking = false;
            Unit.NotifyChanging(new PropertyChangingEventArgs("Property"));
            Assert.False(PropertyChangedRaised);
        }

        [Fact(DisplayName = "Object default state is unchanged")]
        public void DefaultStateNotChanged()
        {
            Assert.False(Unit.IsChanged);
        }

        [Fact(DisplayName = "Tracking is enabled by default")]
        public void TrackingByDefault()
        {
            Assert.True(Unit.Tracking);
        }

        [Fact(DisplayName = "Property change with flag change object state")]
        public void NotifyChangeWithChange()
        {
            Unit.Tracking = true;
            Unit.NotifyChanged(new PropertyChangedEventArgs("Property"), true);
            Assert.True(Unit.IsChanged);
        }

        [Fact(DisplayName = "Property change with flag not set does not change object state")]
        public void NotifyChangedWithNoChanges()
        {
            Unit.Tracking = true;
            Unit.NotifyChanged(new PropertyChangedEventArgs("Property"),false);
            Assert.False(Unit.IsChanged);
        }

        [Fact(DisplayName = "Property change flag default value is false")]
        public void NotifyChangedDefaultValue()
        {
            Unit.Tracking = true;
            Unit.NotifyChanged(new PropertyChangedEventArgs("Property"));
            Assert.False(Unit.IsChanged);
        }

        [Fact(DisplayName = "AcceptChanges restores IsChanged Flag")]
        public void AcceptChangesRestoreState()
        {
            Unit.Tracking = true;
            Unit.NotifyChanged(new PropertyChangedEventArgs("Property"), true);
            Assert.True(Unit.IsChanged);
            Unit.AcceptChanges();
            Assert.False(Unit.IsChanged);
        }

        [Fact(DisplayName = "Propretry Changed with flag raise StateChanged event")]
        public void NotifyChangeEvent()
        {
            Unit.Tracking = true;
            Unit.NotifyChanged(new PropertyChangedEventArgs("Property"), true);
            Assert.Equal(1, StateChangedCount);
        }

        [Fact(DisplayName = "Proprerty Changed with no flag does nto raise StateChanged event")]
        public void NotifyChangeNoEvent()
        {
            Unit.Tracking = true;
            Unit.NotifyChanged(new PropertyChangedEventArgs("Property"), false);
            Assert.Equal(0, StateChangedCount);
        }

        [Fact(DisplayName = "AcceptChanges raise StateChanged")]
        public void AcceptChangesEvent()
        {
            Unit.Tracking = true;
            Unit.NotifyChanged(new PropertyChangedEventArgs("Property"), true);
            StateChangedCount = 0;
            Unit.AcceptChanges();
            Assert.Equal(1, StateChangedCount);
        }

        [Fact(DisplayName = "StateChanged is raised only at state change")]
        public void OnlyOneEvent()
        {
            Unit.Tracking = true;
            Unit.NotifyChanged(new PropertyChangedEventArgs("Property"), true);
            Unit.NotifyChanged(new PropertyChangedEventArgs("Property"), true);
            Assert.Equal(1, StateChangedCount);
        }
    }
}