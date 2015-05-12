using System;
using System.Threading.Tasks;

namespace Loki.UI.Tasks
{
    public interface ITaskComponent
    {
        IObservableCollection<TaskRun> Tasks { get; }

        bool ClearOnCompletion { get; set; }

        //ITaskConfiguration<TArgs> CreateTask<TArgs, TResult>(string title, Func<TArgs, TResult> workAction, Action<TResult> callbackAction, Action<Exception> errorAction);

        ITaskConfiguration<TArgs, TResult> CreateTask<TArgs, TResult>(string title, Func<TArgs, Task<TResult>> workAction, Action<TResult> callbackAction, Action<Exception> errorAction);

        void Cancel(TaskRun item);

        void Clear(TaskRun item);
    }
}