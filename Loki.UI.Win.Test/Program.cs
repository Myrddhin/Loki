using System;
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
            new TestBootStrapper().Run(args);
        }
    }
}