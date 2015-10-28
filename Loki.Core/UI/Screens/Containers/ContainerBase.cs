using System;
using System.Collections.Generic;

namespace Loki.UI
{
    public abstract class ContainerBase<T> : Screen, IConductor, IParent<T> where T : class
    {
        protected ContainerBase(IDisplayServices coreServices)
            : base(coreServices)
        {
        }

        private ICloseStrategy<T> closeStrategy;

        public IScreenFactory Factory { get; set; }

        /// <summary>
        /// Occurs when an activation request is processed.
        /// </summary>
        public event EventHandler<ActivationProcessedEventArgs> ActivationProcessed = delegate { };

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <returns>The collection of children.</returns>
        public abstract IObservableCollection<T> Children { get; }

        /// <summary>
        /// Gets or sets the close strategy.
        /// </summary>
        /// <value>The close strategy.</value>
        public ICloseStrategy<T> CloseStrategy
        {
            get { return closeStrategy ?? (closeStrategy = new DefaultCloseStrategy<T>()); }
            set { closeStrategy = value; }
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <returns></returns>
        IObservableEnumerable IParent.Children
        {
            get { return Children; }
        }

        /// <summary>
        /// Activates the specified item.
        /// </summary>
        /// <param name="item">The item to activate.</param>
        public abstract void ActivateItem(T item);

        /// <summary>
        /// Deactivates the specified item.
        /// </summary>
        /// <param name="item">The item to close.</param>
        /// <param name="close">Indicates whether or not to close the item after deactivating it.</param>
        public abstract void DeactivateItem(T item, bool close);

        /// <summary>
        /// Activates the specified item.
        /// </summary>
        /// <param name="item">The item to activate.</param>
        void IConductor.ActivateItem(object item)
        {
            ActivateItem((T)item);
        }

        /// <summary>
        /// Deactivates the specified item.
        /// </summary>
        /// <param name="item">The item to close.</param>
        /// <param name="close">Indicates whether or not to close the item after deactivating it.</param>
        void IConductor.DeactivateItem(object item, bool close)
        {
            DeactivateItem((T)item, close);
        }

        /// <summary>
        /// Ensures that an item is ready to be activated.
        /// </summary>
        /// <param name="newItem">New item.</param>
        /// <returns>The item to be activated.</returns>
        protected virtual T EnsureItem(T newItem)
        {
            var node = newItem as IChild;
            if (node != null && node.Parent != this)
            {
                node.Parent = this;
            }

            return newItem;
        }

        /// <summary>
        /// Called by a subclass when an activation needs processing.
        /// </summary>
        /// <param name="item">The item on which activation was attempted.</param>
        /// <param name="success">If set to <c>true</c> activation was successful.</param>
        protected virtual void OnActivationProcessed(T item, bool success)
        {
            if (item == null)
            {
                return;
            }

            ActivationProcessed(
                this,
                new ActivationProcessedEventArgs
                {
                    Item = item,
                    Success = success
                });
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Children.Apply(x => ViewModelExtenstions.TryInitialize(x));
        }
    }
}