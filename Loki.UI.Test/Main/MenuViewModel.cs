namespace Loki.UI.Test
{
    public class MenuViewModel : ContainerAllActive<NavigationElement>
    {
        public MenuViewModel(IDisplayServices coreServices)
            : base(coreServices)
        {
            DisplayName = "Menu principal";
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Items.Add(new CommandElement(Services) { DisplayName = "Rechercher", Command = Loki.Commands.ApplicationCommands.Search });
            Items.Add(new CommandElement(Services) { DisplayName = "Sauvegarder", Command = Loki.Commands.ApplicationCommands.Save });
            Items.Add(new CommandElement(Services) { DisplayName = "Rafraîchir", Command = Loki.Commands.ApplicationCommands.Refresh });
            Items.Add(new CommandElement(Services) { DisplayName = "Exporter", Command = Loki.Commands.ApplicationCommands.Export });
        }
    }
}