using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Loki.Common;
using Loki.UI.Tasks;

namespace Loki.UI
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AsyncScreen : Screen
    {
        #region Status

        private static PropertyChangedEventArgs argsStatusChanged = ObservableHelper.CreateChangedArgs<AsyncScreen>(x => x.Status);

        private static PropertyChangingEventArgs argsStatusChanging = ObservableHelper.CreateChangingArgs<AsyncScreen>(x => x.Status);

        private string status;

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                if (value != status)
                {
                    NotifyChanging(argsStatusChanging);
                    status = value;
                    NotifyChanged(argsStatusChanged);
                }
            }
        }

        #endregion Status

        #region Background workers

        protected ITaskConfiguration<TArg> CreateWorker<TArg, TResult>(string title, Func<TArg, Task<TResult>> workAction, Action<TResult> callbackAction)
        {
            return CreateWorker<TArg, TResult>(title, workAction, callbackAction, this.Error);
        }

        protected ITaskConfiguration<TArg> CreateWorker<TArg, TResult>(string title, Func<TArg, Task<TResult>> workAction, Action<TResult> callbackAction, Action<Exception> errorAction)
        {
            string taskDisplayTitle = string.Format(CultureInfo.InvariantCulture, "{0} : {1}", DisplayName, title);
            Func<TArg, TResult> runner = (args) =>
                {
                    Toolkit.UI.Threading.OnUIThread(() => BeginBackgroudWork(title));
                    var result = workAction(args);
                    Task.WaitAll(result);
                    return result.Result;
                };

            Action<TResult> callBack = result =>
                {
                    EndBackgroudWork(title);
                    callbackAction(result);
                };

            Action<Exception> error = ex =>
                {
                    EndBackgroudWork(title);
                    errorAction(ex);
                };

            return Tasks.CreateTask<TArg, TResult>(taskDisplayTitle, runner, callBack, error);
        }

        private HashSet<string> titles = new HashSet<string>();

        protected virtual void BeginBackgroudWork(string title)
        {
            titles.Add(title);
            if (titles.Any())
            {
                IsBusy = true;
                Status = titles.First();
            }
            else
            {
                IsBusy = false;
                Status = string.Empty;
            }

            Commands.RefreshState();
        }

        protected virtual void EndBackgroudWork(string title)
        {
            titles.Remove(title);
            if (titles.Any())
            {
                IsBusy = true;
                Status = titles.First();
            }
            else
            {
                IsBusy = false;
                Status = string.Empty;
            }

            Commands.RefreshState();
        }

        #endregion Background workers
    }
}