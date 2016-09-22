using Xunit;

namespace Loki.UI.Models
{
    [Trait("Category", "UI models")]
    public class DisplayUnitTest
    {
        public DisplayUnit Unit { get; set; }

        public DisplayUnitTest()
        {
            Unit = new DisplayUnit();
        }

        [Fact(DisplayName = "Tracking is inactive by default")]
        public void TrackingInactiveByDefault()
        {
            Assert.False(Unit.Tracking);
        }
    }
}