using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loki.IoC;

namespace Loki.UI
{
    public class UIServiceContainer
    {
        public UIServiceContainer(IObjectCreator context)
        {
            windows = new LazyResolver<IWindowManager>(context);
            threading = new LazyResolver<IThreadingContext>(context);
            templating = new LazyResolver<ITemplatingEngine>(context);
            signals = new LazyResolver<ISignalManager>(context);
        }

        private Lazy<IWindowManager> windows;

        private Lazy<IThreadingContext> threading;

        private Lazy<ITemplatingEngine> templating;

        private Lazy<ISignalManager> signals;

        /// <summary>
        /// Gets the windows manager.
        /// </summary>
        public IWindowManager Windows
        {
            get
            {
                return windows.Value;
            }
        }

        /// <summary>
        /// Gets the threading manager.
        /// </summary>
        public IThreadingContext Threading
        {
            get
            {
                return threading.Value;
            }
        }

        /// <summary>
        /// Gets the templating engine.
        /// </summary>
        public ITemplatingEngine Templating
        {
            get
            {
                return templating.Value;
            }
        }

        /// <summary>
        /// Gets the signals manager.
        /// </summary>
        public ISignalManager Signals
        {
            get
            {
                return signals.Value;
            }
        }
    }
}