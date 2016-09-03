using System;
using System.Threading.Tasks;

using Loki.Common.IoC;

using Xunit;

namespace Loki.Common.IoC.Tests
{
    [Trait("Category", "IoC")]
    public class IoCTest
    {
        [Fact(DisplayName = "Singleton as default lifetime")]
        public void SingletonAsDefault()
        {
            var ctx = new IoCContainer(false);
            ctx.RegisterInstaller(new DummyInstaller());

            var instance1 = ctx.Resolve<DummyClass>();
            var instance2 = ctx.Resolve<DummyClass>();

            Assert.Same(instance1, instance2);
        }

        [Fact(DisplayName = "Multiple implementations of the same instance are resolvable")]
        public void ResolveAll()
        {
            IDependencyResolver ctx = IoCContainer.DependencyResolverFactory();
            ctx.OverrideInfrastructureService<IReplacement>(typeof(Replacement));
            ctx.OverrideInfrastructureService<IReplacement>(typeof(SecondReplacement));

            var test = ctx.ResolveAll<IReplacement>();

            Assert.Equal(2, test.Length);
            Assert.Equal(typeof(Replacement), test[1].GetType());
            Assert.Equal(typeof(SecondReplacement), test[0].GetType());

        }

        [Fact(DisplayName = "Register Infrastructure")]
        public void TestPostRegistration()
        {
            IDependencyResolver ctx = IoCContainer.DependencyResolverFactory();
            ctx.OverrideInfrastructureService<IReplacement>(typeof(Replacement));

            var test = ctx.Resolve<IReplacement>();

            Assert.NotNull(test);
            Assert.IsType<Replacement>(test);
        }

        [Fact(DisplayName = "Contexts tracks no reference of transients POCO")]
        public void TransientNoDisposeNorDependecyNoTrack()
        {
            var ctx = new IoCContainer(false);
            ctx.RegisterInstaller(new DummyInstaller());

            WeakReference<DummyTransient> reference = null;

            var runner = Task.Run(
                () =>
                {
                    var instance = ctx.Resolve<DummyTransient>();
                    reference = new WeakReference<DummyTransient>(instance);
                });

            runner.Wait();
            DummyTransient buffer;
            GC.Collect();
            Assert.False(reference.TryGetTarget(out buffer));
            ctx.Release(buffer);
        }

        [Fact(DisplayName = "Release objects remove references")]
        public void ReleaseReferences()
        {
            var ctx = new IoCContainer(false);
            ctx.RegisterInstaller(new DummyInstaller());

            WeakReference<DummyDisposable> reference = null;

            var runner = Task.Run(
                () =>
                {
                    var instance = ctx.Resolve<DummyDisposable>();
                    reference = new WeakReference<DummyDisposable>(instance);
                    ctx.Release(instance);
                });

            runner.Wait();
            DummyDisposable buffer;
            GC.Collect();
            Assert.False(reference.TryGetTarget(out buffer));
        }

        [Fact(DisplayName = "Contexts tracks reference of transients disposables")]
        public void TransientDisposeDependecyTrack()
        {
            var ctx = new IoCContainer(false);
            ctx.RegisterInstaller(new DummyInstaller());

            WeakReference<DummyDisposable> reference = null;

            var runner = Task.Run(
                () =>
                {
                    var instance = ctx.Resolve<DummyDisposable>();
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
            var ctx = new IoCContainer(false);
            ctx.RegisterInstaller(new DummyInstaller());

            WeakReference<DummyDependant> reference = null;

            var runner = Task.Run(
                () =>
                {
                    var instance = ctx.Resolve<DummyDependant>();
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
            var ctx = new IoCContainer(false);
            ctx.RegisterInstaller(new DummyInstaller());

            var item = ctx.Resolve<DummyClass>("Instance");
            var instance1 = ctx.Resolve<DummyClass>();
            item.DummyString = "1";
            Assert.NotEqual(item.DummyString, instance1.DummyString);
        }

        [Fact(DisplayName = "Initialisable types are initialized (simple)")]
        public void CorrectInitializeSimple()
        {
            var ctx = new IoCContainer(false);
            ctx.RegisterInstaller(new DummyInstaller());
            var init = ctx.Resolve<DummyInitializable>();
            Assert.True(init.BeginDone);
            Assert.True(init.EndDone);
        }

        [Fact(DisplayName = "Initialisable types are initialized (named)")]
        public void CorrectInitializeNamed()
        {
            var ctx = new IoCContainer(false);
            ctx.RegisterInstaller(new DummyInstaller());
            var init = ctx.Resolve<DummyInitializable>("Instance2");
            Assert.True(init.BeginDone);
            Assert.True(init.EndDone);
        }
    }
}