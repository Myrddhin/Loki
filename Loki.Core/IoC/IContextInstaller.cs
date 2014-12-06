namespace Loki.IoC
{
    /// <summary>
    /// Interface for context installers.
    /// </summary>
    public interface IContextInstaller
    {
        /// <summary>
        /// Do the install tasks in the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        void Install(IObjectContext context);
    }
}