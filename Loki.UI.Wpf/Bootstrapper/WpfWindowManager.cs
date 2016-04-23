using System.Globalization;
using System.Threading;
using System.Windows;

namespace Loki.UI.Wpf
{
    public class WpfWindowManager : IWindowManager
    {
        private readonly ITemplatingEngine engine;

        public WpfWindowManager(ITemplatingEngine engine)
        {
            this.engine = engine;
        }

        public CultureInfo Culture
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
        }

        public bool DesignMode
        {
            get
            {
                return View.DesignMode;
            }
        }

        public bool Confirm(string message)
        {
            return MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        public void Message(string message)
        {
            MessageBox.Show(message);
        }

        public void Warning(string message)
        {
            MessageBox.Show(message, string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public string GetOpenFileName(FileDialogInformations informations)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.Filter = informations.Filter;
            fileDialog.DefaultExt = informations.DefaultExtension;
            fileDialog.InitialDirectory = informations.InitialDirectory;
            bool result = fileDialog.ShowDialog() ?? false;

            if (result)
            {
                return fileDialog.FileName;
            }

            return string.Empty;
        }

        public string GetSaveFileName(FileDialogInformations informations)
        {
            var fileDialog = new Microsoft.Win32.SaveFileDialog();
            fileDialog.AddExtension = true;
            fileDialog.Filter = informations.Filter;
            fileDialog.DefaultExt = informations.DefaultExtension;
            fileDialog.InitialDirectory = informations.InitialDirectory;
            bool result = fileDialog.ShowDialog() ?? false;

            if (result)
            {
                return fileDialog.FileName;
            }

            return string.Empty;
        }

        public bool? ShowAsPopup(object viewModel)
        {
            var template = engine.GetTemplate(viewModel);

            Window templateAsWindow = template as Window;
            if (templateAsWindow == null)
            {
                templateAsWindow = new Window();
                templateAsWindow.ShowInTaskbar = false;
                templateAsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                templateAsWindow.Padding = new Thickness { Bottom = 0, Top = 0 };
                templateAsWindow.MinWidth = 100;
                templateAsWindow.MinHeight = 100;
                templateAsWindow.ResizeMode = ResizeMode.NoResize;
                templateAsWindow.WindowStyle = WindowStyle.None;
                templateAsWindow.SizeToContent = SizeToContent.WidthAndHeight;
                templateAsWindow.Owner = Application.Current.MainWindow;

                /*var dobject = template as FrameworkElement;
                if (dobject != null)
                {
                    templateAsWindow.Width = dobject.Width;
                    templateAsWindow.Height = dobject.Height;
                }*/
                View.SetModel(templateAsWindow, viewModel);
                engine.CreateBind(templateAsWindow, viewModel);
            }

            var screen = viewModel as IScreen;
            if (screen != null)
            {
                screen.DialogResultSetter = r =>
                    {
                        if (templateAsWindow.DialogResult != r)
                        {
                            templateAsWindow.DialogResult = r;
                        }
                    };
            }

            return templateAsWindow.ShowDialog();
        }
    }
}