namespace Loki.Common
{
    /// <summary>
    /// Weak callback base interface.
    /// </summary>
    public interface IWeakCallback
    {
        /// <summary>
        /// Gets the listener reference.
        /// </summary>
        object Listener { get; }
    }

    /// <summary>
    /// Weak event callback generic interface.
    /// </summary>
    /// <typeparam name="TArgs">The type of the args.</typeparam>
    public interface IWeakEventCallback<TArgs> : IWeakCallback
    {
        /// <summary>
        /// Invokes the callback with the specified parameters.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        bool Invoke(object sender, TArgs e);
    }
}