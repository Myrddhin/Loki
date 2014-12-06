using System;

namespace Loki.IoC
{
    public interface IContextAware
    {
        event EventHandler ContextInitialized;

        void SetContext(IObjectContext context);
    }
}