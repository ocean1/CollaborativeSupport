using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Client;
using Logger;
using System.IO;
using CommonUtils.Configuration;

namespace Client
{
    static class Program
    {

        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ClientOptions clientOptions = ClientOptions.Instance;

            FileErrorLogger errorLogger = null;
            if (clientOptions.logErrorEnable)
            {
                errorLogger = new FileErrorLogger(clientOptions.errorLogFile);
                if (!errorLogger.Error)
                {
                    MyLogger.Instance.RegisterObserver(errorLogger);
                }
                else
                {
                    errorLogger.Dispose();
                    errorLogger = null;
                }
                MyLogger.Enabled = true;
            }

            /*FileMessageLogger messageLogger;
            if (serverOptions.logMessageEnable)
            {
                messageLogger = new FileMessageLogger(serverOptions.messageLogFile);
                MyLogger.Instance.RegisterObserver(messageLogger);
            }*/

            Client c = null;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (LoginForm loginFrm = new LoginForm())
            {

                // catch the event of new client created and obtain Client object instance
                loginFrm.NewClientEvent += new NewClientEventHandler((object sender, NewClientEventArgs e) => { c = e.C; });
                Application.Run(loginFrm);
            }

            // if connection has been succesfull create chat form window
            if (c != null)
            {
                // strange error happens here when closing form lol!
                Application.Run(new MainForm(c));
            }

            clientOptions.Dispose();
        }

    }
}
