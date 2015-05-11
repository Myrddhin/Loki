using System;
using System.Threading;
using System.Threading.Tasks;

namespace Loki.UI.Tasks
{
    internal class TaskComponent : ITaskComponent
    {
        private readonly BindableCollection<TaskRun> tasks = new BindableCollection<TaskRun>();

        private TaskBuilder builder;

        public TaskScheduler Scheduler { get; internal set; }

        public IThreadingContext Threads { get; set; }

        public TaskComponent()
        {
            ClearOnCompletion = true;
            builder = new TaskBuilder(this);
            Scheduler = TaskScheduler.Default;
        }

        public bool ClearOnCompletion { get; set; }

        public IObservableCollection<TaskRun> Tasks
        {
            get { return tasks; }
        }

        public void Cancel(TaskRun item)
        {
            item.Cancel();
        }

        public void Clear(TaskRun item)
        {
            tasks.Remove(item);
            item.Dispose();
        }

        private void Register(TaskRun item)
        {
            Threads.OnUIThread(() =>
            {
                tasks.Add(item);
                item.TaskCompleted += OnTaskCompleted;
            });
        }

        private void Unregister(TaskRun item)
        {
            Threads.OnUIThread(() =>
            {
                item.TaskCompleted -= OnTaskCompleted;

                if (ClearOnCompletion)
                {
                    Clear(item);
                }
            });
        }

        private void OnTaskCompleted(object sender, TaskCompletedEventArgs args)
        {
            Unregister(args.Item);
        }

        /* public TaskRun StartNew<TResult>(string label, Func<CancellationToken, TResult> action, Action<TResult> callback)
         {
             var tokenSource = new CancellationTokenSource();
             var task = builder.CreateTaskWithCallback(() => action(tokenSource.Token), callback);
             var item = CreateTaskItem(label, task, tokenSource);

             Register(item);
             Start(item);

             return item;
         }*/

        public ITaskConfiguration<TArgs> CreateTask<TArgs, TResult>(string title, Func<TArgs, TResult> workAction, Action<TResult> callbackAction, Action<Exception> errorAction)
        {
            var config = new TaskConfiguration<TArgs, TResult>(this);
            config.Title = title;
            config.Worker = workAction;
            config.Callback = callbackAction;
            config.Error = errorAction;
            return config;
        }

        public ITaskConfiguration<TArgs> CreateCancellableTask<TArgs, TResult>(string title, Func<TArgs, CancellationToken, TResult> workAction, Action<TResult> callbackAction, Action<Exception> errorAction)
        {
            throw new NotImplementedException();
        }

        /*public TaskRun StartNew<TResult>(string label, Func<TResult> action, Action<TResult> callback)
        {
            var task = builder.CreateTaskWithCallback(action, callback);
            var item = CreateTaskItem(label, task);

            Register(item);
            Start(item);

            return item;
        }

        public TaskRun StartNew(string label, Action<CancellationToken> action)
        {
            var tokenSource = new CancellationTokenSource();
            var task = builder.CreateCancelableTask(label, action, tokenSource);
            var item = CreateTaskItem(label, task, tokenSource);

            Register(item);
            Start(item);

            return item;
        }

        public TaskRun StartNew(string label, Action action)
        {
            var task = builder.CreateTask(label, action);
            var item = CreateTaskItem(label, task);

            Register(item);
            Start(item);

            return item;
        }*/

        /* protected virtual TaskRun CreateTaskItem(string label, Task task)
         {
             return new TaskRun(label, task);
         }

         protected virtual TaskRun CreateTaskItem(string label, Task task, CancellationTokenSource cts)
         {
             return new TaskRun(label, task, cts);
         }*/

        internal virtual void Start(TaskRun item)
        {
            Register(item);
            item.Start(Scheduler);
        }
    }
}