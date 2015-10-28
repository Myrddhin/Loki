using System;
using System.Windows;
using Loki.Common;
using Loki.UI.Wpf.Resources;

namespace Loki.UI.Wpf
{
    public class WpfSignalManager : BaseObject, ISignalManager
    {
        public WpfSignalManager(ILoggerComponent logManager, IErrorComponent errorManager) : base(logManager, errorManager)
        {
            //if (Application.Current != null)
            //{
            //    Application.Current.DispatcherUnhandledException += (s, e) => OnLastChanceError(new ExceptionEventArgs(e.Exception));
            //}
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
            MessageBoxes.Message(message);
        }

        public void Warning(string warning)
        {
            MessageBoxes.Warning(warning);
        }

        public void Error(Exception exception, bool imperative)
        {
            LokiException ex = exception as LokiException;
            if (ex != null)
            {
                ErrorMessageBox.Show(ex, imperative);
            }
            else
            {
                ErrorMessageBox.Show(ErrorManager.BuildError<LokiException>(ErrorMessages.Application_UnhandledException, exception), imperative);
            }

            if (imperative)
            {
                ApplicationExit(-1);
            }
        }
    }
}