﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using Loki.Common;
using Loki.Common.Diagnostics;

namespace Loki.UI.Win
{
    public class WinformWindowManager : BaseObject, IWindowManager
    {
        public WinformWindowManager(IDiagnostics logger) : base(logger)
        {
        }

        public virtual CultureInfo Culture
        {
            get { return Application.CurrentCulture; }
        }

        public ITemplatingEngine Templates { get; set; }

        public bool? ShowAsPopup(object screen)
        {
            var template = Templates.GetTemplate(screen) as Form;
            if (template == null)
            {
                Log.ErrorFormat("Unable to find template for viewmodel {0}", screen);
                return null;
            }
            else
            {
                return template.ShowDialog() == DialogResult.OK;
            }
        }

        public bool DesignMode
        {
            get
            {
                bool designMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;

                if (!designMode)
                {
                    using (var process = Process.GetCurrentProcess())
                    {
                        return process.ProcessName.ToLowerInvariant().Contains("devenv");
                    }
                }

                return designMode;
            }
        }

        public string GetOpenFileName(FileDialogInformations informations)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = informations.Filter;
            fileDialog.DefaultExt = informations.DefaultExtension;
            fileDialog.InitialDirectory = informations.InitialDirectory;
            bool dialogResult = fileDialog.ShowDialog() == DialogResult.OK;

            if (dialogResult)
            {
                return fileDialog.FileName;
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetSaveFileName(FileDialogInformations informations)
        {
            var fileDialog = new SaveFileDialog();
            fileDialog.Filter = informations.Filter;
            fileDialog.AddExtension = true;
            fileDialog.DefaultExt = informations.DefaultExtension;
            fileDialog.InitialDirectory = informations.InitialDirectory;
            bool dialogResult = fileDialog.ShowDialog() == DialogResult.OK;

            if (dialogResult)
            {
                return fileDialog.FileName;
            }
            else
            {
                return string.Empty;
            }
        }

        public bool Confirm(string message)
        {
            throw new NotImplementedException();
           // return XtraMessageBox.Show(message, "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        public void Message(string message)
        {
        }

        public void Warning(string message)
        {
        }
    }
}