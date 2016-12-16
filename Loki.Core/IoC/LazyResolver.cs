using System;

namespace Loki.IoC
{
    internal class LazyResolver<T> : Lazy<T>
        where T : class
    {
        public LazyResolver(IObjectCreator context)
            : base(context.Get<T>)
        {
        }
    }
}