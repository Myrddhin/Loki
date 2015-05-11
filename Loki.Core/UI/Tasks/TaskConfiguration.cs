﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Loki.UI.Tasks
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class TaskConfiguration<TArgs, TResult> : ITaskConfiguration<TArgs>
    {
        private TaskScheduler initialContextScheduler;

        private TaskComponent mainService;

        public TaskConfiguration(TaskComponent service)
        {
            mainService = service;

            var context = SynchronizationContext.Current;
            if (context != null)
            {
                initialContextScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            }
            else
            {
                initialContextScheduler = TaskScheduler.Default;
            }
        }

        public string Title { get; set; }

        public Func<TArgs, TResult> Worker { get; set; }

        public Action<TResult> Callback { get; set; }

        public Action<Exception> Error { get; set; }

        private int runLock;

        public void Start(TArgs args)
        {
            int originValue = Interlocked.CompareExchange(ref runLock, 0, 1);
            if (originValue != 0)
            {
                return;
            }

            var initialTask = new Task<TResult>(
                  () =>
                  {
                      return Worker(args);
                  });

            TaskRun run = new TaskRun(Title, initialTask);
            initialTask.ContinueWith(
                t =>
                {
                    runLock = 0;
                    if (t.IsFaulted)
                    {
                        Error(t.Exception);
                    }
                    else if (t.IsCompleted)
                    {
                        Callback(t.Result);
                    }
                },
                  initialContextScheduler);

            mainService.Start(run);
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public bool IsRunning
        {
            get { return runLock != 0; }
        }
    }
}