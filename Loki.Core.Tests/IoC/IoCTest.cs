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

        [Fact(DisplayName = "Default context is created")]
        public void DefaultContextNoNull()
        {
            Assert.NotNull(Component.DefaultContext);
        }

        [Fact(DisplayName = "Singleton as default lifetime")]
        public void SingletonAsDefault()
        {
            var ctx = Component.DefaultContext;
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

        [Fact(DisplayName = "Main context cannot be dropped")]
        public void DropMainContext()
        {
            Assert.Throws<NotSupportedException>(() => Component.DropContext(Component.DefaultContext));
        }

        [Fact(DisplayName = "Contexts tracks no reference of transients POCO")]
        public void TransientNoDisposeNorDependecyNoTrack()
        {
            var ctx = Component.DefaultContext;
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
            var ctx = Component.DefaultContext;
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
            var ctx = Component.DefaultContext;
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
            var ctx = Component.DefaultContext;
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
    }
}