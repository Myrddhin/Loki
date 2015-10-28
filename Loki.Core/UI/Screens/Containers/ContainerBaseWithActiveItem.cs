using System.ComponentModel;

using Loki.Common;

namespace Loki.UI
{
    public abstract class ContainerBaseWithActiveItem<T> : ContainerBase<T>, IConductActiveItem where T : class
    {
        protected ContainerBaseWithActiveItem(IDisplayServices coreServices)
            : base(coreServices)
        {
        }

        private static readonly PropertyChangedEventArgs argsActiveItemChanged = ObservableHelper.CreateChangedArgs<ContainerBaseWithActiveItem<T>>(x => x.ActiveItem);

        private T activeItem;

        /// <summary>
        /// Gets or sets the currently active item.
        /// </summary>
        public T ActiveItem
        {
            get { return activeItem; }
            set { ActivateItem(value); }
        }

        /// <summary>
        /// Gets or sets the currently active item.
        /// </summary>
        /// <value></value>
        object IHaveActiveItem.ActiveItem
        {
            get { return ActiveItem; }
            set { ActiveItem = (T)value; }
        }

        /// <summary>
        /// Changes the active item.
        /// </summary>
        /// <param name="newItem">
        /// The new item to activate.
        /// </param>
        /// <param name="closePrevious">
        /// Indicates whether or not to close the previous active item.
        /// </param>
        protected virtual void ChangeActiveItem(T newItem, bool closePrevious)
        {
            ViewModelExtenstions.TryDeactivate(activeItem, closePrevious);

            newItem = EnsureItem(newItem);

            if (IsActive)
            {
                ViewModelExtenstions.TryActivate(newItem);
            }

            activeItem = newItem;
            OnPropertyChanged(argsActiveItemChanged);
            OnActivationProcessed(activeItem, true);
        }
    }
}