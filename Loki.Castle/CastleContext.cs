using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Loki.Common;
using Loki.IoC;
using Loki.IoC.Registration;

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

            this.Register(Element.For<IObjectCreator>().Instance(this));
        }

        public void Reset()
        {
            InitializeContext();
        }

        protected WindsorContainer InternalContainer { get; private set; }

        private ConcurrentDictionary<Type, Func<object>> builders = new ConcurrentDictionary<Type, Func<object>>();

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
            if (!disposed)
            {
                if (disposing)
                {
                    InternalContainer.Dispose();
                }

                disposed = true;
            }
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

            return item;
        }

        public T Get<T>() where T : class
        {
            var type = typeof(T);
            T item = null;
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

            return item;
        }

        public object Get(Type type)
        {
            object item = null;
            if (!builders.ContainsKey(type))
            {
                item = InternalContainer.Resolve(type);
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

            return item;
        }

        public IEnumerable<object> GetAll(Type type)
        {
            var result = InternalContainer.ResolveAll(type).OfType<object>();
            foreach (var awareItem in result.OfType<IContextAware>())
            {
                awareItem.SetContext(this);
            }

            return result;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var result = InternalContainer.ResolveAll<T>();

            foreach (var awareItem in result.OfType<IContextAware>())
            {
                awareItem.SetContext(this);
            }

            return result;
        }

        public object Get(Type type, string objectName)
        {
            object item = null;

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

            if (definition.Lifestyle.Type == LifestyleType.NoTracking)
            {
                RegisterNoTracking(definition);
            }

            if (!string.IsNullOrEmpty(definition.Name))
            {
                registration = registration.Named(GetName(typeof(T), definition.Name));
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

                    default:
                        break;
                }
            }

            foreach (var property in definition.ConfiguredProperties)
            {
                if (property.Ignore)
                {
                    registration = registration.PropertiesIgnore(x => x.Name == property.Key.Name);
                }
                else
                {
                    if (string.IsNullOrEmpty(property.Name))
                    {
                        registration = registration.DependsOn(global::Castle.MicroKernel.Registration.Property.ForKey(property.Key.Name).Eq(property.Value));
                    }
                    else
                    {
                        registration = registration.DependsOn(Dependency.OnComponent(property.Key.Name, GetName(property.Key.PropertyType, property.Name)));
                    }
                }
            }

            InternalContainer.Register(registration);
        }

        public void Release(object objectToRelease)
        {
            InternalContainer.Release(objectToRelease);
        }

        private void RegisterNoTracking<T>(ElementRegistration<T> definition) where T : class
        {
            Type interfaceType = definition.Types.First();
            if (!builders.ContainsKey(interfaceType))
            {
                // build new builder
                var concreteType = definition.Implementation;

                if (concreteType == null)
                {
                    concreteType = interfaceType;
                }

                var constructor = concreteType.GetConstructor(Type.EmptyTypes);

                Func<object> builder = () => constructor.Invoke(new object[0] { });

                builders.TryAdd(interfaceType, builder);
            }
        }

        public void Initialize(params IContextInstaller[] installers)
        {
            if (installers != null)
            {
                foreach (var installer in installers)
                {
                    installer.Install(this);
                }
            }
        }
    }
}