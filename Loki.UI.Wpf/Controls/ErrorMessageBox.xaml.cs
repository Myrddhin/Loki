using System;
using System.Windows;

namespace Loki.UI.Wpf
{
    /// <summary>
    /// Interaction logic for ErrorMessageBox.xaml.
    /// </summary>
    public partial class ErrorMessageBox
    {
        public ErrorMessageBox()
        {
            InitializeComponent();
        }

        public static void Show(IThreadingContext threading, Exception excep, bool imperative)
        {
            ErrorMessageBox dialog = new ErrorMessageBox();

            dialog.Title = "Erreur";
            dialog.LBL_Message.Text = excep.Message;
            if (string.IsNullOrEmpty(excep.StackTrace) && excep.InnerException != null)
            {
                dialog.RTX_Message.Text = excep.InnerException.ToString();
            }
            else
            {
                dialog.RTX_Message.Text = excep.ToString();
            }

            if (!imperative)
            {
                threading.BeginOnUIThread(() => { dialog.ShowDialog(); });
            }
            else
            {
                threading.OnUIThread(() => { dialog.ShowDialog(); });
            }
        }

        private void BTN_Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(RTX_Message.Text);
        }

        private void BTN_OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}