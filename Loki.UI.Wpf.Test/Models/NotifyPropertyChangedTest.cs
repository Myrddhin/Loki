using System.ComponentModel;

using Xunit;

namespace Loki.UI
{
    [Trait("Category", "UI models")]
    public class NotifyPropertyChangedTest
    {
        [Fact(DisplayName = "Property changed raised when tracking active")]
        public void WithTrackingActive()
        {
            var item = new NotifyPropertyChanged();
            bool raised = false;
            item.PropertyChanged += (s, e) => { raised = true; };
            item.Tracking = true;
            item.NotifyChanged(new PropertyChangedEventArgs("Property"));
            Assert.True(raised);
        }

        [Fact(DisplayName = "Property changed not raised when tracking active")]
        public void WithTrackingInactive()
        {
            var item = new NotifyPropertyChanged();
            bool raised = false;
            item.PropertyChanged += (s, e) => { raised = true; };
            item.Tracking = false;
            item.NotifyChanged(new PropertyChangedEventArgs("Property"));
            Assert.False(raised);
        }

        [Fact(DisplayName = "Refresh sends property changed with event args empty")]
        public void Refresh()
        {
            var item = new NotifyPropertyChanged();
            bool raised = false;
            item.PropertyChanged += (s, e) => { raised = true; Assert.Equal(string.Empty, e.PropertyName); };
            item.Tracking = true;
            item.Refresh();
            Assert.True(raised);
        }
    }
}