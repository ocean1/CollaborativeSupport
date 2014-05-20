using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils.Network.Packets
{
    [Serializable]
    public class UserList : Packet, IEnumerable<string>
    {
        public string[] usernames;

        public UserList(string[] usernames):base(Command.List)
        {
            this.usernames = usernames;
        }



        public IEnumerator<string> GetEnumerator()
        {
            return usernames.AsEnumerable<string>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return usernames.AsEnumerable<string>().GetEnumerator();
        }
    }

}
