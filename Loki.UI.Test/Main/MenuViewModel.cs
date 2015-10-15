using Loki.Common;

namespace Loki.UI.Test
{
    public class MenuViewModel : ContainerAllActive<NavigationElement>
    {
        public MenuViewModel(ICoreServices services, IUIServices uiServices)
            : base(services, uiServices)
        {
            DisplayName = "Menu principal";
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Items.Add(new CommandElement(Services, UI) { DisplayName = "Rechercher", Command = Loki.Commands.ApplicationCommands.Search });
            Items.Add(new CommandElement(Services, UI) { DisplayName = "Sauvegarder", Command = Loki.Commands.ApplicationCommands.Save });
            Items.Add(new CommandElement(Services, UI) { DisplayName = "Rafraîchir", Command = Loki.Commands.ApplicationCommands.Refresh });
            Items.Add(new CommandElement(Services, UI) { DisplayName = "Exporter", Command = Loki.Commands.ApplicationCommands.Export });
        }
    }
}