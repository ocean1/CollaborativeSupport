using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils.Network.Packets
{
    class BigPacket:Packet
    {
        public int length;

        public BigPacket(int length): base ( Command.BigPacket )
        {
            this.length = length;
        }
    }
}
