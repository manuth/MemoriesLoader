using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    static class Program
    {
        /// <summary>
        /// The ip-address for UPNP-broadcasts.
        /// </summary>
        private static IPAddress upnpBroadcastIP = new IPAddress(new byte[] { 239, 255, 255, 250 });

        /// <summary>
        /// The port for UPNP-broadcasts.
        /// </summary>
        private static int upnpBroadcastPort = 1900;

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (UdpClient udpClient = new UdpClient()
            {
                EnableBroadcast = true
            })
            {
                /* Listen to packets sent to any ipaddress on port 1900 */
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, upnpBroadcastPort));
                /* Joining the default UDP broadcast group */
                udpClient.JoinMulticastGroup(upnpBroadcastIP);
                udpClient.DropMulticastGroup(upnpBroadcastIP);
                udpClient.Close();
            }
        }
    }
}
