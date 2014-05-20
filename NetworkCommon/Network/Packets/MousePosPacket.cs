using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CommonUtils.Network.Packets
{
    [Serializable]
    class MousePosPacket :Packet
    {
        Point mousePos;

        public MousePosPacket(Point mousePos)
            : base(Command.Message)
        {
            this.mousePos = mousePos;
        }
        
    }
}
