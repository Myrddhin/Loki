using Loki.Common;

namespace Loki.UI.Test
{
    public class NavigationMenuViewModel : ContainerAllActive<NavigationElement>
    {
        public NavigationMenuViewModel(ICoreServices services, IUIServices uiServices)
            : base(services, uiServices)
        {
            DisplayName = "Navigation";
        }

        protected override void OnInitialize()
        {
            var navMessage = new NavigationMessage<Screen>();

            base.OnInitialize();
            var portfolioGroup = new NavigationGroupElement(Services, UI) { DisplayName = "Portfolio" };
            portfolioGroup.Children.Add(new MessageElement(Services, UI) { DisplayName = "Dashboard", Message = navMessage });
            Items.Add(portfolioGroup);

            Items.Add(new NavigationGroupElement(Services, UI) { DisplayName = "Analysis" });
        }
    }
}