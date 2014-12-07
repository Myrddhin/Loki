using System.Windows;
using DevExpress.Xpf.Core;
using Loki.UI.Wpf.Resources;

namespace Loki.UI.Wpf
{
    public static class MessageBoxes
    {
        public static bool Confirm(string formatString, params object[] formatParams)
        {
            return DXMessageBox.Show(string.Format(formatString, formatParams), string.Empty, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK;
        }

        public static void Message(string formatString, params object[] formatParams)
        {
            DXMessageBox.Show(string.Format(formatString, formatParams), "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void Warning(string formatString, params object[] formatParams)
        {
            DXMessageBox.Show(string.Format(formatString, formatParams), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}