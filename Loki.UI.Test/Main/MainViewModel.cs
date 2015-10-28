using System.Linq;

namespace Loki.UI.Test
{
    public class MainViewModel : ContainerAllActive<Screen>
    {
        public MainViewModel(IDisplayServices coreServices)
            : base(coreServices)
        {
            DisplayName = "Loki test application";
        }

        public DocumentsViewModel Documents
        {
            get { return Items.OfType<DocumentsViewModel>().FirstOrDefault(); }
            set { EnsureItem(value); }
        }

        public MenuViewModel Menu
        {
            get { return Items.OfType<MenuViewModel>().FirstOrDefault(); }
            set { EnsureItem(value); }
        }

        public NavigationMenuViewModel Navigation
        {
            get { return Items.OfType<NavigationMenuViewModel>().FirstOrDefault(); }
            set { EnsureItem(value); }
        }
    }
}