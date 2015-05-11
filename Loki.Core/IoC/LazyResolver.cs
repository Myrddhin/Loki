using System;
using Loki.Common;

namespace Loki.IoC
{
    internal class LazyResolver<T> : Lazy<T> where T : class
    {
        public LazyResolver(IObjectCreator context)
            : base(context.Get<T>)
        {
        }

        public LazyResolver()
            : this(Toolkit.IoC.DefaultContext)
        {
        }
    }
}