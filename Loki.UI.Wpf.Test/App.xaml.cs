using System.Windows;
using Loki.Common;

namespace Loki.UI.Wpf.Test
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DevExpress.Xpf.Core.ThemeManager.ApplicationThemeName = "DXStyle";
        }
    }
}