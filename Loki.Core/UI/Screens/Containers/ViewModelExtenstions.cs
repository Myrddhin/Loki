using System;

namespace Loki.UI
{
    public static class ViewModelExtenstions
    {
        /// <summary>
        /// Activates a child whenever the specified parent is activated.
        /// </summary>
        /// <param name="child">The child to activate.</param>
        /// <param name="parent">The parent whose activation triggers the child's activation.</param>
        public static void ActivateWith(this IActivable child, IActivable parent)
        {
            var childReference = new WeakReference(child);
            EventHandler<ActivationEventArgs> handler = null;
            handler = (s, e) =>
            {
                var activatable = (IActivable)childReference.Target;
                if (activatable == null)
                {
                    ((IActivable)s).Activated -= handler;
                }
                else
                {
                    activatable.Activate();
                }
            };

            parent.Activated += handler;
        }

        /// <summary>
        /// Closes the specified item.
        /// </summary>
        /// <param name="conductor">The conductor.</param>
        /// <param name="item">The item to close.</param>
        public static void CloseItem(this IConductor conductor, object item)
        {
            conductor.DeactivateItem(item, true);
        }

        /// <summary>
        /// Closes the specified item.
        /// </summary>
        /// <param name="conductor">The conductor.</param>
        /// <param name="item">The item to close.</param>
        public static void CloseItem<T>(this ContainerBase<T> conductor, T item) where T : class
        {
            conductor.DeactivateItem(item, true);
        }

        /// <summary>
        /// Activates and Deactivates a child whenever the specified parent is Activated or Deactivated.
        /// </summary>
        /// <param name="child">The child to activate/deactivate.</param>
        /// <param name="parent">The parent whose activation/deactivation triggers the child's activation/deactivation.</param>
        public static void ConductWith<TChild, TParent>(this TChild child, TParent parent)
            where TChild : IActivable, IDesactivable
            where TParent : IActivable, IDesactivable
        {
            child.ActivateWith(parent);
            child.DeactivateWith(parent);
        }

        /// <summary>
        /// Deactivates a child whenever the specified parent is deactivated.
        /// </summary>
        /// <param name="child">The child to deactivate.</param>
        /// <param name="parent">The parent whose deactivation triggers the child's deactivation.</param>
        public static void DeactivateWith(this IDesactivable child, IDesactivable parent)
        {
            var childReference = new WeakReference(child);
            EventHandler<DesactivationEventArgs> handler = null;
            handler = (s, e) =>
            {
                var deactivatable = (IDesactivable)childReference.Target;
                if (deactivatable == null)
                {
                    ((IDesactivable)s).Desactivated -= handler;
                }
                else
                {
                    deactivatable.Desactivate(e.WasClosed);
                }
            };

            parent.Desactivated += handler;
        }

        /// <summary>
        /// Activates the item if it implements <see cref="IActivable"/>, otherwise does nothing.
        /// </summary>
        /// <param name="potentialActivatable">The potential activatable.</param>
        public static void TryActivate(object potentialActivatable)
        {
            var activator = potentialActivatable as IActivable;
            if (activator != null)
            {
                activator.Activate();
            }
        }

        /// <summary>
        /// Deactivates the item if it implements <see cref="IDesactivable"/>, otherwise does nothing.
        /// </summary>
        /// <param name="potentialDeactivatable">The potential deactivatable.</param>
        /// <param name="close">Indicates whether or not to close the item after deactivating it.</param>
        public static void TryDeactivate(object potentialDeactivatable, bool close)
        {
            var deactivator = potentialDeactivatable as IDesactivable;
            if (deactivator != null)
            {
                deactivator.Desactivate(close);
            }
        }

        public static void TryInitialize(object potentialInitializable)
        {
            var init = potentialInitializable as IInitializable;
            if (init != null)
            {
                init.Initialize();
            }
        }
    }
}