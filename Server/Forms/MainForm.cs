using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CommonUtils;
using CommonUtils.Network.Packets;
using Logger;


namespace Server
{
    public partial class MainForm : CommonUtils.MainForm
    {

        public Rectangle AreaSelected; /* l'area selezionata*/
        //private VideoManager videoManager;

        private Server server;

        public MainForm(Server s)
            : base(s)
        {
            InitializeComponent();
            this.panel4.Height -= 55;
            this.Height += 55;
            this.server = s;
            server.videoManager = null;

            this.shortcutKeyTxt.Text = ServerOptions.Instance.enableVideoSendKeyString;

            //add the admin username
            this.AddListBoxItem(ServerOptions.Instance.username);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            // the result tells us if we can process a
            // new video screenshot or the worker threads
            // are still on work 
            if (server.videoManager.TrySendVideo() == false)
            {
                MyLogger.Instance.AddError("Lost a video frame! i.e increasing timer interval!", logLevel.WARNING);
                if (timer1.Interval < 200) timer1.Interval += 5; // increase interval at most 200 ms
                //remember we should reset timer interval when changing target to send because the new area to analyze could be smaller!
            }

            timer1.Start();

        }

        #region Button Click Handlers
        private void Select_Area_Click(object sender, EventArgs e)
        {
            SelectAreaBtn.BackColor = SystemColors.ActiveCaption;
            SelectWindowBtn.BackColor = SystemColors.Control;

            using (SelectArea area_form = new SelectArea(this))
            {
                timer1.Stop();
                area_form.ShowDialog();
                if (server.videoManager != null) server.videoManager.Dispose();
                server.videoManager = new VideoManager(IntPtr.Zero, AreaSelected);
                server.videoManager.WindowClosedEvent += new EventHandler(videoManager_WindowClosedEvent);
                server.videoManager.SendPacket += packet => server.SendAllClients(packet, null); // show some lambda functions love
                timer1.Start();
            }

            checkBox1.Enabled = true;
        }

        private void SelectWindowBtn_Click(object sender, EventArgs e)
        {
            SelectWindowBtn.BackColor = SystemColors.ActiveCaption;
            SelectAreaBtn.BackColor = SystemColors.Control;

            using (MouseHook mh = new MouseHook())
            {
                timer1.Stop();
                // couldn't use an event because it get's processed in the same thread, and we can't unhook
                mh.SetHook();
                mh.mre.Wait(); // wait until the hWnd is found
                mh.Unhook();

                if (server.videoManager != null) server.videoManager.Dispose();
                server.videoManager = new VideoManager(mh.hWnd);
                server.videoManager.WindowClosedEvent += new EventHandler(videoManager_WindowClosedEvent);
                server.videoManager.SendPacket += packet => server.SendAllClients(packet, null); // show some lambda functions love
                timer1.Start();
            }
            checkBox1.Enabled = true;
        }

        void videoManager_WindowClosedEvent(object sender, EventArgs e)
        {
            timer1.Stop();
        }
        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                SelectAreaBtn.Enabled = true;
                SelectWindowBtn.Enabled = true;

                // if there's an active instance of videoManager
                // start the timer
                if (server.videoManager != null)
                {
                    timer1.Interval = 50;
                    timer1.Start();
                }

            }
            else
            {
                // Disable buttons
                timer1.Stop();

                SelectAreaBtn.Enabled = false;
                SelectWindowBtn.Enabled = false;

                // we should also tell the client to hide the ShowScreen form!

            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (ServerOptions.Instance.EnabledVideoSendKey == keyData)
            {
                checkBox1.Checked = !checkBox1.Checked;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void shortcutKeyTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control || e.Alt) &&
                (e.KeyCode != Keys.Menu) &&
                (e.KeyCode != Keys.ControlKey) &&
                (e.KeyCode != Keys.Apps))
            {
                // it's a valid combination with at least one modifier
                // between Alt and Control

                shortcutKeyTxt.Text = e.KeyData.ToString();
                ServerOptions.Instance.EnabledVideoSendKey = e.KeyData;
            }

            e.Handled = true;

        }

    }
}
