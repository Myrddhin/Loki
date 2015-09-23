using System.Collections.Generic;

namespace Loki.IoC
{
    /// <summary>
    /// Interface for application engines.
    /// </summary>
    public interface IIoCComponent
    {
        /// <summary>
        /// Initializes the engine.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Gets the data context.
        /// </summary>
        /// <returns>The data context.</returns>
        IObjectContext DefaultContext { get; }

        /// <summary>
        /// Creates a new context.
        /// </summary>
        /// <param name="contextName">Name of the context.</param>
        /// <returns>The new context.</returns>
        IObjectContext CreateContext(string contextName);

        /// <summary>
        /// Drops the context.
        /// </summary>
        /// <param name="context">The context.</param>
        void DropContext(IObjectContext context);

        /// <summary>
        /// Gets all the created contexts.
        /// </summary>
        /// <value>
        /// The contexts.
        /// </value>
        IReadOnlyDictionary<string, IObjectContext> Contexts { get; }
    }
}