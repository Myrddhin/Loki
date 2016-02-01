namespace Loki.UI
{
    public class NavigationGroupElement : NavigationElement, IParent
    {
        public NavigationGroupElement(IDisplayServices coreServices)
            : base(coreServices)
        {
            Children = new BindableCollection<NavigationElement>(coreServices);
        }

        public BindableCollection<NavigationElement> Children { get; private set; }

        IObservableEnumerable IParent.Children
        {
            get { return this.Children; }
        }
    }
}