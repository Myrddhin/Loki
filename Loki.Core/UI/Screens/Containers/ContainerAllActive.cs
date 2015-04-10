using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Loki.UI
{
    public class ContainerAllActive<T> : ContainerBase<T> where T : class
    {
        private readonly BindableCollection<T> items = new BindableCollection<T>();
        private readonly bool openPublicItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerAllActive&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="openPublicItems">If set to <c>true</c> opens public items that are properties of this class.</param>
        public ContainerAllActive(bool openPublicItems)
            : this()
        {
            this.openPublicItems = openPublicItems;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerAllActive&lt;T&gt;"/> class.
        /// </summary>
        public ContainerAllActive()
        {
            items.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        e.NewItems.OfType<IChild>().Apply(x => x.Parent = this);
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        e.OldItems.OfType<IChild>().Apply(x => x.Parent = null);
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        e.NewItems.OfType<IChild>().Apply(x => x.Parent = this);
                        e.OldItems.OfType<IChild>().Apply(x => x.Parent = null);
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        items.OfType<IChild>().Apply(x => x.Parent = this);
                        break;
                }
            };
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <returns>The collection of children.</returns>
        public override IObservableCollection<T> Children
        {
            get { return items; }
        }

        /// <summary>
        /// Gets the items that are currently being conducted.
        /// </summary>
        public IObservableCollection<T> Items
        {
            get { return items; }
        }

        /// <summary>
        /// Activates the specified item.
        /// </summary>
        /// <param name="item">The item to activate.</param>
        public override void ActivateItem(T item)
        {
            if (item == null)
            {
                return;
            }

            item = EnsureItem(item);

            if (IsActive)
            {
                ViewModelExtenstions.TryActivate(item);
            }

            OnActivationProcessed(item, true);
        }

        /// <summary>
        /// Called to check whether or not this instance can close.
        /// </summary>
        /// <param name="callback">The implementor calls this action with the result of the close check.</param>
        public override void CanClose(Action<bool> callback)
        {
            CloseStrategy.Execute(
                items.ToList(),
                (canClose, closable) =>
                {
                    if (!canClose && closable.Any())
                    {
                        closable.OfType<IDesactivable>().Apply(x => x.Desactivate(true));
                        items.RemoveRange(closable);
                    }

                    callback(canClose);
                });
        }

        /// <summary>
        /// Deactivates the specified item.
        /// </summary>
        /// <param name="item">The item to close.</param>
        /// <param name="close">Indicates whether or not to close the item after deactivating it.</param>
        public override void DeactivateItem(T item, bool close)
        {
            if (item == null)
            {
                return;
            }

            if (close)
            {
                CloseStrategy.Execute(
                    new[] { item },
                    (canClose, closable) =>
                    {
                        if (canClose)
                        {
                            CloseItemCore(item);
                        }
                    });
            }
            else
            {
                ViewModelExtenstions.TryDeactivate(item, false);
            }
        }

        /// <summary>
        /// Ensures that an item is ready to be activated.
        /// </summary>
        /// <param name="newItem">The new item to associate.</param>
        /// <returns>The item to be activated.</returns>
        protected override T EnsureItem(T newItem)
        {
            var index = items.IndexOf(newItem);

            if (index == -1)
            {
                items.Add(newItem);
            }
            else
            {
                newItem = items[index];
            }

            return base.EnsureItem(newItem);
        }

        /// <summary>
        /// Called when activating.
        /// </summary>
        protected override void OnActivate()
        {
            items.OfType<IActivable>().Apply(x => x.Activate());
        }

        /// <summary>
        /// Called when deactivating.
        /// </summary>
        /// <param name="close">Inidicates whether this instance will be closed.</param>
        protected override void OnDesactivate(bool close)
        {
            items.OfType<IDesactivable>().Apply(x => x.Desactivate(close));
            if (close)
            {
                items.Clear();
            }
        }

        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            if (openPublicItems)
            {
                GetType().GetProperties()
                    .Where(x => x.Name != "Parent" && typeof(T).IsAssignableFrom(x.PropertyType))
                    .Select(x => x.GetValue(this, null))
                    .Cast<T>()
                    .Apply(ActivateItem);
            }
        }

        private void CloseItemCore(T item)
        {
            ViewModelExtenstions.TryDeactivate(item, true);
            items.Remove(item);
        }
    }
}