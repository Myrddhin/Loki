using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using DevExpress.Xpf.Core;
using Loki.Common;

namespace Loki.UI.Wpf
{
    public class WpfWindowManager : IWindowManager
    {
        public CultureInfo Culture
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
        }

        private bool? inDesignMode = null;

        public bool DesignMode
        {
            get
            {
                if (inDesignMode == null)
                {
                    var descriptor = DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement));
                    inDesignMode = (bool)descriptor.Metadata.DefaultValue;
                }

                return inDesignMode.GetValueOrDefault(false);
            }
        }

        public bool Confirm(string message)
        {
            //return WinUIMessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
            return DXMessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        //public string GetOpenFileName(FileDialogInformations informations)
        //{
        //    var fileDialog = new Microsoft.Win32.OpenFileDialog();
        //    fileDialog.Filter = informations.Filter;
        //    fileDialog.DefaultExt = informations.DefaultExtension;
        //    fileDialog.InitialDirectory = informations.InitialDirectory;
        //    bool result = fileDialog.ShowDialog() ?? false;

        //    if (result)
        //    {
        //        return fileDialog.FileName;
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        //public string GetSaveFileName(FileDialogInformations informations)
        //{
        //    var fileDialog = new Microsoft.Win32.SaveFileDialog();
        //    fileDialog.AddExtension = true;
        //    fileDialog.Filter = informations.Filter;
        //    fileDialog.DefaultExt = informations.DefaultExtension;
        //    fileDialog.InitialDirectory = informations.InitialDirectory;
        //    bool result = fileDialog.ShowDialog() ?? false;

        //    if (result)
        //    {
        //        return fileDialog.FileName;
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        public bool? ShowAsPopup(object viewModel)
        {
            var template = Toolkit.UI.Templating.GetTemplate(viewModel);

            DXWindow templateAsWindow = template as DXWindow;
            if (templateAsWindow == null)
            {
                templateAsWindow = new DXWindow();
                templateAsWindow.ShowIcon = false;
                templateAsWindow.ShowInTaskbar = false;
                templateAsWindow.BorderEffect = BorderEffect.Default;
                templateAsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                templateAsWindow.ShowTitle = false;
                templateAsWindow.Padding = new Thickness() { Bottom = 0, Top = 0 };
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
                Toolkit.UI.Templating.CreateBind(templateAsWindow, viewModel);
            }

            var screen = viewModel as IScreen;
            if (screen != null)
            {
                screen.DialogResultSetter = (r) => { if (templateAsWindow.DialogResult != r) { templateAsWindow.DialogResult = r; } };
            }

            return templateAsWindow.ShowDialog();
        }
    }
}