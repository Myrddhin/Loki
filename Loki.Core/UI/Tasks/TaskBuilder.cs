using System;
using System.Threading;
using System.Threading.Tasks;

using Loki.Common;

namespace Loki.UI.Tasks
{
    internal class TaskBuilder : LoggableObject
    {
        private readonly TaskComponent service;

        public TaskBuilder(TaskComponent service) : base(null)
        {
            this.service = service;
        }

        public Task CreateCancelableTask(string label, Action<CancellationToken> action, CancellationTokenSource tokenSource)
        {
            return CreateTask(label, () => action(tokenSource.Token));
        }

        public Task CreateTask(string label, Action action)
        {
            var task = new Task(
                t =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex);
                        throw;
                    }
                },
            CancellationToken.None,
            TaskCreationOptions.LongRunning);

            return task;
        }

        public Task CreateTaskWithCallback<TResult>(Func<TResult> action, Action<TResult> callback)
        {
            var task = new Task(
                t =>
                {
                    TResult res = default(TResult);
                    try
                    {
                        res = action();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex);
                        throw;
                    }
                    finally
                    {
                        service.Threads.OnUIThread(() => callback(res));
                    }
                },
            CancellationToken.None,
            TaskCreationOptions.LongRunning);

            return task;
        }
    }
}