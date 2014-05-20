using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonUtils.Network.Packets
{
    [Serializable]
    public class ClipboardPacket : Packet
    {
        public string username;
        public object clipBoardData;
        public string description;
        public string format;

        public ClipboardPacket(string username, string description, object clipboardData, string format):base(Command.Clipboard)
        {
            this.clipBoardData = clipboardData;
            this.description = description;
            this.format = format;
            this.username = username;
        }

    }
}
