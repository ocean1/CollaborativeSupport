using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CommonUtils.Network
{
    public class NetworkUtils
    {
        public static  IPEndPoint ParseEndPoint(string endpointstring)
        {
            string[] values = endpointstring.Split(new char[] { ':' });

            if (2 > values.Length)
            {
                throw new FormatException("Invalid endpoint format");
            }

            IPAddress ipAddress;
            string ipString = string.Join(":", values.Take(values.Length - 1).ToArray());
            if (!IPAddress.TryParse(ipString, out ipAddress))
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(ipString);
                bool ipv4address_found = false;
                foreach (IPAddress ip in ipHostInfo.AddressList)
                {
                    /* assumes that the first IPv4 address is good and working
                     * don't try with other IP addresses */
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = ip;
                        ipv4address_found = true;
                        break;
                    }
                }

                if (ipv4address_found == false)
                    throw new Exception(string.Format("Can't find an IPv4 address for {0}", ipString));

            }

            int port;
            if (!int.TryParse(values[values.Length - 1], out port)
             || port < IPEndPoint.MinPort
             || port > IPEndPoint.MaxPort)
            {
                throw new FormatException(string.Format("Invalid end point port '{0}'", values[values.Length - 1]));
            }

            return new IPEndPoint(ipAddress, port);
        }
    }
}
