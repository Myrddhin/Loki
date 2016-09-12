using System;
using System.Collections.Generic;

namespace Loki.Common
{
    public abstract class BaseService : BaseObject, IDisposable
    {
        protected IInfrastructure Infrastructure { get; }

        private readonly HashSet<IDisposable> disposables = new HashSet<IDisposable>();

        private bool disposed;

        protected BaseService(IInfrastructure infrastructure)
            : base(infrastructure.Diagnostics)
        {
            Infrastructure = infrastructure;
            this.Infrastructure.MessageBus.Subscribe(this);
        }

        protected void RegisterDisposable(IDisposable disposable)
        {
            this.disposables.Add(disposable);
        }

        protected void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.disposables.Apply(x => x.Dispose());
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