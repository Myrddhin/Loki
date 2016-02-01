using System;
using System.Runtime.InteropServices;

namespace Loki.UI.Office
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal static class NativeMethods
    {
        [DllImport("gdi32.dll")]
        internal static extern void DeleteObject(IntPtr handle);
    }
}