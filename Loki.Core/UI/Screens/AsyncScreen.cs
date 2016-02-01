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
        public AsyncScreen(IDisplayServices coreServices)
            : base(coreServices)
        {
        }

        #region Status

        private static readonly PropertyChangedEventArgs argsStatusChanged = ObservableHelper.CreateChangedArgs<AsyncScreen>(x => x.Status);

        private static readonly PropertyChangingEventArgs argsStatusChanging = ObservableHelper.CreateChangingArgs<AsyncScreen>(x => x.Status);

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

        protected ITaskConfiguration<TArg, TResult> CreateWorker<TArg, TResult>(string title, Func<TArg, Task<TResult>> workAction)
        {
            return CreateWorker<TArg, TResult>(title, workAction, this.Error);
        }

        protected ITaskConfiguration<TArg, TResult> CreateWorker<TArg, TResult>(string title, Func<TArg, Task<TResult>> workAction, Action<Exception> errorAction)
        {
            string taskDisplayTitle = string.Format(CultureInfo.InvariantCulture, "{0} : {1}", DisplayName, title);

            return new TaskConfiguration<TArg, TResult>(this, title, workAction, errorAction);
        }

        private class TaskConfiguration<TArg, TResult> : ITaskConfiguration<TArg, TResult>
        {
            public TaskConfiguration(AsyncScreen parent, string title, Func<TArg, Task<TResult>> worker, Action<Exception> errorAction)
            {
                Title = title;
                DoWorkAsync = async (args) =>
                    {
                        try
                        {
                            await parent.ThreadingContext.OnUIThreadAsync(() => parent.BeginBackgroudWork(title));
                            running = true;
                            return await worker(args);
                        }
                        catch (Exception ex)
                        {
                            errorAction(ex);
                        }
                        finally
                        {
                            running = false;
                            parent.ThreadingContext.OnUIThread(() => parent.EndBackgroudWork(title));
                        }

                        return await Task.FromResult(default(TResult));
                    };
            }

            public Func<TArg, Task<TResult>> DoWorkAsync
            {
                get;
                private set;
            }

            public string Title
            {
                get;
                private set;
            }

            public void Cancel()
            {
                throw new NotImplementedException();
            }

            private bool running;

            public bool IsRunning
            {
                get { return running; }
            }
        }

        private readonly HashSet<string> titles = new HashSet<string>();

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