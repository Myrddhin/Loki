using System;

namespace Loki.UI.Tasks
{
    public interface ITaskComponent
    {
        IObservableCollection<TaskRun> Tasks { get; }

        bool ClearOnCompletion { get; set; }

        ITaskConfiguration<TArgs> CreateTask<TArgs, TResult>(string title, Func<TArgs, TResult> workAction, Action<TResult> callbackAction, Action<Exception> errorAction);

        void Cancel(TaskRun item);

        void Clear(TaskRun item);
    }
}