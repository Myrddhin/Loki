using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Loki.UI
{
    public class DefaultThreadingContext : IThreadingContext
    {
        private readonly SynchronizationContext context;

        private readonly TaskScheduler currentScheduler;

        public DefaultThreadingContext()
        {
            context = SynchronizationContext.Current;
            if (context != null)
            {
                currentScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            }
            else
            {
                currentScheduler = TaskScheduler.Default;
            }
        }

        public void BeginOnUIThread(Action action)
        {
            if (context == null)
            {
                action();
            }
            else
            {
                Exception exception = null;
                SendOrPostCallback method = (o) =>
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

                context.Post(method, null);

                if (exception != null)
                {
                    throw new TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);
                }
            }
        }

        public Task OnUIThreadAsync(Action action)
        {
            CancellationToken token = new CancellationToken();
            return Task.Factory.StartNew(action, token, TaskCreationOptions.None, currentScheduler);
        }

        public void OnUIThread(Action action)
        {
            if (context == null)
            {
                action();
            }
            else
            {
                Exception exception = null;
                SendOrPostCallback method = (o) =>
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

                context.Send(method, null);

                if (exception != null)
                {
                    throw new TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);
                }
            }
        }
    }
}