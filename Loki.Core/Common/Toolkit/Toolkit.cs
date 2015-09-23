using System;
using System.Collections.Generic;
using System.Diagnostics;
using Loki.Core.Resources;
using Loki.IoC;
using Loki.UI;

namespace Loki.Common
{
    /// <summary>
    /// Loki services helper class.
    /// </summary>
    public static class Toolkit
    {
        #region Extensibility

        private const string DefaultEngine = "Windsor";
        private static ExtensionManager<IIoCComponent> engineManager;

        /// <summary>
        /// Initializes the toolkit.
        /// </summary>
        public static void Initialize()
        {
            InitializeIoC();

            common = new CoreServiceContainer(IoC.DefaultContext);
            ui = new UIServiceContainer(IoC.DefaultContext);
        }

        private static void InitializeIoC()
        {
            engineManager = new ExtensionManager<IIoCComponent>(DefaultEngine);
            engineManager.Initialize();

            if (IoC != null)
            {
                IoC.Initialize();
            }
        }

        #endregion Extensibility

        #region Object Context

        private static readonly List<IContextInstaller> installers = new List<IContextInstaller>(new[] { ServicesInstaller.All });

        private static CoreServiceContainer common;

        private static UIServiceContainer ui;

        [Obsolete]
        public static CoreServiceContainer Common
        {
            get
            {
                if (common == null)
                {
                    Trace.Fail(Errors.Utils_Toolkit_NotInitialized);
                }

                return common;
            }
        }

        [Obsolete]
        public static UIServiceContainer UI
        {
            get
            {
                if (ui == null)
                {
                    Trace.Fail(Errors.Utils_Toolkit_NotInitialized);
                }

                return ui;
            }
        }

        public static IEnumerable<IContextInstaller> Installers
        {
            get { return installers; }
        }

        /// <summary>
        /// Registers an additional installer for the toolkit.
        /// </summary>
        /// <param name="installer">The installer.</param>
        public static void RegisterInstaller(IContextInstaller installer)
        {
            installers.Add(installer);
        }

        /// <summary>
        /// Resets the toolkit.
        /// </summary>
        public static void Reset()
        {
            Shutdown();

            Initialize();
        }

        public static void Shutdown()
        {
            IoC.SafeDispose();

            common = null;
            ui = null;
        }

        #endregion Object Context

        /// <summary>
        /// Gets the engine service.
        /// </summary>
        public static IIoCComponent IoC
        {
            get
            {
                return engineManager.SelectedComponent;
            }
        }
    }
}