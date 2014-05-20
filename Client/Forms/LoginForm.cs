using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Client
{
    public partial class LoginForm : Form
    {
        public event NewClientEventHandler NewClientEvent;
        delegate void TestDelegate(string message);

        private Client client;
        private ClientOptions clientOptions;

        public LoginForm()
        {
            InitializeComponent();
            clientOptions = ClientOptions.Instance;

            this.txtServerUri.Text = clientOptions.serverURI;
            this.txtUsername.Text = clientOptions.username;
            this.txtPassword.Text = clientOptions.password;
            this.checkBox1.Checked = clientOptions.rimemberForNextSession;

            //this.DialogResult = DialogResult.Abort;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            StartClient();
            //TODO substitute MessageBox with a textBox/label shown on login windows
            //c.printError = s => { MessageBox.Show(s); }; /* lambda function, the preferred way to add inline code according to msdn */

            //try to connect
        }

        #region keypress event to catch "enter" and start server
        private void multiple_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals(Convert.ToChar(13)))
            {
                //TODO here call the sendmessage!
                StartClient();
            }
        }
        #endregion

        private void ShowMessage(string s)
        {
            if (this.InvokeRequired)
            {
                // using begininvoke because in this case Invoke will lock the client
                this.BeginInvoke(new Client.PrintErrorDelegate(this.ShowMessage), s);
            }
            else
            {
                MessageBox.Show(this, s, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartClient()
        {
            this.groupBox1.Enabled = false;
            this.btnConnect.Enabled = false;
            this.connectionProgressBar.Visible = true;

            try
            {
                client = new Client(
                    this.txtServerUri.Text,
                    this.txtUsername.Text,
                    this.txtPassword.Text,
                    ShowMessage
                );

                client.printMessage += s => MessageBox.Show(s);

                if (client.Connect() == true)
                {
                    NewClientEvent(this, new NewClientEventArgs(this.client));

                    // don't close until the connection has been done right
                    // close this dialog and open the messaging one
                    // or form is closed if closed->close the whole app
                    // ChatForm cf = new ChatForm(c);
                    // cf.Show();

                    clientOptions.serverURI = this.txtServerUri.Text;
                    clientOptions.username = this.txtUsername.Text;
                    clientOptions.password = this.txtPassword.Text;
                    clientOptions.rimemberForNextSession = this.checkBox1.Checked;

                    this.Close();
                }

            }
            finally
            {
                this.groupBox1.Enabled = true;
                this.btnConnect.Enabled = true;
                this.connectionProgressBar.Visible = false;
            }
        }

    }
}
