using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManiMofoUploader
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// The ip-address for UDP-broadcasts.
        /// </summary>
        private static IPAddress udpBroadcastIP = new IPAddress(new byte[] { 239, 255, 255, 250 });

        /// <summary>
        /// The port for UDP-broadcasts.
        /// </summary>
        private static int udpBroadcastPort = 1900;

        public MainForm()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                /* Setting the appearance of the window to the one of a Tool-Window.
                 * This hides the window from the Task-Manager all process-viewers. */
                createParams.ExStyle |= 0x80;
                return createParams;
            }
        }

        /// <summary>
        /// Ocurrs when the mainform finished loading.
        /// Starts looking for UPNP-Broadcasts.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains no event data.</param>
        private async void MainForm_Load(object sender, EventArgs e)
        {
            UdpClient udpClient = new UdpClient();
            /* Listen to packets sent to any ipaddress on port 1900 */
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 1900));
            /* Joining the default UDP broadcast group */
            udpClient.JoinMulticastGroup(udpBroadcastIP);

            while (true)
            {
                UdpReceiveResult udpReceiveResult = await udpClient.ReceiveAsync();

                if (udpReceiveResult.Buffer.Length > 0)
                {
                    string result = Encoding.UTF8.GetString(udpReceiveResult.Buffer);

                    if (result.ToUpper().Contains("MTPNULLSERVICE") && result.ToUpper().Contains("LOCATION"))
                    {
                        Match match = Regex.Match(result, ".*LOCATION: (.*?)\\r?$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        WebClient webClient = new WebClient();
                        string deviceInfo = await webClient.DownloadStringTaskAsync(match.Groups[1].Value);

                        if (deviceInfo.Contains("Sony Corporation"))
                        {
                            Process.Start(new ProcessStartInfo("bash", $"-c \"gphoto2 -P --port ptpip:{udpReceiveResult.RemoteEndPoint.Address}\""));
                        }
                    }
                }
            }
        }
    }
}
