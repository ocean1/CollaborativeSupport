using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using CommonUtils.Configuration;
using Logger;

namespace Server
{
    public partial class NewSession : Form
    {
        public event NewServerEventHandler NewServerEvent;

        //private Server s;
        private MyLogger logger;
        private ServerOptions serverOptions;

        public NewSession()
        {
            InitializeComponent();
            logger = MyLogger.Instance;

            //try
            //{
                //ServerOptions.LoadFromFile(out serverOptions, ServerOptions.defaultConfigFile);
            serverOptions = ServerOptions.Instance;
            this.checkBox1.Checked = serverOptions.rimemberForNextSession;
            this.txtUsername.Text = serverOptions.username;
            this.portTxtBox.Text = serverOptions.port.ToString();
            this.pwdTxtBox.Text = serverOptions.password;
            /*}
            catch (OptionException s_ex)
            {
                logger.AddError(s_ex, logLevel.ERROR);
                serverOptions = new ServerOptions();
            }*/
        }

        private void StartServerBtn_Click(object sender, EventArgs e)
        {
            StartServer();
        }


        private void StartServer()
        {
            try
            {
                this.groupBox2.Enabled = false;
                this.StartServerBtn.Enabled = false;
                this.connectionProgressBar.Visible = true;

                #region initialize screen
                /*
                 ScreenHost.ScreenObject remoteObject = new ScreenHost.ScreenObject();
                remoteObject.CreatWind();
                TcpChannel channel = new TcpChannel(8082);
                ChannelServices.RegisterChannel(channel, false);
                RemotingServices.Marshal(remoteObject, "castScreen");
                /************************************* TCP *************************************/
                #endregion

                Server server = new Server(int.Parse(portTxtBox.Text), "Admin", pwdTxtBox.Text,serverOptions.maxConnections);
                //s.AddUserToList = msg => { MessageBox.Show(this, msg); };
                server.printError = err => MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // msg => { MessageBox.Show(this, msg); }; // assign delegates

                NewServerEventArgs valueArgs = new NewServerEventArgs(server);
                NewServerEvent(this, valueArgs);

                #region commented out
                //server.StartServer(); // finally start the server

                /*if (this.checkBox1.Checked == true)
                {
                    try
                    {
                        serverOptions.port = int.Parse(this.portTxtBox.Text);
                        serverOptions.password = pwdTxtBox.Text;
                        //serverOptions.SaveToFile(ServerOptions.defaultConfigFile);
                    }
                    catch (Exception ex)
                    {
                        logger.AddError(ex, logLevel.ERROR);
                        MessageBox.Show(this, "Error saving configuration to file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (File.Exists(ServerOptions.defaultConfigFile))
                    {
                        try{
                            File.Delete(ServerOptions.defaultConfigFile);
                        }catch(Exception ex){
                            logger.AddError(ex, logLevel.ERROR);
                            MessageBox.Show(this, "Error deleting configuration file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }*/
                #endregion

                serverOptions.username = this.txtUsername.Text;
                serverOptions.port = int.Parse(this.portTxtBox.Text);
                serverOptions.password = pwdTxtBox.Text;
                serverOptions.rimemberForNextSession = this.checkBox1.Checked;

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.groupBox2.Enabled = true;
                this.StartServerBtn.Enabled = true;
                this.connectionProgressBar.Visible = false;
            }
        }

        private void KeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals(Convert.ToChar(13)))
            {
                StartServer();
            }
        }

    }
}
