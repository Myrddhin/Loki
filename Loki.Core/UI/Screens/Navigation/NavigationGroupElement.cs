using System.Collections;
using Loki.Common;

namespace Loki.UI
{
    public class NavigationGroupElement : NavigationElement, IParent, INavigationElement
    {
        public NavigationGroupElement()
        {
            Children = new BindableCollection<NavigationElement>();
        }

        public BindableCollection<NavigationElement> Children { get; private set; }

        IObservableEnumerable IParent.Children
        {
            get { return this.Children; }
        }
    }
}