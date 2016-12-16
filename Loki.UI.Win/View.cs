using System;
using System.Windows.Forms;

namespace Loki.UI.Win
{
    public static class View
    {
        public static T GetViewModel<T>(this Control control)
            where T : class
        {
            var buffer = control;
            while (buffer != null)
            {
                if (buffer.Tag != null)
                {
                    return buffer.Tag as T;
                }

                buffer = buffer.Parent;
            }

            return null;
        }

        public static void ExecuteOnFirstLoad(this Control control, Action action)
        {
            LayoutEventHandler layoutUpdated = null;
            layoutUpdated = (s, e) =>
            {
                control.Layout -= layoutUpdated;
                action?.Invoke();
            };

            control.Layout += layoutUpdated;
        }

        public static void ProtectedCall(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}