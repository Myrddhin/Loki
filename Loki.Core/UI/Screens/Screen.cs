﻿using System;
using System.ComponentModel;

//using Loki.Commands;
using Loki.Common;
using Loki.UI.Commands;

namespace Loki.UI
{
    public class Screen : DisplayElement, IScreen, IChild//, ICommandAware
    {
        public Screen(IDisplayServices coreServices)
            : base(coreServices)
        {
            //Commands = new CommandBind(this, coreServices.UI.Commands, coreServices.Core.Events, coreServices.UI.Windows);
        }

        //public CommandBind Commands
        //{
        //    get;
        //    protected set;
        //}

        /// <summary>
        /// Raised after activation occurs.
        /// </summary>
        public event EventHandler Activated = delegate { };

        /// <summary>
        /// Raised before deactivation.
        /// </summary>
        public event EventHandler<DesactivationEventArgs> AttemptingDesactivation = delegate { };

        /// <summary>
        /// Occurs when this instance is closed.
        /// </summary>
        public event EventHandler Closed = delegate { };

        /// <summary>
        /// Occurs when this instance is closed.
        /// </summary>
        public event EventHandler Closing = delegate { };

        /// <summary>
        /// Raised after deactivation.
        /// </summary>
        public event EventHandler<DesactivationEventArgs> Desactivated = delegate { };

        public Action<bool?> DialogResultSetter { get; set; }

        #region DisplayName

        private static readonly PropertyChangedEventArgs argsDisplayNameChanged = ObservableHelper.CreateChangedArgs<Screen>(x => x.DisplayName);

        private static readonly PropertyChangingEventArgs argsDisplayNameChanging = ObservableHelper.CreateChangingArgs<Screen>(x => x.DisplayName);

        private string displayName;

        public string DisplayName
        {
            get
            {
                return displayName;
            }

            set
            {
                if (value != displayName)
                {
                    NotifyChanging(argsDisplayNameChanging);
                    displayName = value;
                    NotifyChangedAndDirty(argsDisplayNameChanged);
                }
            }
        }

        #endregion DisplayName

        #region IsActive

        private static readonly PropertyChangedEventArgs argsIsActiveChanged = ObservableHelper.CreateChangedArgs<Screen>(x => x.IsActive);

        private static readonly PropertyChangingEventArgs argsIsActiveChanging = ObservableHelper.CreateChangingArgs<Screen>(x => x.IsActive);

        private bool isActive;

        public bool IsActive
        {
            get
            {
                return isActive;
            }

            private set
            {
                if (value != isActive)
                {
                    NotifyChanging(argsIsActiveChanging);
                    isActive = value;
                    NotifyChanged(argsIsActiveChanged);
                }
            }
        }

        #endregion IsActive

        #region Parent

        private static readonly PropertyChangedEventArgs argsParentChanged = ObservableHelper.CreateChangedArgs<Screen>(x => x.Parent);

        private static readonly PropertyChangingEventArgs argsParentChanging = ObservableHelper.CreateChangingArgs<Screen>(x => x.Parent);

        private object parent;

        public object Parent
        {
            get
            {
                return parent;
            }

            set
            {
                if (value != parent)
                {
                    NotifyChanging(argsParentChanging);
                    parent = value;
                    NotifyChanged(argsParentChanged);
                }
            }
        }

        #endregion Parent

        #region IsInitialized

        private static readonly PropertyChangedEventArgs argsIsInitializedChanged = ObservableHelper.CreateChangedArgs<Screen>(x => x.IsInitialized);

        private static readonly PropertyChangingEventArgs argsIsInitializedChanging = ObservableHelper.CreateChangingArgs<Screen>(x => x.IsInitialized);

        private bool initialized;

        public bool IsInitialized
        {
            get
            {
                return initialized;
            }

            set
            {
                if (value != initialized)
                {
                    NotifyChanging(argsIsInitializedChanging);
                    initialized = value;
                    NotifyChanged(argsIsInitializedChanged);
                }
            }
        }

        #endregion IsInitialized

        #region IsLoaded

        private static readonly PropertyChangedEventArgs argsIsLoadedChanged = ObservableHelper.CreateChangedArgs<Screen>(x => x.IsLoaded);

        private static readonly PropertyChangingEventArgs argsIsLoadedChanging = ObservableHelper.CreateChangingArgs<Screen>(x => x.IsLoaded);

        private bool loaded;

        public bool IsLoaded
        {
            get
            {
                return loaded;
            }

            set
            {
                if (value != loaded)
                {
                    NotifyChanging(argsIsLoadedChanging);
                    loaded = value;
                    NotifyChanged(argsIsLoadedChanged);
                }
            }
        }

        #endregion IsLoaded

        #region State

        private static readonly PropertyChangedEventArgs argsStateChanged = ObservableHelper.CreateChangedArgs<Screen>(x => x.State);

        private static readonly PropertyChangingEventArgs argsStateChanging = ObservableHelper.CreateChangingArgs<Screen>(x => x.State);

        private ICentralizedChangeTracking state;

        public ICentralizedChangeTracking State
        {
            get
            {
                return state;
            }

            set
            {
                if (value != state)
                {
                    NotifyChanging(argsStateChanging);
                    state = value;
                    NotifyChanged(argsStateChanged);
                }
            }
        }

        #endregion State

        public virtual void CanClose(Action<bool> callback)
        {
            callback(true);
        }

        void IActivable.Activate()
        {
            if (IsActive)
            {
                return;
            }

            Initialize();

            Load();

            IsActive = true;
            this.Refresh();
            Log.DebugFormat("Activating {0}.", this.DisplayName);
            OnActivate();

            Activated(
                this,
                new ActivationEventArgs
                {
                    WasInitialized = initialized
                });
        }

        void IActivable.Desactivate(bool close)
        {
            if (IsActive || (IsInitialized && close))
            {
                AttemptingDesactivation(
                    this,
                    new DesactivationEventArgs
                    {
                        WasClosed = close
                    });

                IsActive = false;
                Log.DebugFormat("Deactivating {0}.", this.displayName);
                OnDesactivate(close);

                Desactivated(
                    this,
                    new DesactivationEventArgs
                    {
                        WasClosed = close
                    });
            }
        }

        public void Initialize()
        {
            if (!IsInitialized)
            {
                Log.DebugFormat("Initializing {0}.", this);

                // subsribe to messagebus
                this.Bus.Subscribe(this);

                // configure commands.
                State = this;

                OnInitialize();

                IsInitialized = true;
            }
        }

        public void Load()
        {
            if (!IsLoaded)
            {
                Log.DebugFormat("Loading {0}.", this);
                OnLoad();
                IsLoaded = true;
            }
        }

        private bool closing;

        public void TryClose(bool? dialogResult = null)
        {
            var conductor = Parent as IConductor;
            if (conductor != null)
            {
                conductor.CloseItem(this);
            }
            else
            {
                if (closing)
                {
                    return;
                }

                closing = true;

                Closing(this, EventArgs.Empty);

                Tracking = false;

                ((IDesactivable)this).Desactivate(true);

                // unsubscribe to message bus
                this.Bus.Unsubscribe(this);

                OnClose();

               // Commands.SafeDispose();

                Log.DebugFormat("Closed {0}.", this);

                if (DialogResultSetter != null)
                {
                    DialogResultSetter(dialogResult);
                }

                Closed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when activating.
        /// </summary>
        protected virtual void OnActivate()
        {
        }

        /// <summary>
        /// Called when closing.
        /// </summary>
        protected virtual void OnClose()
        {
           // Commands.Unbind();
        }

        /// <summary>
        /// Called when deactivating.
        /// </summary>
        /// <param name="close">
        /// Inidicates whether this instance will be closed.
        /// </param>
        protected virtual void OnDesactivate(bool close)
        {
        }

        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected virtual void OnInitialize()
        {
        }

        /// <summary>
        /// Called when first load.
        /// </summary>
        protected virtual void OnLoad()
        {
        }
    }
}