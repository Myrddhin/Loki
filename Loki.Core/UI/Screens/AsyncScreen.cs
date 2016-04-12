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
            BusyAfter = new TimeSpan(0);
        }

        #region IsBusy

        private readonly PropertyChangedEventArgs busyChangedEventArgs = ObservableHelper.CreateChangedArgs<AsyncScreen>(x => x.IsBusy);

        private bool busy;

        public bool IsBusy
        {
            get
            {
                return busy;
            }

            set
            {
                if (!Equals(busy, value))
                {
                    busy = value;
                    NotifyChanged(busyChangedEventArgs);
                }
            }
        }

        #endregion IsBusy

        #region Status

        private static readonly PropertyChangedEventArgs StatusChangedArgs = ObservableHelper.CreateChangedArgs<AsyncScreen>(x => x.Status);

        private static readonly PropertyChangingEventArgs StatusChangingArgs = ObservableHelper.CreateChangingArgs<AsyncScreen>(x => x.Status);

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
                    NotifyChanging(StatusChangingArgs);
                    status = value;
                    NotifyChanged(StatusChangedArgs);
                }
            }
        }

        #endregion Status

        #region BusyAfter

        private TimeSpan busyAfter;

        public TimeSpan BusyAfter
        {
            get
            {
                return busyAfter;
            }

            set
            {
                if (busyAfter != value)
                {
                    busyAfter = value;
                    NotifyChanged(BusyAfterChangedArgs);
                }
            }
        }

        private static readonly PropertyChangedEventArgs BusyAfterChangedArgs = ObservableHelper.CreateChangedArgs<AsyncScreen>(x => x.BusyAfter);

        #endregion BusyAfter

        protected override void OnInitialize()
        {
            BeginBackgroudWork(string.Empty);

            base.OnInitialize();

            this.Services.Core.Events.WatchPropertyChanged(this, this, screen => screen.IsLoaded, screen => screen.Loaded_Changed);
        }

        private void Loaded_Changed(object sender, EventArgs e)
        {
            this.EndBackgroudWork(string.Empty);
        }

        #region Background workers

        protected ITaskConfiguration<TArg, TResult> CreateWorker<TArg, TResult>(string title, Func<TArg, Task<TResult>> workAction)
        {
            return CreateWorker(title, workAction, this.Error);
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
                DoWorkAsync = async args =>
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