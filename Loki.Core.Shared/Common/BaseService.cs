using System;

namespace Loki.Common
{
    public abstract class BaseService : BaseObject, IInitializable, IDisposable
    {
        protected IInfrastrucure Infrastructure { get; private set; }

        private readonly Lazy<bool> initialized;

        private bool disposed = false;

        protected BaseService(IInfrastrucure infrastructure)
            : base(infrastructure.Diagnostics)
        {
            Infrastructure = infrastructure;
            initialized = new Lazy<bool>(this.InitializeService);
        }

        protected abstract bool InitializeService();

        public void Initialize()
        {
            this.Infrastructure.MessageBus.Subscribe(this);

            Log.DebugFormat("Initialized :{0}", initialized.Value);
        }

        public bool Initialized => this.initialized.IsValueCreated;

        protected void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.Infrastructure.MessageBus.Unsubscribe(this);
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}