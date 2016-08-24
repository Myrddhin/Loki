using System.ComponentModel;

using Xunit;

namespace Loki.UI
{
    [Trait("Category", "UI models")]
    public class NotifyPropertyChangedTest
    {
        public NotifyPropertyChangedTest()
        {
            Unit = new NotifyPropertyChanged();
            Unit.PropertyChanged += (s, e) =>
                {
                    PropertyChangedRaised = true;
                    PropertyName = e.PropertyName;
                };
        }

        protected NotifyPropertyChanged Unit { get; private set; }

        protected bool PropertyChangedRaised { get; private set; }

        protected string PropertyName { get; private set; }

        [Fact(DisplayName = "Property changed raised when tracking active")]
        public void WithTrackingActive()
        {
            Unit.Tracking = true;
            Unit.NotifyChanged(new PropertyChangedEventArgs("Property"));
            Assert.True(PropertyChangedRaised);
        }

        [Fact(DisplayName = "Property changed not raised when tracking active")]
        public void WithTrackingInactive()
        {
            Unit.Tracking = false;
            Unit.NotifyChanged(new PropertyChangedEventArgs("Property"));
            Assert.False(PropertyChangedRaised);
        }

        [Fact(DisplayName = "Tracking is active by defauyt")]
        public void TrackingActiveByDefault()
        {
            Assert.True(Unit.Tracking);
        }

        [Fact(DisplayName = "Refresh sends property changed with event args empty")]
        public void Refresh()
        {
            Unit.Tracking = true;
            Unit.Refresh();
            Assert.True(PropertyChangedRaised);
            Assert.Equal(string.Empty, PropertyName);
        }
    }
}