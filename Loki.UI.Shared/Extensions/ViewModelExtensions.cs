namespace Loki.UI.Models
{
    public static class ViewModelExtensions
    {
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
            var deactivator = potentialDeactivatable as IActivable;
            if (deactivator != null)
            {
                deactivator.Desactivate(close);
            }
        }
    }
}