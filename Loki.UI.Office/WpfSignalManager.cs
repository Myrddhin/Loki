using System;
using System.Windows;

using Loki.Common;

namespace Loki.UI.Office
{
    public class WpfSignalManager : BaseObject, ISignalManager
    {
        private readonly IThreadingContext threading;

        public WpfSignalManager(ILoggerComponent logManager, IErrorComponent errorManager, IThreadingContext threading) : base(logManager, errorManager)
        {
            this.threading = threading;

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
            // MessageBoxes.Message(message);
        }

        public void Warning(string warning)
        {
            //  MessageBoxes.Warning(warning);
        }

        public void Error(Exception exception, bool imperative)
        {
            LokiException ex = exception as LokiException;
            if (ex != null)
            {
                // ErrorMessageBox.Show(threading, ex, imperative);
            }
            else
            {
                //  ErrorMessageBox.Show(threading, ErrorManager.BuildError<LokiException>(ErrorMessages.Application_UnhandledException, exception), imperative);
            }

            if (imperative)
            {
                ApplicationExit(-1);
            }
        }
    }
}