using System;
using System.Reflection;

namespace Loki.Common.IoC
{
    public class LightInjectDependencyResolver : IDependencyResolver
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void OverrideInfrastructureService<T>(Type type)
            where T : class
        {
            throw new NotImplementedException();
        }

        public void OverrideInfrastructureInstance<T>(T instance)
            where T : class
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>()
        {
            throw new NotImplementedException();
        }

        public T[] ResolveAll<T>()
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(string name)
        {
            throw new NotImplementedException();
        }

        public void RegisterAssembly(Assembly assembly)
        {
            throw new NotImplementedException();
        }

        public void RegisterInstaller(object installer)
        {
            throw new NotImplementedException();
        }

        public void Release(object reference)
        {
            throw new NotImplementedException();
        }
    }
}