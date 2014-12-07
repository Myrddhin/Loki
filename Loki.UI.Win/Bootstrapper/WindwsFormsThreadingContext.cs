using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loki.UI.Win
{
    internal class WindwsFormsThreadingContext : IThreadingContext
    {
        protected SynchronizationContext context;

        protected TaskScheduler currentScheduler;

        public WindwsFormsThreadingContext()
        {
            context = WindowsFormsSynchronizationContext.Current;
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

                context.Post(method, null);

                if (exception != null)
                {
                    throw new TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);
                }
            }
        }
    }
}