using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using Loki.Common;

using Microsoft.Office.Core;

using stdole;

namespace Loki.UI.Office
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ComVisible(true)]
    public class LokiRibbon : IRibbonExtensibility
    {
        public string RibbonXmlResource { get; set; }

        private IRibbonUI ribbon;

        private OfficeShell model;

        public TModel GetModel<TModel>() where TModel : OfficeShell
        {
            return model as TModel;
        }

        public void PublishMessage<TMessage>(TMessage message)
        {
            if (model != null)
            {
                model.Bus.PublishOnUIThread(model.ThreadingContext, message);
            }
        }

        public void PublishMessage<TMessage>() where TMessage : new()
        {
            PublishMessage(new TMessage());
        }

        public void SetModel<TModel>(TModel value) where TModel : OfficeShell
        {
            model = value;
            RefreshLayout();
        }

        public string DisplayName
        {
            get
            {
                var ribbonModel = GetModel<OfficeShell>();
                if (ribbonModel != null)
                {
                    return ribbonModel.DisplayName;
                }

                return "No name";
            }
        }

        public string GetMainLabel(IRibbonControl control)
        {
            return DisplayName;
        }

        public string GetTabLabel(IRibbonControl control)
        {
            return DisplayName;
        }

        public void Ribbon_Load(IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;

            RefreshLayout();
        }

        protected virtual Bitmap GetImage(string name)
        {
            return null;
        }

        public IPictureDisp GetImage(IRibbonControl control)
        {
            var image = GetImage(control.Id);
            if (image != null)
            {
                return new PictureDispImpl(image);
            }

            return null;
        }

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText(RibbonXmlResource);
        }

        private void RefreshLayout()
        {
            if (ribbon != null)
            {
                ribbon.Invalidate();
            }
        }

        #region Helpers

        private string GetResourceText(string resourceName)
        {
            Assembly asm = GetType().Assembly;
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }

            return null;
        }

        #endregion Helpers
    }
}