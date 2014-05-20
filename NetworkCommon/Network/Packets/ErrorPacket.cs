namespace CommonUtils.Network.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public enum CollaborationError
    {
        UserNameTaken,  //user name already taken
        MaxUsersReached,       //max number of users on server reached
        AuthFailed,      //Authentication Failed
        None,
        SocketError
    }

    [Serializable]
    public class ErrorPacket : Packet
    {
        public CollaborationError error
        {
            get;
            private set;
        }

        private static string[] errors = new string[]{
            "User name already taken!\n",
            "Max. number of users reached!\n",
            "Authentication Failed\n",
            "None",
            "Generic Socket Error"
        };

        public ErrorPacket(CollaborationError ce)
            : base(Command.Error)
        {
            //TODO: aggiungi assert per le varie condizioni di ce
            this.error = ce;
        }

        public static string getErrorString(CollaborationError ce)
        {
            string error = errors[(int)ce];
            return error;
        }

        public override string ToString()
        {
            return errors[(int)this.error];
        }
    }
}
