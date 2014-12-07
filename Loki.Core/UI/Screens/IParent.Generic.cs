namespace Loki.UI
{
    /// <summary>
    /// Interface used to define a specialized parent.
    /// </summary>
    /// <typeparam name="T">The type of children.</typeparam>
    public interface IParent<T> : IParent
    {
        /// <summary>
        ///   Gets the children.
        /// </summary>
        /// <returns>
        ///   The collection of children.
        /// </returns>
        new IObservableCollection<T> Children { get; }
    }
}