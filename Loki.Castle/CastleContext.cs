using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Loki.Common;
using Loki.IoC;
using Loki.IoC.Registration;
using Loki.UI;

namespace Loki.Castle
{
    public class CastleContext : BaseObject, IObjectContext
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

            Register(Element.For<IObjectCreator>().Instance(this));
        }

        public void Reset()
        {
            InitializeContext();
        }

        protected WindsorContainer InternalContainer { get; private set; }

        private readonly ConcurrentDictionary<Type, Func<object>> builders = new ConcurrentDictionary<Type, Func<object>>();

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

        private bool disposed = false;

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
            var item = InternalContainer.Resolve<T>(GetName(typeof(T), objectName));
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
            var type = typeof(T);
            T item;
            if (!builders.ContainsKey(type))
            {
                item = InternalContainer.Resolve<T>();
            }
            else
            {
                item = builders[type]() as T;
            }

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
            object item = null;

            item = !builders.ContainsKey(type) ? InternalContainer.Resolve(type) : builders[type]();

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

        public IEnumerable<object> GetAll(Type type)
        {
            var result = InternalContainer.ResolveAll(type).OfType<object>();

            // Fix enumerate closure
            var enumerable = result as object[] ?? result.ToArray();

            foreach (var awareItem in enumerable.OfType<IContextAware>())
            {
                awareItem.SetContext(this);
            }

            foreach (var initializable in enumerable.OfType<IInitializable>())
            {
                initializable.Initialize();
            }

            return enumerable;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var result = InternalContainer.ResolveAll<T>();

            foreach (var awareItem in result.OfType<IContextAware>())
            {
                awareItem.SetContext(this);
            }

            foreach (var initializable in result.OfType<IInitializable>())
            {
                initializable.Initialize();
            }

            return result;
        }

        public object Get(Type type, string objectName)
        {
            object item;

            if (!builders.ContainsKey(type))
            {
                item = InternalContainer.Resolve(objectName, type);
            }
            else
            {
                item = builders[type]();
            }

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

        private const string NameRegistrationFormat = "{0}|{1}";

        private string GetName(Type type, string name)
        {
            return string.Format(CultureInfo.InvariantCulture, NameRegistrationFormat, type.FullName, name);
        }

        public void Register<T>(ElementRegistration<T> definition) where T : class
        {
            ComponentRegistration<object> registration = Component.For(definition.Types);

            if (definition.IsFactory)
            {
                registration.AsFactory();
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