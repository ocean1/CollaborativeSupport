using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CommonUtils
{
    class NativeMethods
    {
        [DllImport("user32.dll")]
        internal static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetClipboardViewer();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    }
}
