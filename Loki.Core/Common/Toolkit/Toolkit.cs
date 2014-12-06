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

            common = new CoreServiceContainer(Toolkit.IoC.DefaultContext);
            ui = new UIServiceContainer(Toolkit.IoC.DefaultContext);
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

        private static List<IContextInstaller> installers = new List<IContextInstaller>(new IContextInstaller[] { ServicesInstaller.All });

        private static CoreServiceContainer common;

        private static UIServiceContainer ui;

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
            IoC.SafeDispose();

            common = null;

            Initialize();
        }

        #endregion Object Context

        /*

        private static Lazy<ICommandComponent> commands = new Lazy<ICommandComponent>(() => Context.Get<ICommandComponent>());

            private static Lazy<ISettingsComponent> settings = new Lazy<ISettingsComponent>(() => Context.Get<ISettingsComponent>());

            private static Lazy<ITaskComponent> tasks = new Lazy<ITaskComponent>(() => Context.Get<ITaskComponent>());

            /// <summary>
            /// Gets the command service.
            /// </summary>
            /// <value>The configured command service.</value>
            public static ICommandComponent Commands
            {
                get
                {
                    return commands.Value;
                }
            }*/

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

        /*

          /// <summary>
          /// Gets the settings service.
          /// </summary>
          /// <value>The configured settings service.</value>
          public static ISettingsComponent Settings
          {
              get
              {
                  return settings.Value;
              }
          }

          /// <summary>
          /// Gets the task service.
          /// </summary>
          public static ITaskComponent Tasks
          {
              get
              {
                  return tasks.Value;
              }
          }*/
    }
}