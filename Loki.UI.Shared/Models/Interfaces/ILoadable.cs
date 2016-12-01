namespace Loki.UI.Models
{
    public interface ILoadable
    {
        /// <summary>
        ///  Gets a value indicating whether whether or not this instance is loaded.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        void Load();
    }
}