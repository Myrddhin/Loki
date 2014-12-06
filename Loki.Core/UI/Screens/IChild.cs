namespace Loki.UI
{
    /// <summary>
    /// Denotes a node within a parent/child hierarchy.
    /// </summary>
    public interface IChild
    {
        /// <summary>
        /// Gets or sets the Parent.
        /// </summary>
        object Parent { get; set; }
    }
}