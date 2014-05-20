using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace Server
{
    class MouseHook : IDisposable
    {

        private NativeMethods.LowLevelMouseProc _proc;
        private IntPtr _hookID;
        public IntPtr hWnd;

        //private readonly object _lock = new object();

        private bool _disposed;
        public ManualResetEventSlim mre;

        public MouseHook()
        {
            this._hookID = IntPtr.Zero;
            this._proc = HookCallback;
            this.mre = new ManualResetEventSlim();
        }

        public IntPtr SetHook()
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                this._hookID = NativeMethods.SetWindowsHookEx(NativeMethods.WH_MOUSE_LL, this._proc, NativeMethods.GetModuleHandle(curModule.ModuleName), 0);
                return this._hookID;
            }
        }

        /// <summary>
        /// This is the mouse hook callback, will read the current mouse position
        /// if the message intercepted is WM_LBUTTONDOWN get the window handle (if any)
        /// and signal to the FoundHWND event that the window handle has been found
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && NativeMethods.MouseMessages.WM_LBUTTONDOWN == (NativeMethods.MouseMessages)wParam)
            {
                NativeMethods.MSLLHOOKSTRUCT hookStruct = (NativeMethods.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeMethods.MSLLHOOKSTRUCT));

                hWnd = NativeMethods.WindowFromPoint(hookStruct.pt);

                while (true)
                {
                    IntPtr h2 = NativeMethods.GetParent(hWnd);
                    if (h2 == null || h2 == IntPtr.Zero) break;
                    hWnd = h2;
                }

                if (hWnd != null)
                {
                    mre.Set();
                }

                return (System.IntPtr)1; //don't pass the message to other hooks to avoid loosing focus!
            }
            return NativeMethods.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        /// <summary>
        /// Delete our installed hook from the hook chain
        /// </summary>
        public void Unhook()
        {
            if (!_disposed && this._hookID != IntPtr.Zero) // make sure the hook is installed and object has not been disposed before unhooking
            {
                if (NativeMethods.UnhookWindowsHookEx(this._hookID) == false)
                    MessageBox.Show("UnhookWindowsHookEx failed");  // this should better be a call to the error logger
                this._hookID = IntPtr.Zero;
            }
        }

        #region IDisposable implementation
        public void Dispose()
        {
            Dispose(true); // dispose managed and unmanaged rosources alike!
            GC.SuppressFinalize(this); // avoid to execute finalization of this class from happening two times
        }

        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing">tells if the method has been called directly (true)
        /// or indirectly by the runtime (false)</param>
        private void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    // clean up managed resources here
                    mre.Dispose();
                }

                // clean up unmanaged resources here
                Unhook();
            }
            _disposed = true;
        }

        ~MouseHook()
        {
            Dispose(false);
        }
        #endregion

    }
}
