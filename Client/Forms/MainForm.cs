using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CommonUtils;
using CommonUtils.Network.Packets;

namespace Client
{
    public partial class MainForm : CommonUtils.MainForm
    {
        private delegate void eventdelegate(object sender, EventArgs e);

        private ShowScreen showScreen;
        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(Client c):base(c)
        {
            InitializeComponent();
            c.PeerDisconnectedEvent += new EventHandler(c_PeerDisconnectedEvent);
            showScreen = new ShowScreen();
            showScreen.Show();
            showScreen.BringToFront();
            c.showVideo = showScreen.ShowVideo;
        }

        void c_PeerDisconnectedEvent(object sender, EventArgs e)
        {

            if (this.InvokeRequired )
            {
                this.BeginInvoke(new eventdelegate(c_PeerDisconnectedEvent),sender,e);
            }
            else
            {
                // disable all controls
                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }
                showScreen.Close();
            }
            
        }

    }
}
