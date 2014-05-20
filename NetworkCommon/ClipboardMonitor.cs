using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace CommonUtils
{
    /// <summary>
    /// Clipboard monitor control a control that monitors the clipboard
    /// for changes and notify issuing an event
    /// Control is inherited instead of component because
    /// we need a window proc to own clipboard
    /// </summary>
    [Description("Clipboard Monitor"),
    DefaultProperty("OnClipboardChange"),
    DefaultEvent("ClipboardChange"),
    ToolboxItemFilter("System.Windows.Forms"),]
    public partial class ClipboardMonitor : Control
    {

        private EventHandler<ClipboardChangeEventArgs> onClipboardChange;
        private IntPtr nextClipboardViewer;

        public ClipboardMonitor()
        {
            InitializeComponent();
            nextClipboardViewer = NativeMethods.SetClipboardViewer(this.Handle);
        }

        public ClipboardMonitor(IContainer container)
            : this()
        {
            container.Add(this);
        }

        [Category("Default"),
        Description("The event handler when new data is present in the clipboard")]
        public event EventHandler<ClipboardChangeEventArgs> ClipboardChange
        {
            add
            {
                onClipboardChange += value;
            }
            remove
            {
                onClipboardChange -= value;
            }
        }

        protected override void WndProc(ref System.Windows.Forms.Message message)
        {
            // defined in winuser.h
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;

            switch (message.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    ClipboardChanged();
                    NativeMethods.SendMessage(nextClipboardViewer, message.Msg, message.WParam, message.LParam);
                    break;

                case WM_CHANGECBCHAIN:
                    if (message.WParam == nextClipboardViewer)
                        nextClipboardViewer = message.LParam;
                    else
                        NativeMethods.SendMessage(nextClipboardViewer, message.Msg, message.WParam, message.LParam);
                    break;

                default:
                    base.WndProc(ref message);
                    break;
            }
        }

        void ClipboardChanged()
        {
            try
            {
                IDataObject iData = System.Windows.Forms.Clipboard.GetDataObject();
                if (onClipboardChange != null)
                {
                    onClipboardChange(this, new ClipboardChangeEventArgs(iData));
                }

            }
            catch (Exception)
            {
                // Swallow or pop-up, not sure
                // Trace.Write(e.ToString());
                //MessageBox.Show(e.ToString());
            }
        }

        public class ClipboardChangeEventArgs : EventArgs
        {
            public readonly IDataObject DataObject;

            public ClipboardChangeEventArgs(IDataObject dataObject)
            {
                DataObject = dataObject;
            }
        }

    }
}
