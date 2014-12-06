namespace Loki.IoC.Registration
{
    /// <summary>
    /// Lifestyle types.
    /// </summary>
    public enum LifestyleType
    {
        /// <summary>
        /// Singleton type.
        /// </summary>
        Singleton,

        /// <summary>
        /// Pooled type.
        /// </summary>
        Pooled,

        /// <summary>
        /// Transient type.
        /// </summary>
        Transient,

        /// <summary>
        /// No tracking.
        /// </summary>
        NoTracking,

        /// <summary>
        /// Per HTTP request.
        /// </summary>
        PerRequest
    }
}