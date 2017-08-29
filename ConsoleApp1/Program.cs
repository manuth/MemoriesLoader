using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (UdpClient udpClient = new UdpClient())
            {
                /* Listen to packets sent to any ipaddress on port 1900 */
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 1900));
                /* Joining the default UDP broadcast group */
                udpClient.JoinMulticastGroup(IPAddress.Parse("239.255.255.250"));
                udpClient.DropMulticastGroup(IPAddress.Parse("239.255.255.250"));
            }
        }
    }
}
