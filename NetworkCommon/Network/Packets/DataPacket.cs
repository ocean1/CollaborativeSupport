namespace CommonUtils.Network.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public class DataPacket : Packet
    {
        //public string strName;      //Name by which the client logs into the room
        public string Message;   //Message text
        public string username;

        //  default constructor
        public DataPacket() : base()
        {
            this.username = null;
            this.Message = null;
        }

        public DataPacket(Command cmd, string message)
            : base(cmd)
        {
            this.username = null;
            this.Message = message;
        }

        public DataPacket(Command cmd, string name, string message)
            : base(cmd)
        {
            this.username = name;
            this.Message = message;
        }
        
        public override string ToString() {
            StringBuilder tmp = new StringBuilder(String.Empty);
            if (this.username != null)
            {
                tmp.Append(this.username + ": ");
            }
            if (this.Message != null)
            {
                tmp.Append(this.Message);
            }

            if (tmp.Equals(String.Empty)) return "Empty packet\n";

            tmp.Append("\n");

            return tmp.ToString();
        }

    }

}
