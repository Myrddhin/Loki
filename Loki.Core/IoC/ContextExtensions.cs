using System;

namespace Loki.IoC
{
    public static class ContextExtensions
    {
        public static void Scope<T>(this IObjectCreator context, Action<T> scopedFunction) where T : class
        {
            T value = context.Get<T>();
            scopedFunction(value);
            context.Release(value);
        }
    }
}