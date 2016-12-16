﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

using Loki.Common.Resources;

namespace Loki.Common
{
    public class ExtensionManager<TComponent> : IDisposable
    {
        private readonly string defaultComponentName;

        private readonly string searchDirectory;

        private readonly AggregateCatalog catalog;

        private CompositionContainer container;

        public ExtensionManager(string defaultComponentName)
        {
            this.defaultComponentName = defaultComponentName;

            this.searchDirectory = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;

            // An aggregate catalog that combines multiple catalogs
            catalog = new AggregateCatalog();
        }

        public TComponent SelectedComponent
        {
            get;
            set;
        }

        [ImportMany]
        private IEnumerable<Lazy<TComponent, IExtensionMetadata>> AvailableComponents { get; set; }

        public void Initialize()
        {
            // Adds all the parts found in the same appdomain path
            catalog.Catalogs.Add(new DirectoryCatalog(searchDirectory, "Loki*.dll"));

            // Create the CompositionContainer with the parts in the catalog
            container = new CompositionContainer(catalog);

            // Fill the imports of this object
            try
            {
                container.ComposeParts(this);

                // Assign component
                if (AvailableComponents.Count() == 1)
                {
                    SelectedComponent = AvailableComponents.First().Value;
                }
                else if (AvailableComponents.Count() > 1)
                {
                    // Multiple components found ; select default.
                    var extension = AvailableComponents.First(x => x.Metadata.Type == defaultComponentName);
                    if (extension != null)
                    {
                        SelectedComponent = extension.Value;
                    }
                }

                if (SelectedComponent == null)
                {
                    Trace.Fail(string.Format(CultureInfo.InvariantCulture, Errors.Utils_ExtensionManager_Component, typeof(TComponent), searchDirectory));
                }
            }
            catch (ReflectionTypeLoadException loadException)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(loadException.ToString());
                foreach (var loadFail in loadException.LoaderExceptions)
                {
                    builder.AppendLine(loadFail.ToString());
                }

                Trace.Fail(builder.ToString());
            }
            catch (CompositionException compositionException)
            {
                Trace.Fail(compositionException.ToString());
            }
            catch (TypeInitializationException initException)
            {
                Trace.Fail(initException.ToString());
            }
        }

        #region IDisposable

        private bool disposed;

        ~ExtensionManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (!disposing)
            {
                return;
            }

            this.catalog.Dispose();
            this.container?.Dispose();

            this.disposed = true;
        }

        #endregion IDisposable
    }
}