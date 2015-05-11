namespace Loki.UI
{
    /// <summary>
    /// Denotes a node within a parent/child hierarchy.
    /// </summary>
    /// <typeparam name="TParent">The type of parent.</typeparam>
    public interface IChild<TParent> : IChild
    {
        /// <summary>
        /// Gets or sets the Parent.
        /// </summary>
        new TParent Parent { get; set; }
    }
}