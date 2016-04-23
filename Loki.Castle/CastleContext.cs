using System;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using Loki.IoC;
using Loki.IoC.Registration;
using Loki.UI;

namespace Loki.Castle
{
    public class CastleContext : IObjectContext
    {
        public CastleContext()
        {
            InitializeContext();
        }

        private void InitializeContext()
        {
            InternalContainer.SafeDispose();
            InternalContainer = new WindsorContainer();
            InternalContainer.Kernel.AddFacility<TypedFactoryFacility>();
        }

        protected WindsorContainer InternalContainer { get; private set; }

        #region Disposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CastleContext()
        {
            Dispose(false);
        }

        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                InternalContainer.Dispose();
            }

            disposed = true;
        }

        #endregion Disposable

        #region Builders

        public T Get<T>(string objectName) where T : class
        {
            var item = InternalContainer.Resolve<T>(objectName);
            var awareItem = item as IContextAware;
            if (awareItem != null)
            {
                awareItem.SetContext(this);
            }

            var initializable = item as IInitializable;
            if (initializable != null)
            {
                initializable.Initialize();
            }

            return item;
        }

        public T Get<T>() where T : class
        {
            var item = this.InternalContainer.Resolve<T>();

            var awareItem = item as IContextAware;
            if (awareItem != null)
            {
                awareItem.SetContext(this);
            }

            var initializable = item as IInitializable;
            if (initializable != null)
            {
                initializable.Initialize();
            }

            return item;
        }

        public object Get(Type type)
        {
            object item = InternalContainer.Resolve(type);

            var awareItem = item as IContextAware;
            if (awareItem != null)
            {
                awareItem.SetContext(this);
            }

            var initializable = item as IInitializable;
            if (initializable != null)
            {
                initializable.Initialize();
            }

            return item;
        }

        public object Get(Type type, string objectName)
        {
            object item = InternalContainer.Resolve(objectName, type);

            var awareItem = item as IContextAware;
            if (awareItem != null)
            {
                awareItem.SetContext(this);
            }

            var initializable = item as IInitializable;
            if (initializable != null)
            {
                initializable.Initialize();
            }

            return item;
        }

        #endregion Builders

        public void Register<T>(ElementRegistration<T> definition) where T : class
        {
            ComponentRegistration<object> registration = Component.For(definition.Types);

            if (definition.IsFactory)
            {
                registration.AsFactory();
            }

            if (definition.IsDefault)
            {
                registration.IsDefault();
            }

            if (definition.IsFallback)
            {
                registration.IsFallback();
            }

            if (!string.IsNullOrEmpty(definition.Name))
            {
                registration = registration.Named(definition.Name);
            }

            if (definition.Lifestyle.SingletonInstance != default(T))
            {
                registration.Instance(definition.Lifestyle.SingletonInstance).LifestyleSingleton();
            }
            else
            {
                registration = registration.ImplementedBy(definition.Implementation).IsDefault();

                switch (definition.Lifestyle.Type)
                {
                    case LifestyleType.Singleton:
                        registration = registration.LifestyleSingleton();
                        break;

                    case LifestyleType.Pooled:
                        registration = registration.LifestylePooled(definition.Lifestyle.PoolSizeMin, definition.Lifestyle.PoolSizeMax);
                        break;

                    case LifestyleType.Transient:
                        registration = registration.LifestyleTransient();
                        break;

                    case LifestyleType.PerRequest:
                        registration = registration.LifestylePerWebRequest();
                        break;
                }
            }

            foreach (var property in definition.ConfiguredProperties)
            {
                if (property.Ignore)
                {
                    var iterateProperty = property;
                    registration = registration.PropertiesIgnore(x => x.Name == iterateProperty.Key.Name);
                }
                else
                {
                    registration = string.IsNullOrEmpty(property.Name) ?
                        registration.DependsOn(Property.ForKey(property.Key.Name).Eq(property.Value))
                        : registration.DependsOn(Dependency.OnComponent(property.Key.Name, property.Name));
                }
            }

            InternalContainer.Register(registration);
        }

        public void Release(object objectToRelease)
        {
            var awareItem = objectToRelease as IContextAware;
            if (awareItem != null)
            {
                awareItem.Release();
            }

            InternalContainer.Release(objectToRelease);
        }

        public void Initialize(params IContextInstaller[] installers)
        {
            if (installers == null)
            {
                return;
            }

            foreach (var installer in installers)
            {
                installer.Install(this);
            }
        }
    }
}