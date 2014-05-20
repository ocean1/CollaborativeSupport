using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CommonUtils.Network.Packets;


namespace CommonUtils.Network
{

    public class PacketComposer
    {

        public const int byteLengthSize = sizeof(int);

        public static byte[] Serialize(Packet packet, bool includeLength)
        {
            byte[] p;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();

                if (includeLength)
                    ms.Write(BitConverter.GetBytes((int)0),0,byteLengthSize);

                bf.Serialize(ms, packet);

                if (includeLength)
                {
                    int len = (int)ms.Position - byteLengthSize; // the position in the ms equals the total size
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Write(BitConverter.GetBytes(len), 0, byteLengthSize);
                }

                p = ms.ToArray();
            }
            return p;
        }

        public static Packet Deserialize(byte[] buffer)
        {
            return Deserialize(buffer, 0);
        }

        public static Packet Deserialize(byte[] buffer, int position)
        {
            Packet p;
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                ms.Seek(position, SeekOrigin.Begin);
                BinaryFormatter bf = new BinaryFormatter();
                p = (Packet)bf.Deserialize(ms);
            }
            return p;
        }
    }
}
