using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public delegate void NewServerEventHandler(object sender, NewServerEventArgs e);


    // hold event arguments
    public class NewServerEventArgs : System.EventArgs
    {
        private Server server;

        public NewServerEventArgs(Server server)
        {
            this.server = server;
        }

        public Server S
        {
            get
            {
                return this.server;
            }
        }
    }
}
