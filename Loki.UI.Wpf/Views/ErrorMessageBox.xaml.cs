using System;
using System.Windows;
using DevExpress.Xpf.Core;
using Loki.Common;
using Loki.UI.Wpf.Resources;

namespace Loki.UI.Wpf
{
    /// <summary>
    /// Interaction logic for ErrorMessageBox.xaml.
    /// </summary>
    public partial class ErrorMessageBox : DXWindow
    {
        public ErrorMessageBox()
        {
            InitializeComponent();
        }

        public static void Show(Exception P_Exception, bool P_Imperative)
        {
            ErrorMessageBox L_Dialog = new ErrorMessageBox();

            L_Dialog.Title = "Erreur";
            L_Dialog.LBL_Message.Text = P_Exception.Message;
            if (string.IsNullOrEmpty(P_Exception.StackTrace) && P_Exception.InnerException != null)
            {
                L_Dialog.RTX_Message.Text = P_Exception.InnerException.ToString();
            }
            else
            {
                L_Dialog.RTX_Message.Text = P_Exception.ToString();
            }

            if (!P_Imperative)
            {
                Toolkit.UI.Threading.BeginOnUIThread(() => { L_Dialog.ShowDialog(); });
            }
            else
            {
                Toolkit.UI.Threading.OnUIThread(() => { L_Dialog.ShowDialog(); });
            }
        }

        private void BTN_Copy_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Clipboard.SetText(RTX_Message.Text);
        }

        private void BTN_OK_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}