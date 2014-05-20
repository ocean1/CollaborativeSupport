using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils.Network.Packets
{

    [Serializable]
    public class AuthenticationResponse : Packet
    {
        public string username
        {
            get;
            private set;
        }

        public byte[] passwordHash
        {
            get;
            private set;
        }

        public AuthenticationResponse(string username, byte[] pwdHash) : base( Command.Authenticate )
        {
            this.username = username;
            this.passwordHash = pwdHash;
        }

    }
}
