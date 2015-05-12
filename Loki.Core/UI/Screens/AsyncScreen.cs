﻿using System;
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

        protected ITaskConfiguration<TArg, TResult> CreateWorker<TArg, TResult>(string title, Func<TArg, Task<TResult>> workAction, Action<TResult> callbackAction)
        {
            return CreateWorker<TArg, TResult>(title, workAction, callbackAction, this.Error);
        }

        protected ITaskConfiguration<TArg, TResult> CreateWorker<TArg, TResult>(string title, Func<TArg, Task<TResult>> workAction, Action<TResult> callbackAction, Action<Exception> errorAction)
        {
            string taskDisplayTitle = string.Format(CultureInfo.InvariantCulture, "{0} : {1}", DisplayName, title);
            Func<TArg, Task<TResult>> runner = async (args) =>
                {
                    await Toolkit.UI.Threading.OnUIThreadAsync(() => BeginBackgroudWork(title));
                    return await workAction(args);
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
                            await Toolkit.UI.Threading.OnUIThreadAsync(() => parent.BeginBackgroudWork(title));
                            return await worker(args);
                        }
                        catch (Exception ex)
                        {
                            errorAction(ex);
                        }
                        finally
                        {
                            Toolkit.UI.Threading.OnUIThread(() => parent.EndBackgroudWork(title));
                        }

                        return await Task.FromResult(default(TResult));
                    };
            }

            private Func<TArg, Task<TResult>> worker;

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