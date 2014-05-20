namespace CommonUtils.Network
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public class InvalidPacketException : Exception
    {
        public InvalidPacketException(string reason, Exception inner)
            : base(reason, inner)
        {
        }

        public InvalidPacketException(string reason)
            : base(reason)
        {
        }
    }

}
