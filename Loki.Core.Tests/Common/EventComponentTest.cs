using System;
using System.Threading.Tasks;

using Loki.Common;
using Loki.IoC.Registration;

using Moq;

using Xunit;

namespace Loki.Core.Tests.Common
{
    public class EventComponentTest : CommonTest
    {
        public EventComponentTest()
        {
            LogMock = new Mock<ILoggerComponent>();
            Log = new Mock<ILog>();
            LogMock.Setup(x => x.GetLog(It.IsAny<string>())).Returns(Log.Object);

            Context.OverrideInfrastructureInstance(LogMock.Object);

            ErrorMock = new Mock<IErrorComponent>();

            Context.OverrideInfrastructureInstance(ErrorMock.Object);

            Component = Context.Resolve<IEventComponent>();
        }

        public IEventComponent Component { get; private set; }

        public Mock<ILoggerComponent> LogMock { get; private set; }

        public Mock<IErrorComponent> ErrorMock { get; private set; }

        public Mock<ILog> Log { get; private set; }

        [Fact]
        public void PropertyChanged()
        {
            var raiser = new NotifyPropertyChangedRaiser();
            var listener = new EventListener();
            Component.PropertyChanged.Register(raiser, string.Empty, listener, (eventListener, o, arg3) => eventListener.NotifyPropertyChanged_Listen(o, arg3));

            raiser.Raise();
            Assert.NotNull(listener.ListenPropertyChangedEventArgs);
        }

        [Fact]
        public void PropertyChangedUnregister()
        {
            var raiser = new NotifyPropertyChangedRaiser();
            var listener = new EventListener();
            Component.PropertyChanged.Register(raiser, string.Empty, listener, (eventListener, o, arg3) => eventListener.NotifyPropertyChanged_Listen(o, arg3));
            Component.PropertyChanged.Unregister(raiser, string.Empty, listener);
            raiser.Raise();
            Assert.Null(listener.ListenCanExecuteChangedArgs);
        }

        [Fact]
        public void PropertyChangedUnregisterSource()
        {
            var raiser = new NotifyPropertyChangedRaiser();
            var listener = new EventListener();
            Component.PropertyChanged.Register(raiser, string.Empty, listener, (eventListener, o, arg3) => eventListener.NotifyPropertyChanged_Listen(o, arg3));
            Component.PropertyChanged.UnregisterSource(raiser);
            raiser.Raise();
            Assert.Null(listener.ListenCanExecuteChangedArgs);
        }

        [Fact]
        public void PropertyChangedNoLockOnDest()
        {
            var raiser = new NotifyPropertyChangedRaiser();
            WeakReference<EventListener> weakListener = null;
            var runner = Task.Run(
               () =>
               {
                   var listener = new EventListener();
                   weakListener = new WeakReference<EventListener>(listener);
                   Component.PropertyChanged.Register(raiser, string.Empty, listener, (eventListener, o, arg3) => eventListener.NotifyPropertyChanged_Listen(o, arg3));
               });

            runner.Wait();
            GC.Collect();
            EventListener buffer;
            Assert.False(weakListener.TryGetTarget(out buffer));
        }

        [Fact]
        public void PropertyChangedNoLockOnSource()
        {
            var listener = new EventListener();
            WeakReference<NotifyPropertyChangedRaiser> weakRaiser = null;
            var runner = Task.Run(
               () =>
               {
                   var raiser = new NotifyPropertyChangedRaiser();
                   weakRaiser = new WeakReference<NotifyPropertyChangedRaiser>(raiser);
                   Component.PropertyChanged.Register(raiser, string.Empty, listener, (eventListener, o, arg3) => eventListener.NotifyPropertyChanged_Listen(o, arg3));
               });

            runner.Wait();
            GC.Collect();
            NotifyPropertyChangedRaiser buffer;
            Assert.False(weakRaiser.TryGetTarget(out buffer));
        }

        [Fact]
        public void CanExecuteChanged()
        {
            var raiser = new NotifyCanExecuteChangedRaiser();
            var listener = new EventListener();
            Component.CanExecuteChanged.Register(raiser, listener, (eventListener, o, arg3) => eventListener.NotifyCanExecuteChanged_Listen(o, arg3));

            raiser.Raise();
            Assert.NotNull(listener.ListenCanExecuteChangedArgs);
        }

        [Fact]
        public void CanExecuteChangedUnregister()
        {
            var raiser = new NotifyCanExecuteChangedRaiser();
            var listener = new EventListener();
            Component.CanExecuteChanged.Register(raiser, listener, (eventListener, o, arg3) => eventListener.NotifyCanExecuteChanged_Listen(o, arg3));
            Component.CanExecuteChanged.Unregister(raiser, listener);
            raiser.Raise();
            Assert.Null(listener.ListenCanExecuteChangedArgs);
        }

        [Fact]
        public void CanExecuteChangedUnregisterSource()
        {
            var raiser = new NotifyCanExecuteChangedRaiser();
            var listener = new EventListener();
            Component.CanExecuteChanged.Register(raiser, listener, (eventListener, o, arg3) => eventListener.NotifyCanExecuteChanged_Listen(o, arg3));
            Component.CanExecuteChanged.UnregisterSource(raiser);
            raiser.Raise();
            Assert.Null(listener.ListenCanExecuteChangedArgs);
        }

        [Fact]
        public void CanExecuteChangedNoLockOnDest()
        {
            var raiser = new NotifyCanExecuteChangedRaiser();
            WeakReference<EventListener> weakListener = null;
            var runner = Task.Run(
               () =>
               {
                   var listener = new EventListener();
                   weakListener = new WeakReference<EventListener>(listener);
                   Component.CanExecuteChanged.Register(raiser, listener, (eventListener, o, arg3) => eventListener.NotifyCanExecuteChanged_Listen(o, arg3));
               });

            runner.Wait();
            GC.Collect();
            EventListener buffer;
            Assert.False(weakListener.TryGetTarget(out buffer));
        }

        [Fact]
        public void CanExecuteChangedNoLockOnSource()
        {
            var listener = new EventListener();
            WeakReference<NotifyCanExecuteChangedRaiser> weakRaiser = null;
            var runner = Task.Run(
               () =>
               {
                   var raiser = new NotifyCanExecuteChangedRaiser();
                   weakRaiser = new WeakReference<NotifyCanExecuteChangedRaiser>(raiser);
                   Component.CanExecuteChanged.Register(raiser, listener, (eventListener, o, arg3) => eventListener.NotifyCanExecuteChanged_Listen(o, arg3));
               });

            runner.Wait();
            GC.Collect();
            NotifyCanExecuteChangedRaiser buffer;
            Assert.False(weakRaiser.TryGetTarget(out buffer));
        }

        [Fact]
        public void RemoveCollectedEntries()
        {
            var listener = new EventListener();
            WeakReference<NotifyCanExecuteChangedRaiser> weakRaiser = null;
            var runner = Task.Run(
               () =>
               {
                   var raiser = new NotifyCanExecuteChangedRaiser();
                   weakRaiser = new WeakReference<NotifyCanExecuteChangedRaiser>(raiser);
                   Component.CanExecuteChanged.Register(raiser, listener, (eventListener, o, arg3) => eventListener.NotifyCanExecuteChanged_Listen(o, arg3));
               });
            NotifyCanExecuteChangedRaiser buffer;
            runner.Wait();
            GC.Collect();
            Component.RemoveCollectedEntries();

            Assert.False(weakRaiser.TryGetTarget(out buffer));
        }
    }
}