using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils.Network.Packets
{
    [Serializable]
    public class AuthenticationRequest : Packet
    {
        public byte[] salt;

        public AuthenticationRequest( byte[] salt )
            : base(Command.Authenticate)
        {
            this.salt = salt;
        }

    }
}
