using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using CommonUtils.Network;
using System.Threading;


/* TODO: quando ci sono errori sul socket o altro, se necessario, chiude la connessione e elimina il client dalla lista
    log -> *il cliente ha abbandonato la stanza* */
namespace CommonUtils
{
    public partial class MainForm : Form
    {

        private delegate void AddListBoxItemDelegate(string s);
        private delegate void RemoveListBoxItemDelegate(string s);
        public delegate void AddTextBoxItemDelegate(string s);

        protected ConnectionManager cm;

        private ConcurrentDictionary<Socket, PeerStatus> clientList = new ConcurrentDictionary<Socket, PeerStatus>(); //Registered users

        private ArrayList clipDataList;

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(ConnectionManager cm)
            : this()
        {
            this.cm = cm;
            clipDataList = new ArrayList();
        }

        #region delegate functions
        public void AddListBoxItem(string s)
        {
            if (this.UsersListBox.InvokeRequired)
            {
                // This is a worker thread so delegate the task.
                this.UsersListBox.BeginInvoke(new AddListBoxItemDelegate(this.AddListBoxItem), s);
            }
            else
            {
                // This is the UI thread so perform the task.
                this.UsersListBox.Items.Add(s);
            }
        }

        public void RemoveListBoxItem(string s)
        {
            if (this.UsersListBox.InvokeRequired)
            {
                // This is a worker thread so delegate the task.
                this.UsersListBox.BeginInvoke(new RemoveListBoxItemDelegate(this.RemoveListBoxItem), s);
            }
            else
            {
                // This is the UI thread so perform the task.
                UsersListBox.Items.Remove(s);
            }
        }

        public void AddTextBoxItem(string s)
        {
            if (this.chatTxtBox.InvokeRequired)
            {
                // This is a worker thread so delegate the task.
                this.chatTxtBox.BeginInvoke(new AddTextBoxItemDelegate(this.AddTextBoxItem), s);
            }
            else
            {
                // This is the UI thread so perform the task.
                this.chatTxtBox.AppendText(s + "\n");
            }
        }

        private void AddClipData(string username, string description, object data, string format)
        {
            if (clipDataListView.InvokeRequired)
            {
                clipDataListView.BeginInvoke(new ConnectionManager.ShowClipDataDelegate(this.AddClipData), username, description, data, format);
            }
            else
            {
                clipDataListView.Items.Add(username);
                int row = clipDataListView.Items.Count - 1;
                clipDataListView.Items[row].SubItems.Add(description);
                clipDataListView.Items[row].SubItems.Add(format);
                clipDataList.Add(data);
            }
        }
        #endregion

        // catch "enter" key on the text box
        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals(Convert.ToChar(13)))
            {
                GetMessageAndSend();
                e.Handled = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (cm != null)
            {
                cm.printError = this.AddTextBoxItem;
                cm.printMessage = this.AddTextBoxItem;
                cm.addUserToList = this.AddListBoxItem;
                cm.removeUserFromList = this.RemoveListBoxItem;
                cm.showClipData = this.AddClipData;
                cm.Begin();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            GetMessageAndSend();
        }

        private void GetMessageAndSend()
        {
            string msg = txtMessage.Text;
            if (msg != null && msg.Length > 0)
            {
                cm.SendMessage(msg);
                txtMessage.Clear();
            }
        }

        private void clipboardMonitor1_ClipboardChange(object sender, ClipboardMonitor.ClipboardChangeEventArgs e)
        {
            // controllo ciclico sulla clipboard per verificare che sia presente del testo per visualizzarlo
            string cliptxt;

            if (System.Windows.Forms.Clipboard.ContainsText())
            {
                cliptxt = "Text"; //System.Windows.Forms.Clipboard.GetText();
                clipSendBtn.Enabled = true;
            }
            else
            {
                if (System.Windows.Forms.Clipboard.ContainsAudio())
                {
                    cliptxt = "Audio";
                    clipSendBtn.Enabled = true;
                }
                else if (System.Windows.Forms.Clipboard.ContainsFileDropList())
                {
                    cliptxt = "File Drop List";
                    clipSendBtn.Enabled = false;
                }
                else if (System.Windows.Forms.Clipboard.ContainsImage())
                {
                    cliptxt = "Image";
                    clipSendBtn.Enabled = true;
                }
                else
                {
                    cliptxt = "Other";
                    //clipSendBtn.Enabled = false;
                    clipSendBtn.Enabled = true;
                }

            }
            clipTxtBox.Text = "Clipboard contains: " + cliptxt;

        }

        private void clipSendBtn_Click(object sender, EventArgs e)
        {
            using (ChooseFormatDlg clipDlg = new ChooseFormatDlg(System.Windows.Forms.Clipboard.GetDataObject()))
            {
                if (clipDlg.ShowDialog() == DialogResult.OK)
                    cm.SendClipData(clipDlg.description, clipDlg.dataObject, clipDlg.format);
            }

        }

        private void clipDataList_DoubleClick(object sender, EventArgs e)
        {
            int row = clipDataListView.SelectedIndices[0];
            object data = clipDataList[row];
            string format = clipDataListView.SelectedItems[0].SubItems[2].Text;

            Clipboard.SetData(format, data);

        }

    }
}
