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
            activator?.Activate();
        }

        /// <summary>
        /// Deactivates the item if it implements <see cref="IActivable"/>, otherwise does nothing.
        /// </summary>
        /// <param name="potentialDeactivatable">The potential deactivatable.</param>
        public static void TryDeactivate(object potentialDeactivatable)
        {
            var deactivator = potentialDeactivatable as IActivable;
            deactivator?.Desactivate();
        }

        /// <summary>
        /// Closes the item if it implements <see cref="ICloseable"/>, otherwise does nothing.
        /// </summary>
        /// <param name="potentialClosable">The potential closeable.</param>
        /// <param name="dialogResult">The dialog result.</param>
        public static void TryClose(object potentialClosable, bool? dialogResult = null)
        {
            var closable = potentialClosable as ICloseable;
            closable?.TryClose(dialogResult);
        }
    }
}