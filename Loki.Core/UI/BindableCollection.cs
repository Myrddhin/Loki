using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

using Loki.Common;
using Loki.Common.Diagnostics;

namespace Loki.UI
{
    public class BindableCollection<T> : ObservableCollection<T>, IObservableCollection<T>, ISupportInitialize, IBindingList, IRaiseItemChangedEvents
    {
        private readonly Func<T> addingFunctor;

        private readonly ICoreServices services;

        private readonly IThreadingContext threading;

        private readonly List<T> removedItems = new List<T>();

        #region Log

        private ILog log;

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        protected ILog Log
        {
            get
            {
                if (log == null)
                {
                    Interlocked.CompareExchange(ref log, services.Diagnostics.GetLog(LoggerName), null);
                }

                return log;
            }
        }

        /// <summary>
        /// Gets the Logger name ; must be redefined in derived classes.
        /// </summary>
        protected virtual string LoggerName
        {
            get
            {
                return this.GetType().FullName;
            }
        }

        #endregion Log

        #region ItemChanged

        public event EventHandler<ItemChangedEventArgs<T>> ItemChanged;

        protected virtual void OnItemChanged(ItemChangedEventArgs<T> e)
        {
            EventHandler<ItemChangedEventArgs<T>> handler = ItemChanged;

            if (handler != null)
            {
                threading.OnUIThread(() => handler(this, e));
            }
        }

        #endregion ItemChanged

        private int addNewPos = -1;

        [NonSerialized]
        private PropertyDescriptorCollection itemTypeProperties;

        [NonSerialized]
        private int lastChangeIndex = -1;

        private bool raiseItemChangedEvents;

        public bool IsNotifying { get; set; }

        public bool IsTrackingRemovedItems { get; set; }

        public IEnumerable<T> RemovedItems
        {
            get { return removedItems; }
        }

        public void AddRange(IEnumerable<T> items)
        {
            bool oldRaiseEvents = IsNotifying;
            int oldCount = this.Count;
            IsNotifying = false;
            var enumerable = items as List<T> ?? items.ToList();
            foreach (T item in enumerable)
            {
                Add(item);
            }

            IsNotifying = oldRaiseEvents;

            if (oldRaiseEvents)
            {
                FireListChanged(ListChangedType.Reset, -1);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, enumerable, oldCount));
            }
        }

        public virtual void BeginInit()
        {
            IsNotifying = false;
            removedItems.Clear();
        }

        public virtual void EndInit()
        {
            IsNotifying = true;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            bool oldRaiseEvents = IsNotifying;
            IsNotifying = false;
            var enumerable = items as List<T> ?? items.ToList();
            foreach (T item in enumerable)
            {
                Remove(item);
            }

            IsNotifying = oldRaiseEvents;

            if (oldRaiseEvents)
            {
                FireListChanged(ListChangedType.Reset, -1);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T>(), enumerable));
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (IsNotifying)
            {
                threading.OnUIThread(() => base.OnCollectionChanged(e));
            }

            if (IsTrackingRemovedItems && e.OldItems != null)
            {
                removedItems.AddRange(e.OldItems.Cast<T>());
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (IsNotifying)
            {
                threading.OnUIThread(() => base.OnPropertyChanged(e));
            }
        }

        #region ListChanged

        public event ListChangedEventHandler ListChanged;

        protected virtual void OnListChanged(ListChangedEventArgs e)
        {
            ListChangedEventHandler handler = ListChanged;

            if (handler != null)
            {
                threading.OnUIThread(() => handler(this, e));
            }
        }

        #endregion ListChanged

        #region Constructors

        public BindableCollection(IDisplayServices services, Func<T> addingFunctor = null)
            : this(services.Core, services.UI.Threading, addingFunctor)
        {
        }

        public BindableCollection(ICoreServices services, IThreadingContext threading, Func<T> addingFunctor = null)
        {
            this.addingFunctor = addingFunctor;
            this.services = services;
            this.threading = threading;
            Initialize();
        }

        public BindableCollection(IDisplayServices services, IEnumerable<T> list, Func<T> addingFunctor = null)
            : this(services.Core, services.UI.Threading, list, addingFunctor)
        {
        }

        public BindableCollection(ICoreServices services, IThreadingContext threading, IEnumerable<T> list, Func<T> addingFunctor = null)
            : base(list)
        {
            this.addingFunctor = addingFunctor;
            this.services = services;
            this.threading = threading;
            Initialize();
        }

        private bool ItemTypeHasDefaultConstructor
        {
            get
            {
                Type itemType = typeof(T);

                if (itemType.IsPrimitive)
                {
                    return true;
                }

                if (itemType.GetConstructor(BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null, new Type[0], null) != null)
                {
                    return true;
                }

                return false;
            }
        }

        private void Initialize()
        {
            IsTrackingRemovedItems = true;
            IsNotifying = true;

            // Check for INotifyPropertyChanged
            if (typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(T)))
            {
                // Supports INotifyPropertyChanged
                this.raiseItemChangedEvents = true;

                // Loop thru the items already in the collection and hook their change notification.
                foreach (T item in this.Items)
                {
                    BindItem(item);
                }
            }
        }

        #endregion Constructors

        #region ListChanged event

        public void ResetBindings()
        {
            FireListChanged(ListChangedType.Reset, -1);
        }

        public void ResetItem(int position)
        {
            FireListChanged(ListChangedType.ItemChanged, position);
        }

        private void FireListChanged(ListChangedType type, int index)
        {
            if (IsNotifying)
            {
                OnListChanged(new ListChangedEventArgs(type, index));
            }
        }

        #endregion ListChanged event

        #region Collection overrides

        protected override void ClearItems()
        {
            EndNew(addNewPos);

            if (this.raiseItemChangedEvents)
            {
                foreach (T item in this.Items)
                {
                    UnbindItem(item);
                }
            }

            if (IsTrackingRemovedItems)
            {
                removedItems.AddRange(this);
            }

            base.ClearItems();

            FireListChanged(ListChangedType.Reset, -1);
        }

        protected override void InsertItem(int index, T item)
        {
            EndNew(addNewPos);
            base.InsertItem(index, item);

            if (this.raiseItemChangedEvents && addNewPos != index)
            {
                BindItem(item);
            }

            FireListChanged(ListChangedType.ItemAdded, index);
        }

        protected override void RemoveItem(int index)
        {
            // Need to all RemoveItem if this on the AddNew item
            if (!(this.addNewPos >= 0 && this.addNewPos == index))
            {
                // not removing new
                EndNew(addNewPos);
                if (this.raiseItemChangedEvents)
                {
                    UnbindItem(this[index]);
                }
            }

            base.RemoveItem(index);
            FireListChanged(ListChangedType.ItemDeleted, index);
        }

        protected override void SetItem(int index, T item)
        {
            if (this.raiseItemChangedEvents)
            {
                UnbindItem(this[index]);
            }

            base.SetItem(index, item);

            if (this.raiseItemChangedEvents)
            {
                BindItem(item);
            }

            FireListChanged(ListChangedType.ItemChanged, index);
        }

        #endregion Collection overrides

        #region ICancelAddNew interface

        public virtual void CancelNew(int itemIndex)
        {
            if (addNewPos >= 0 && addNewPos == itemIndex)
            {
                RemoveItem(addNewPos);
                addNewPos = -1;
            }
        }

        public virtual void EndNew(int itemIndex)
        {
            if (addNewPos >= 0 && addNewPos == itemIndex)
            {
                if (this.raiseItemChangedEvents)
                {
                    if (this.Count > itemIndex)
                    {
                        BindItem(this[itemIndex]);
                    }
                }

                addNewPos = -1;
            }
        }

        #endregion ICancelAddNew interface

        #region Property Change Support

        private void BindItem(T item)
        {
            INotifyPropertyChanged inpc = item as INotifyPropertyChanged;

            // Note: inpc may be null if item is null, so always check.
            if (null != inpc)
            {
                //services.Events.Changed.Register(inpc, this, (x, i, args) => x.Child_PropertyChanged(i, args));
            }
        }

        private void Child_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!this.IsNotifying)
            {
                return;
            }

            if (sender == null || e == null)
            {
                // Fire reset event (per INotifyPropertyChanged spec)
                this.ResetBindings();
            }
            else
            {
                T item;

                try
                {
                    item = (T)sender;
                }
                catch (InvalidCastException)
                {
                    this.ResetBindings();
                    return;
                }

                // Find the position of the item. This should never be -1. If it is, somehow the
                // item has been removed from our list without our knowledge.
                int pos = this.lastChangeIndex;

                if (pos < 0 || pos >= this.Count || !this[pos].Equals(item))
                {
                    pos = this.IndexOf(item);
                    this.lastChangeIndex = pos;
                }

                if (pos == -1)
                {
                    this.Log.Error("Item is no longer in our list but we are still getting change notifications.");
                    this.UnbindItem(item);
                    this.ResetBindings();
                }
                else
                {
                    // Get the property descriptor
                    if (null == this.itemTypeProperties)
                    {
                        // Get Shape
                        this.itemTypeProperties = TypeDescriptor.GetProperties(typeof(T));
                        Debug.Assert(this.itemTypeProperties != null, "Null item type properties is not an expected behavior");
                    }

                    PropertyDescriptor pd = this.itemTypeProperties.Find(e.PropertyName, true);

                    // Create event args. If there was no matching property descriptor, we raise
                    // the list changed anyway.
                    ListChangedEventArgs args = new ListChangedEventArgs(ListChangedType.ItemChanged, pos, pd);
                    this.OnItemChanged(new ItemChangedEventArgs<T>(item));

                    // Fire the ItemChanged event
                    this.OnListChanged(args);
                }
            }
        }

        private void UnbindItem(T item)
        {
            INotifyPropertyChanged inpc = item as INotifyPropertyChanged;

            // Note: inpc may be null if item is null, so always check.
            if (null != inpc)
            {
               // services.Events.Changed.Unregister(inpc, this);
            }
        }

        #endregion Property Change Support

        #region IRaiseItemChangedEvents interface

        bool IRaiseItemChangedEvents.RaisesItemChangedEvents
        {
            get { return this.raiseItemChangedEvents; }
        }

        #endregion IRaiseItemChangedEvents interface

        #region IBindingList interface

        public bool AllowEdit
        {
            get { return true; }
        }

        public bool AllowNew
        {
            get { return addingFunctor != null || this.ItemTypeHasDefaultConstructor; }
        }

        public bool AllowRemove
        {
            get { return true; }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get { return ListSortDirection.Ascending; }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get { return SortPropertyCore; }
        }

        public bool IsSorted
        {
            get { return false; }
        }

        public bool SupportsChangeNotification
        {
            get { return true; }
        }

        public bool SupportsSearching
        {
            get { return false; }
        }

        public bool SupportsSorting
        {
            get { return false; }
        }

        protected virtual PropertyDescriptor SortPropertyCore
        {
            get { return null; }
        }

        public T AddNew()
        {
            return (T)(this as IBindingList).AddNew();
        }

        void IBindingList.AddIndex(PropertyDescriptor prop)
        {
            // Not supported
        }

        object IBindingList.AddNew()
        {
            // Create new item and add it to list
            object newItem = AddNewCore();

            // Record position of new item (to support cancellation later on)
            addNewPos = (newItem != null) ? IndexOf((T)newItem) : -1;

            // Return new item to caller
            return newItem;
        }

        void IBindingList.ApplySort(PropertyDescriptor prop, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor prop, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveIndex(PropertyDescriptor prop)
        {
            // Not supported
        }

        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

        protected virtual object AddNewCore()
        {
            // Allow event handler to supply the new item for us
            object newItem = this.CreateItem();

            // Add item to end of list. Note: If event handler returned an item not of type T, the
            // cast below will trigger an InvalidCastException. This is by design.
            Add((T)newItem);

            // Return new item to caller
            return newItem;
        }

        private T CreateItem()
        {
            object newItem = null;

            if (addingFunctor != null)
            {
                newItem = addingFunctor();
            }
            else if (ItemTypeHasDefaultConstructor)
            {
                // If event hander did not supply new item, create one ourselves
                Type type = typeof(T);
                var constructorInfo = type.GetConstructor(Type.EmptyTypes);
                if (constructorInfo != null)
                {
                    newItem = constructorInfo.Invoke(null);
                }
            }

            return (T)newItem;
        }

        #endregion IBindingList interface
    }
}