using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public delegate void NewClientEventHandler(object sender, NewClientEventArgs e);


    // hold event arguments
    public class NewClientEventArgs : System.EventArgs
    {
        private Client c;

        public NewClientEventArgs(Client c)
        {
            this.c = c;
        }

        public Client C
        {
            get
            {
                return this.c;
            }
        }
    }
}
