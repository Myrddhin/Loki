using System;

using Microsoft.Office.Core;

namespace Loki.UI.Office.Tests
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            // Configure installers
            var bootstrapper = new WordPlatform<MainViewModel>(this, ribbon.Value, this.Application);

            bootstrapper.Context.Initialize(TestRegistration.Test);

            // Run
            bootstrapper.Start();
        }

        private readonly Lazy<Ribbon> ribbon = new Lazy<Ribbon>();

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
        }

        protected override IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return ribbon.Value;
        }

        #region Code généré par VSTO

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += ThisAddIn_Startup;
            this.Shutdown += ThisAddIn_Shutdown;
        }

        #endregion Code généré par VSTO
    }
}