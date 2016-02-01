using System;

using Loki.Common;
using Loki.IoC.Registration;

namespace Loki.IoC
{
    public class IoCContext : IObjectContext
    {
        private const string DefaultEngine = "Windsor";

        static IoCContext()
        {
            var engineManager = new ExtensionManager<IIoCComponent>(DefaultEngine);
            engineManager.Initialize();
            Engine = engineManager.SelectedComponent;

            // TODO : remove this when default context will not be required.
            Engine.Initialize();
        }

        private static readonly IIoCComponent Engine;

        private readonly IObjectContext internalContext;

        public IoCContext()
            : this(Guid.NewGuid().ToString())
        {
        }

        public IoCContext(string name)
        {
            internalContext = Engine.CreateContext(name);

            // For type requiring context.
            internalContext.Register(Element.For<IObjectCreator, IObjectContext>().Instance(this));
        }

        public T Get<T>(string objectName) where T : class
        {
            return internalContext.Get<T>(objectName);
        }

        public T Get<T>() where T : class
        {
            return internalContext.Get<T>();
        }

        public object Get(Type type)
        {
            return internalContext.Get(type);
        }

        public object Get(Type type, string objectName)
        {
            return internalContext.Get(type, objectName);
        }

        public void Release(object objectToRelease)
        {
            internalContext.Release(objectToRelease);
        }

        private bool disposed;

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            internalContext.Dispose();
            Engine.DropContext(internalContext);
            disposed = true;
            GC.SuppressFinalize(this);
        }

        public void Initialize(params IContextInstaller[] installers)
        {
            internalContext.Initialize(installers);
        }

        public void Register<T>(ElementRegistration<T> definition) where T : class
        {
            internalContext.Register(definition);
        }
    }
}