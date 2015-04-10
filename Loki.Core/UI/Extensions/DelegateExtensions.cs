using System.Threading.Tasks;
using Loki.Common;

namespace System
{
    public static class DelegateExtensions
    {
        /// <summary>
        ///   Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public static void BeginOnUIThread(this Action action)
        {
            if (Toolkit.UI.Threading != null)
            {
                Toolkit.UI.Threading.BeginOnUIThread(action);
            }
            else
            {
                Task.Factory.StartNew(action);
            }
        }

        /// <summary>
        ///   Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name = "action">The action to execute.</param>
        public static Task OnUIThreadAsync(this Action action)
        {
            if (Toolkit.UI.Threading != null)
            {
                return Toolkit.UI.Threading.OnUIThreadAsync(action);
            }
            else
            {
                return Task.Factory.StartNew(action);
            }
        }

        /// <summary>
        ///   Executes the action on the UI thread.
        /// </summary>
        /// <param name = "action">The action to execute.</param>
        public static void OnUIThread(this Action action)
        {
            if (Toolkit.UI.Threading != null)
            {
                Toolkit.UI.Threading.OnUIThread(action);
            }
            else
            {
                action();
            }
        }
    }
}