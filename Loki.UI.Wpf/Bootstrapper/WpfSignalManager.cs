using System;
using System.Windows;

using Loki.Common;
using Loki.UI.Wpf.Resources;

namespace Loki.UI.Wpf
{
    public class WpfSignalManager : BaseObject, ISignalManager
    {
        private readonly IThreadingContext threading;

        private readonly IWindowManager windows;

        public WpfSignalManager(ILoggerComponent logManager, IErrorComponent errorManager, IThreadingContext threading, IWindowManager windows) : base(logManager, errorManager)
        {
            this.threading = threading;
            this.windows = windows;

            // if (Application.Current != null)
            // {
            // Application.Current.DispatcherUnhandledException += (s, e) => OnLastChanceError(new ExceptionEventArgs(e.Exception));
            // }
        }

        public void ApplicationExit(int errorCode)
        {
            if (Application.Current != null)
            {
                Application.Current.Shutdown(errorCode);
            }
            else
            {
                Environment.Exit(errorCode);
            }
        }

        public void Message(string message)
        {
            windows.Message(message);
        }

        public void Warning(string warning)
        {
            windows.Warning(warning);
        }

        public void Error(Exception exception, bool imperative)
        {
            LokiException ex = exception as LokiException;
            if (ex != null)
            {
                ErrorMessageBox.Show(threading, ex, imperative);
            }
            else
            {
                ErrorMessageBox.Show(threading, ErrorManager.BuildError<LokiException>(ErrorMessages.Application_UnhandledException, exception), imperative);
            }

            if (imperative)
            {
                ApplicationExit(-1);
            }
        }
    }
}