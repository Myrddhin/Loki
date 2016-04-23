using System;

using Loki.IoC.Registration;

using Microsoft.Office.Tools;

using Interop = Microsoft.Office.Interop.Word;

namespace Loki.UI.Office.Tests
{
    public class WordPlatform<TMainModel> : OfficePlatform<TMainModel>
         where TMainModel : OfficeShell
    {
        private Interop.Application application;

        public WordPlatform(AddInBase addinInstance, LokiRibbon ribbon, Interop.Application excel)
            : base(addinInstance, ribbon)
        {
            CommonSetup(excel);
            MainObjectBinder = model => addinInstance.Tag = model;
        }

        private void CommonSetup(Interop.Application word)
        {
            if (word != null && !View.DesignMode)
            {
                application = word;

                // register adapter singleton.
                IWordAdapter adapter = new WordAdapter(word);
                Context.Register(Element.For<IWordAdapter>().Instance(adapter));

                BootStrapper.Initialized += BootStrapper_Initialized;
            }
        }

        private void BootStrapper_Initialized(object sender, EventArgs e)
        {
            //// substibe to powerpoint events.
            // application.WorkbookActivate += Application_WorkbookActivate;
            // application.WorkbookOpen += Application_WorkbookOpen;
            // application.SheetSelectionChange += Application_SheetSelectionChange;
            // application.WorkbookBeforeClose += Application_WorkbookBeforeClose;
            // ((Interop.AppEvents_Event)application).NewWorkbook += Application_NewWorkbook;
            // ((Interop.AppEvents_Event)application).WorkbookDeactivate += Application_WorkbookDeactivate;
        }

        // private void Application_SheetSelectionChange(object sh, Interop.Range target)
        // {
        // MessageBus.PublishOnUIThread(new SelectionChangeMessage());
        // }

        // private void Application_WorkbookDeactivate(Interop.Workbook wb)
        // {
        // MessageBus.PublishOnUIThread(new WorkbookChangeMessage());
        // }

        // private void Application_NewWorkbook(Interop.Workbook wb)
        // {
        // MessageBus.PublishOnUIThread(new WorkbookChangeMessage());
        // }

        // private void Application_WorkbookBeforeClose(Interop.Workbook wb, ref bool cancel)
        // {
        // MessageBus.PublishOnUIThread(new WorkbookChangeMessage());
        // }

        // private void Application_WorkbookOpen(Interop.Workbook wb)
        // {
        // MessageBus.PublishOnUIThread(new WorkbookChangeMessage());
        // }

        // private void Application_WorkbookActivate(Interop.Workbook wb)
        // {
        // MessageBus.PublishOnUIThread(new WorkbookChangeMessage());
        // }
    }
}