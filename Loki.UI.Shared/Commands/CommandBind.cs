using Loki.UI.Models;
using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
#if WPF
using System.Windows.Input;
#endif

namespace Loki.UI.Commands
{
    internal class CommandBind<T> : ICommandBind where T : class
    {
        private readonly WeakReference<T> actor;

        private readonly Func<T, Action<object, CanExecuteCommandEventArgs>> canExecuteGetter;

        private readonly ICommand command;

        private readonly Func<T, Func<CommandEventArgs, bool>> confirmGetter;

        private readonly Func<T, Action<object, CommandEventArgs>> executeGetter;

        private readonly IDisposable subscribtion;

        private bool disposed;

        private ILokiCommand LokiCommand => command as ILokiCommand;

        private IObserver<EventPattern<PropertyChangedEventArgs>> target;

        public CommandBind(
            ICommand command,
            T actor,
            Func<T, Action<object, CanExecuteCommandEventArgs>> canFunc,
            Func<T, Action<object, CommandEventArgs>> execFunc,
            Func<T, Func<CommandEventArgs, bool>> confirmFunc)

        {
            this.actor = new WeakReference<T>(actor);
            canExecuteGetter = canFunc;
            executeGetter = execFunc;
            confirmGetter = confirmFunc;
            this.command = command;

            var notifier = actor as INotifyPropertyChanged;
            if (notifier == null || LokiCommand == null)
            {
                return;
            }

            var source = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(h => notifier.PropertyChanged += h, h => notifier.PropertyChanged -= h);
            target = Observer.Create<EventPattern<PropertyChangedEventArgs>>(RefreshCommand);
            subscribtion = source.WeakSubscribe(target);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            subscribtion?.Dispose();
            this.target = null;
            LokiCommand?.RefreshState();

            disposed = true;
        }

        private void RefreshCommand(EventPattern<PropertyChangedEventArgs> context)
        {
            LokiCommand?.RefreshState();
        }

        public Action<object, CanExecuteCommandEventArgs> CanExecute => GetFunctor(canExecuteGetter);

        public Action<object, CommandEventArgs> Execute => GetFunctor(executeGetter);

        private bool deadReference;

        public bool Alive => !deadReference && !disposed;

        public Func<CommandEventArgs, bool> ConfirmDelegate => GetFunctor(confirmGetter);

        public bool Active
        {

            get
            {
                if (this.deadReference)
                {
                    return false;
                }

                T reference;
                if (actor.TryGetTarget(out reference))
                {
                    var activable = reference as IActivable;
                    return activable == null || activable.IsActive;
                }

                deadReference = true;
                return false;
            }
        }

        private F GetFunctor<F>(Func<T, F> functor) where F : class
        {
            if (deadReference)
            {
                return default(F);
            }

            T reference;
            if (actor.TryGetTarget(out reference))
            {
                return functor?.Invoke(reference);
            }

            deadReference = true;
            return default(F);
        }
    }
}