﻿using System;
using System.Collections.Specialized;
using System.ComponentModel;
using Loki.Common;

namespace Loki.UI
{
    public abstract class TrackedObject : BaseObject, INotifyPropertyChangedEx, ICentralizedChangeTracking
    {
        #region Constructor

        protected TrackedObject(ICoreServices services)
            : base(services.Logger, services.Error)
        {
            Tracking = true;
        }

        #endregion Constructor

        #region Change tracking

        public static readonly PropertyChangedEventArgs RefreshAllArgs = new PropertyChangedEventArgs(string.Empty);

        private static PropertyChangedEventArgs changedChangedArgs = ObservableHelper.CreateChangedArgs<TrackedObject>(x => x.IsChanged);

        private static PropertyChangingEventArgs changedChangingArgs = ObservableHelper.CreateChangingArgs<TrackedObject>(x => x.IsChanged);

        private bool changed = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value>Is <c>true</c> if this instance is dirty; otherwise, /c>.</value>
        public bool IsChanged
        {
            get
            {
                return changed;
            }

            set
            {
                if (value != changed)
                {
                    NotifyChanging(changedChangingArgs);
                    changed = value;
                    NotifyStateChanged(EventArgs.Empty);
                    NotifyChanged(changedChangedArgs);
                }
            }
        }

        public bool Tracking { get; set; }

        /// <summary>
        /// Starts the change IsTracking.
        /// </summary>
        public virtual void AcceptChanges()
        {
            IsChanged = false;
        }

        /// <summary>
        /// Notifies the change.
        /// </summary>
        /// <param name="e">Property changed event args.</param>
        public void NotifyChanged(PropertyChangedEventArgs e)
        {
            if (Tracking)
            {
                OnPropertyChanged(e);
            }
        }

        public void Refresh()
        {
            NotifyChanged(RefreshAllArgs);
        }

        protected void NotifyChangedAndDirty(PropertyChangedEventArgs e)
        {
            if (Tracking)
            {
                NotifyChanged(e);
                if (!IsChanged)
                {
                    IsChanged = true;
                }
            }
        }

        protected void NotifyChanging(PropertyChangingEventArgs e)
        {
            if (Tracking)
            {
                OnPropertyChanging(e);
            }
        }

        protected void NotifyStateChanged(EventArgs e)
        {
            if (Tracking)
            {
                OnStateChanged(e);
            }
        }

        protected void ObservedChanged()
        {
            if (Tracking)
            {
                if (!IsChanged)
                {
                    IsChanged = true;
                }
            }
        }

        #endregion Change tracking

        #region State changed

        /// <summary>
        /// Occurs when internal state changed.
        /// </summary>
        public event EventHandler StateChanged;

        /// <summary>
        /// Raises the <see cref="StateChanged" /> event.
        /// </summary>
        /// <param name="e">The<see cref="EventArgs" /> object that provides the arguments for the
        /// event.</param>
        protected virtual void OnStateChanged(EventArgs e)
        {
            EventHandler handler = StateChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion State changed

        #region Property changed

        /// <summary>
        /// Se produit lorsqu'une valeur de propriété est modifiée.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="e"><see cref="PropertyChangedEventArgs" /> object that provides the
        /// arguments for the event.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion Property changed

        #region PropertyChanging

        /// <summary>
        /// Se produit lorsqu'une valeur de propriété va être est modifiée.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Raises the <see cref="PropertyChanging" /> event.
        /// </summary>
        /// <param name="e"><see cref="PropertyChangedEventArgs" /> object that provides the
        /// arguments for the event.</param>
        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            PropertyChangingEventHandler handler = PropertyChanging;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion PropertyChanging

        #region Initialize

        /// <summary>
        /// Signale à l'objet que l'initialisation démarre.
        /// </summary>
        public virtual void BeginInit()
        {
            Tracking = false;
        }

        /// <summary>
        /// Signale à l'objet que l'initialisation est terminée and accepts changes.
        /// </summary>
        public void EndInit()
        {
            EndInit(true);
        }

        /// <summary>
        /// Signale à l'objet que l'initialisation est terminée.
        /// </summary>
        /// <param name="stable">If set to <c>true</c>, mark the entity as clean (accept
        /// changes) .</param>
        public virtual void EndInit(bool stable)
        {
            Tracking = true;
            if (stable)
            {
                AcceptChanges();
            }
        }

        #endregion Initialize

        #region Sub collections

        protected virtual BindableCollection<T> CreateTrackedCollection<T>() where T : ICentralizedChangeTracking
        {
            return CreateTrackedCollection<T>(null);
        }

        protected virtual BindableCollection<T> CreateTrackedCollection<T>(Func<T> adder) where T : ICentralizedChangeTracking
        {
            BindableCollection<T> collection = new BindableCollection<T>(adder);

            collection.ItemChanged += SubCollection_ItemChanged;
            collection.CollectionChanged += SubCollection_CollectionChanged;

            return collection;
        }

        protected T CreateTrackedObject<T>() where T : ICentralizedChangeTracking
        {
            return CreateTrackedObject<T>(null);
        }

        protected T CreateTrackedObject<T>(Func<T> adder) where T : ICentralizedChangeTracking
        {
            Func<T> realAdder = adder == null ? ExpressionHelper.New<T>().Compile() : adder;

            T buffer = realAdder();

            TrackChanges(buffer);

            return buffer;
        }

        protected virtual void SubCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservedChanged();
        }

        protected virtual void SubCollection_ItemChanged<T>(object sender, ItemChangedEventArgs<T> e) where T : ICentralizedChangeTracking
        {
            if (e.Item.IsChanged)
            {
                ObservedChanged();
            }
        }

        protected void TrackChanges(ICentralizedChangeTracking observed)
        {
            this.WatchStateChanged(observed, o => o.Tracked_Changed);
        }

        private void Tracked_Changed(object sender, EventArgs e)
        {
            ICentralizedChangeTracking convertedSender = sender as ICentralizedChangeTracking;
            if (convertedSender != null && convertedSender.IsChanged)
            {
                ObservedChanged();
            }
        }

        #endregion Sub collections
    }
}