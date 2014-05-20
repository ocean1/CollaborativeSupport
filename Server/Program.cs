namespace Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using CommonUtils.Configuration;
    using Logger;
    using System.IO;


    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Server server = null;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ServerOptions serverOptions;


            serverOptions = (ServerOptions)Options<ServerOptions>.Instance;


            FileErrorLogger errorLogger = null;
            if (serverOptions.logErrorEnable)
            {
                errorLogger = new FileErrorLogger(serverOptions.errorLogFile);
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

            using (NewSession nsf = new NewSession())
            {

                // catch the event of new client created and obtain Client object instance
                nsf.NewServerEvent += new NewServerEventHandler((object sender, NewServerEventArgs e) => { server = e.S; });
                Application.Run(nsf);

            }

            // if connection has been succesfull create chat form window 
            if (server != null)
            {
                Application.Run(new MainForm(server));
            }

            if (errorLogger != null) errorLogger.Terminate();

            serverOptions.Dispose();

        }
    }
}
