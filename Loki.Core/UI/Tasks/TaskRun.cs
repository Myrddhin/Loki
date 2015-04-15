using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Loki.UI.Tasks
{
    public class TaskRun : TrackedObject, IDisposable
    {
        private readonly CancellationTokenSource cancellationToken;

        private readonly Task continuationTask;

        private readonly bool cancellable;

        private readonly Stopwatch stopWatch = new Stopwatch();

        private readonly Task underlyingTask;

        private bool disposed;

        public TaskRun(string label, Task task)
        {
            cancellable = false;
            Started = DateTime.Now;
            stopWatch.Start();

            Label = label;

            underlyingTask = task;
            continuationTask = task.ContinueWith(
                t => DispatchToUIThread(() =>
                {
                    stopWatch.Stop();
                    ElapsedTime = stopWatch.ElapsedMilliseconds;

                    Refresh();
                    OnTaskCompleted();
                }),
            new CancellationToken(),
            TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Current);
        }

        public TaskRun(string label, Task task, CancellationTokenSource cts)
            : this(label, task)
        {
            cancellationToken = cts;
            cancellable = true;
        }

        public event EventHandler<TaskCompletedEventArgs> TaskCompleted;

        public bool CanBeCanceled
        {
            get
            {
                return cancellable && IsRunning && !cancellationToken.IsCancellationRequested;
            }
        }

        public bool CanBeCleared
        {
            get { return !IsRunning; }
        }

        public long ElapsedTime
        {
            get;
            private set;
        }

        public bool IsRunning
        {
            get
            {
                switch (TaskStatus)
                {
                    case TaskStatus.Canceled:
                    case TaskStatus.Faulted:
                    case TaskStatus.RanToCompletion:
                    case TaskStatus.WaitingToRun:
                        return false;

                    default:
                        return true;
                }
            }
        }

        public string Label { get; set; }

        public DateTime Started
        {
            get;
            private set;
        }

        internal Task UnderlyingTask
        {
            get { return underlyingTask; }
        }

        protected virtual TaskStatus TaskStatus
        {
            get { return underlyingTask != null ? underlyingTask.Status : TaskStatus.WaitingToRun; }
        }

        public void Cancel()
        {
            if (cancellable)
            {
                cancellationToken.Cancel();
            }
        }

        public void Start(TaskScheduler scheduler)
        {
            UnderlyingTask.Start(scheduler);
        }

        public void WaitAll()
        {
            try
            {
                Task.WaitAll(underlyingTask, continuationTask);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        protected virtual void DispatchToUIThread(Action action)
        {
            action.OnUIThreadAsync();
        }

        protected virtual void OnTaskCompleted()
        {
            var handler = TaskCompleted;
            if (handler != null)
            {
                handler(this, new TaskCompletedEventArgs(this));
            }
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                Cancel();
                WaitAll();

                if (underlyingTask != null)
                {
                    underlyingTask.Dispose();
                }

                if (continuationTask != null)
                {
                    continuationTask.Dispose();
                }

                if (cancellationToken != null)
                {
                    cancellationToken.Dispose();
                }
            }

            disposed = true;
        }

        #endregion Dispose
    }
}