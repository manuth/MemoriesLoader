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
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MemoriesLoader
{
    public partial class MainForm : Form
    {
        private List<Camera> cameras = new List<Camera>();
        
        private Dictionary<string, FileSystemWatcher> fileSystemWatchers = new Dictionary<string, FileSystemWatcher>();

        /// <summary>
        /// The main task.
        /// </summary>
        private Task mainTask;

        /// <summary>
        /// The cancellation-token.
        /// </summary>
        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        /// <summary>
        /// The ip-address for UPNP-broadcasts.
        /// </summary>
        private static IPAddress upnpBroadcastIP = new IPAddress(new byte[] { 239, 255, 255, 250 });

        /// <summary>
        /// The port for UPNP-broadcasts.
        /// </summary>
        private static int upnpBroadcastPort = 1900;

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Ocurrs when the mainform finished loading.
        /// Starts looking for UPNP-Broadcasts.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains no event data.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (File.Exists("Cameras.json"))
            {
                cameras = (List<Camera>)(new DataContractJsonSerializer(typeof(List<Camera>), new DataContractJsonSerializerSettings
                {
                    UseSimpleDictionaryFormat = true
                }).ReadObject(File.OpenRead("Cameras.json")));
            }

            mainTask = Discover();
        }

        private async Task Discover()
        {
            CancellationToken token = tokenSource.Token;

            await Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    using (UdpClient udpClient = new UdpClient())
                    {
                        udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        /* Listen to packets sent to any ipaddress on port 1900 */
                        udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, upnpBroadcastPort));
                        /* Joining the default UDP broadcast group */
                        foreach (IPAddress address in Dns.GetHostAddresses(Dns.GetHostName()))
                        {
                            if (!IPAddress.IsLoopback(address) && address.AddressFamily != AddressFamily.InterNetworkV6)
                            {
                                udpClient.JoinMulticastGroup(upnpBroadcastIP, address);
                                break;
                            }
                        }

                        udpClient.MulticastLoopback = true;

                        while (!token.IsCancellationRequested)
                        {
                            if (udpClient.Available > 0)
                            {
                                UdpReceiveResult udpReceiveResult = await udpClient.ReceiveAsync();

                                if (udpReceiveResult.Buffer.Length > 0)
                                {
                                    string result = Encoding.UTF8.GetString(udpReceiveResult.Buffer);

                                    if (result.ToUpper().Contains("MTPNULLSERVICE") && result.ToUpper().Contains("LOCATION"))
                                    {
                                        Match match = Regex.Match(result, ".*LOCATION: (.*?)\\r?$", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                                        WebRequest request = WebRequest.Create(match.Groups[1].Value);
                                        request.Timeout = 4;

                                        try
                                        {
                                            DeviceDescription deviceInfo = (DeviceDescription)new XmlSerializer(typeof(DeviceDescription)).Deserialize((await request.GetResponseAsync()).GetResponseStream());

                                            if (deviceInfo.Device.Manufacturer == "Sony Corporation")
                                            {
                                                string path = cameras.FirstOrDefault(camera => camera.IPAddress.Equals(udpReceiveResult.RemoteEndPoint.Address))?.Directory ?? udpReceiveResult.RemoteEndPoint.Address.ToString();

                                                if (!Directory.Exists(path))
                                                {
                                                    Directory.CreateDirectory(path);
                                                }

                                                Process process = Process.Start(new ProcessStartInfo("bash", $"-c \"gphoto2 -P --skip-existing --port ptpip:{udpReceiveResult.RemoteEndPoint.Address}\"")
                                                {
                                                    WorkingDirectory = Path.GetFullPath(path)
                                                });

                                                if (!process.WaitForExit(2 * 60 * 1000)) // Wait 2 minutes for the process to exit
                                                {
                                                    process.Kill();
                                                }
                                            }

                                            break;
                                        }
                                        catch
                                        {
                                            // TODO
                                        }
                                    }
                                }
                            }
                        }
                        udpClient.DropMulticastGroup(upnpBroadcastIP);
                        udpClient.Close();
                    }
                }
            }, token);
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mainTask.Status != TaskStatus.RanToCompletion)
            {
                e.Cancel = true;
                tokenSource.Cancel();
                await mainTask;
                Close();
            }
        }
    }
}
