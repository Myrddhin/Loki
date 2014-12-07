namespace Loki.UI.Test
{
    public class NavigationMenuViewModel : ContainerAllActive<NavigationElement>
    {
        public NavigationMenuViewModel()
        {
            DisplayName = "Navigation";
        }

        protected override void OnInitialize()
        {
            var navMessage = new NavigationMessage<Screen>();

            base.OnInitialize();
            var portfolioGroup = new NavigationGroupElement() { DisplayName = "Portfolio" };
            portfolioGroup.Children.Add(new MessageElement() { DisplayName = "Dashboard", Message = navMessage });
            Items.Add(portfolioGroup);

            Items.Add(new NavigationGroupElement() { DisplayName = "Analysis" });
        }
    }
}