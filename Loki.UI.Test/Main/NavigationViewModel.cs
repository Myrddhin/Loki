namespace Loki.UI.Test
{
    public class NavigationMenuViewModel : ContainerAllActive<NavigationElement>
    {
        public NavigationMenuViewModel(IDisplayServices coreServices)
            : base(coreServices)
        {
            DisplayName = "Navigation";
        }

        protected override void OnInitialize()
        {
            var navMessage = new NavigationMessage<Screen>();

            base.OnInitialize();
            var portfolioGroup = new NavigationGroupElement(Services) { DisplayName = "Portfolio" };
            portfolioGroup.Children.Add(new MessageElement(Services) { DisplayName = "Dashboard", Message = navMessage });
            Items.Add(portfolioGroup);

            Items.Add(new NavigationGroupElement(Services) { DisplayName = "Analysis" });
        }
    }
}