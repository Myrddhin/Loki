using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Loki.Common;
using Loki.IoC;

namespace Loki.Castle
{
    [Export(typeof(IIoCComponent))]
    [ExportMetadata("Type", "Windsor")]
    public class CastleEngine : IIoCComponent, IDisposable
    {
        private const string DefaultContextName = "MainContext";

        private readonly Dictionary<string, IObjectContext> contextes;

        private readonly HashSet<IContextInstaller> installers;

        public IReadOnlyDictionary<string, IObjectContext> Contexts
        {
            get
            {
                return contextes;
            }
        }

        /// <summary>
        /// Creates a new context.
        /// </summary>
        /// <param name="contextName">Name of the context.</param>
        /// <returns>
        /// The new context.
        /// </returns>
        public IObjectContext CreateContext(string contextName)
        {
            var context = new CastleContext();

            context.Initialize(installers.ToArray());

            contextes[contextName] = context;
            return context;
        }

        /// <summary>
        /// Drops the context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void DropContext(IObjectContext context)
        {
            var internalContext = Contexts.FirstOrDefault(x => x.Value == context);

            if (internalContext.Value == DefaultContext)
            {
                throw new NotSupportedException();
            }

            if (internalContext.Value != null)
            {
                internalContext.Value.Dispose();
                contextes.Remove(internalContext.Key);
            }
        }

        public void RegisterInstaller(IContextInstaller installer)
        {
            installers.Add(installer);
            foreach (var ctx in contextes.Values)
            {
                installer.Install(ctx);
            }
        }

        /// <summary>
        /// Gets the data context.
        /// </summary>
        /// <returns>The data context.</returns>
        public IObjectContext DefaultContext
        {
            get { return Contexts[DefaultContextName]; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastleEngine"/> class.
        /// </summary>
        public CastleEngine()
        {
            contextes = new Dictionary<string, IObjectContext>();
            installers = new HashSet<IContextInstaller>();
        }

        /// <summary>
        /// Initializes the engine.
        /// </summary>
        public void Initialize()
        {
            installers.Add(ServicesInstaller.All);
            CreateContext(DefaultContextName);
        }

        #region Disposable

        /// <summary>
        /// Finalizes an instance of the <see cref="CastleEngine"/> class.
        /// </summary>
        ~CastleEngine()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases all resources used by an instance of the <see cref="CastleEngine" /> class.
        /// </summary>
        /// <remarks>
        /// This method calls the virtual <see cref="Dispose(bool)" /> method, passing in
        /// <strong>true</strong>, and then suppresses finalization of the instance.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed;

        /// <summary>
        /// Releases the unmanaged resources used by an instance of the <see cref="CastleEngine" />
        /// class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">Is <strong>true</strong> to release both managed and unmanaged
        /// resources; <strong>false</strong> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            foreach (var context in Contexts)
            {
                context.Value.SafeDispose();
            }

            contextes.Clear();

            disposed = true;
        }

        #endregion Disposable
    }
}