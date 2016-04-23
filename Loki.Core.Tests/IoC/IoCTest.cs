using System;
using System.Threading.Tasks;

using Loki.IoC;
using Loki.IoC.Registration;

using Xunit;

namespace Loki.Core.Tests.IoC
{
    public abstract class IoCTest
    {
        public IIoCComponent Component { get; protected set; }

        [Fact(DisplayName = "Singleton as default lifetime")]
        public void SingletonAsDefault()
        {
            var ctx = Component.CreateContext("Default");
            ctx.Register(Element.For<DummyClass>());

            var instance1 = ctx.Get<DummyClass>();
            var instance2 = ctx.Get<DummyClass>();

            Assert.Same(instance1, instance2);
        }

        [Fact(DisplayName = "Dropped contextes are removed")]
        public void CreateAndDropContext()
        {
            // Start count
            var ctxCount = Component.Contexts.Count;

            // Create
            var ctx = Component.CreateContext("Test");
            var addCtxCount = Component.Contexts.Count;
            Assert.Equal(addCtxCount, ctxCount + 1);

            // Remove
            Component.DropContext(ctx);
            var removeCtxCount = Component.Contexts.Count;
            Assert.Equal(removeCtxCount, ctxCount);
        }

        [Fact(DisplayName = "Contexts tracks no reference of transients POCO")]
        public void TransientNoDisposeNorDependecyNoTrack()
        {
            var ctx = Component.CreateContext("Default");
            ctx.Register(Element.For<DummyClass>().Lifestyle.Transient);

            WeakReference<DummyClass> reference = null;

            var runner = Task.Run(
                () =>
                {
                    var instance = ctx.Get<DummyClass>();
                    reference = new WeakReference<DummyClass>(instance);
                });

            runner.Wait();
            DummyClass buffer;
            GC.Collect();
            Assert.False(reference.TryGetTarget(out buffer));
            ctx.Release(buffer);
        }

        [Fact(DisplayName = "Release objects remove references")]
        public void ReleaseReferences()
        {
            var ctx = Component.CreateContext("Default");
            ctx.Register(Element.For<DummyDisposable>().Lifestyle.Transient);

            WeakReference<DummyDisposable> reference = null;

            var runner = Task.Run(
                () =>
                {
                    var instance = ctx.Get<DummyDisposable>();
                    reference = new WeakReference<DummyDisposable>(instance);
                    ctx.Release(instance);
                });

            runner.Wait();
            DummyDisposable buffer;
            GC.Collect();
            Assert.False(reference.TryGetTarget(out buffer));
        }

        [Fact(DisplayName = "Contexts tracks reference of transients disposables")]
        public void TransientDisponeNorDependecyTrack()
        {
            var ctx = Component.CreateContext("Default");
            ctx.Register(Element.For<DummyDisposable>().Lifestyle.Transient);

            WeakReference<DummyDisposable> reference = null;

            var runner = Task.Run(
                () =>
                {
                    var instance = ctx.Get<DummyDisposable>();
                    reference = new WeakReference<DummyDisposable>(instance);
                });

            runner.Wait();
            DummyDisposable buffer;
            GC.Collect();
            Assert.True(reference.TryGetTarget(out buffer));
        }

        [Fact(DisplayName = "Contexts tracks reference of transients with disposables depdendencies")]
        public void TransientDependecyTrack()
        {
            var ctx = Component.CreateContext("Default");
            ctx.Register(Element.For<DummyDisposable>().Lifestyle.Transient);
            ctx.Register(Element.For<DummyDependant>().Lifestyle.Transient);

            WeakReference<DummyDependant> reference = null;

            var runner = Task.Run(
                () =>
                {
                    var instance = ctx.Get<DummyDependant>();
                    reference = new WeakReference<DummyDependant>(instance);
                });

            runner.Wait();
            DummyDependant buffer;
            GC.Collect();
            Assert.True(reference.TryGetTarget(out buffer));
        }

        [Fact(DisplayName = "Named registration")]
        public void NamedRegistration()
        {
            var ctx = Component.CreateContext("Default");
            var instance1 = new DummyClass();
            instance1.DummyString = "1";
            ctx.Register(Element.For<DummyClass>());
            ctx.Register(Element.For<DummyClass>().Instance(instance1).Named("Instance"));

            var item = ctx.Get<DummyClass>("Instance");
            Assert.Equal(item.DummyString, instance1.DummyString);
        }

        [Fact(DisplayName = "Named registration with type")]
        public void NamedRegistrationWithType()
        {
            var ctx = Component.CreateContext("Default");
            var instance1 = new DummyClass { DummyString = "1" };
            ctx.Register(Element.For<DummyClass>().Lifestyle.Transient);
            ctx.Register(Element.For<DummyClass>().Instance(instance1).Named("Instance"));

            var item = ctx.Get(typeof(DummyClass), "Instance") as DummyClass;
            Assert.NotNull(item);
            Assert.Equal(item.DummyString, instance1.DummyString);
        }

        [Fact(DisplayName = "Simple registration with type")]
        public void RegistrationWithType()
        {
            var ctx = Component.CreateContext("Default");
            ctx.Register(Element.For<DummyClass>());

            var item = ctx.Get(typeof(DummyClass)) as DummyClass;
            Assert.NotNull(item);
        }

        [Fact(DisplayName = "Initialisable types are initialized (simple)")]
        public void CorrectInitializeSimple()
        {
            var ctx = Component.CreateContext("Default");
            ctx.Register(Element.For<DummyInitializable>());
            var init = ctx.Get<DummyInitializable>();
            Assert.True(init.IsInitialized);
        }

        [Fact(DisplayName = "Initialisable types are initialized (named)")]
        public void CorrectInitializeNamed()
        {
            var ctx = Component.CreateContext("Default");
            ctx.Register(Element.For<DummyInitializable>().Named("Instance"));
            var init = ctx.Get<DummyInitializable>("Instance");
            Assert.True(init.IsInitialized);
        }

        [Fact(DisplayName = "Initialisable types are initialized (with type)")]
        public void CorrectInitializeSimpleWithType()
        {
            var ctx = Component.CreateContext("Default");
            ctx.Register(Element.For<DummyInitializable>());
            var init = ctx.Get(typeof(DummyInitializable)) as DummyInitializable;
            Assert.NotNull(init);
            Assert.True(init.IsInitialized);
        }

        [Fact(DisplayName = "Initialisable types are initialized (with type and name)")]
        public void CorrectInitializeNamedWithType()
        {
            var ctx = Component.CreateContext("Default");
            ctx.Register(Element.For<DummyInitializable>().Named("Instance"));
            var init = ctx.Get(typeof(DummyInitializable), "Instance") as DummyInitializable;
            Assert.NotNull(init);
            Assert.True(init.IsInitialized);
        }

        [Fact(DisplayName = "Null protection on initialize")]
        public void NullInitialize()
        {
            var ctx = Component.CreateContext("Default");
            ctx.Initialize(null);
        }
    }
}