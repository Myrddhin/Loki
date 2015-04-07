using System;
using System.Windows.Forms;
using Loki.Common;

namespace Loki.UI.Win.Test
{
    public static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        public static void Main(params string[] args)
        {
            Application.SetCompatibleTextRenderingDefault(false);

            // If using EnableVisualStyles, always call DoEvents immediately afterwards. Failing to
            // do so can result in an unexpected SEHException.
            Application.EnableVisualStyles();
            Application.DoEvents();

            new TestBootStrapper().Run(args);
        }
    }
}