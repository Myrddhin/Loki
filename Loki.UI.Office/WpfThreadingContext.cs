using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

using Loki.UI.Office.Resources;

namespace Loki.UI.Office
{
    public class WpfThreadingContext : IThreadingContext
    {
        private readonly Dispatcher dispatcher;

        private readonly TaskScheduler currentScheduler;

        public WpfThreadingContext()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            var context = new DispatcherSynchronizationContext(dispatcher);
            SynchronizationContext.SetSynchronizationContext(context);
            currentScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        public void BeginOnUIThread(Action action)
        {
            ValidateDispatcher();

            dispatcher.BeginInvoke(action);
        }

        public Task OnUIThreadAsync(Action action)
        {
            CancellationToken token = new CancellationToken();
            return Task.Factory.StartNew(action, token, TaskCreationOptions.None, currentScheduler);
        }

        public void OnUIThread(Action action)
        {
            Exception exception = null;
            Action method = () =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            };

            dispatcher.Invoke(method);

            if (exception != null)
            {
                throw new TargetInvocationException(ErrorMessages.Threading_DispatchError, exception);
            }
        }

        private void ValidateDispatcher()
        {
            if (dispatcher == null)
            {
                throw new InvalidOperationException(ErrorMessages.Threading_DispatcherNotInitialized);
            }
        }

        private bool CheckAccess()
        {
            return dispatcher == null || dispatcher.CheckAccess();
        }
    }
}