﻿using System;
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
using MemoriesLoader.Logging;

namespace MemoriesLoader
{
    /// <summary>
    /// Provides a hidden main-form of the application.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// The IP address for UPNP-broadcasts.
        /// </summary>
        private static IPAddress upnpBroadcastIP = new IPAddress(new byte[] { 239, 255, 255, 250 });

        /// <summary>
        /// The port for UPNP-broadcasts.
        /// </summary>
        private static int upnpBroadcastPort = 1900;

        /// <summary>
        /// The form to log to.
        /// </summary>
        private LogForm logForm = new LogForm();

        /// <summary>
        /// The logger that is used to log the procedure.
        /// </summary>
        private Logger logger = new Logger();

        /// <summary>
        /// The log-formatter to log the procedure.
        /// </summary>
        private LogFormatter formatter = new LogFormatter();

        /// <summary>
        /// The destination-path-settings of the cameras.
        /// </summary>
        private List<Camera> cameras = new List<Camera>();
        
        /// <summary>
        /// The filesystem-watchers and the paths they're assigned to.
        /// </summary>
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
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            formatter.Formatter = new Func<LogMessage, string>(
                message =>
                {
                    return $"{message.Time.TimeOfDay} {$"[{message.Level}]".PadRight(11)}\t{message.Description}";
                });
            logger.Targets.AddRange(
                new LogTarget[]
                {
                    new RichTextBoxTarget(logForm.RichTextBox)
                    {
                        Level = LogLevel.Info | LogLevel.Warning,
                        Formatter = formatter
                    },
                    new FileTarget(Properties.Settings.Default.LogFileName)
                    {
                        Level = LogLevel.All,
                        Formatter = formatter
                    }
                });
        }

        /// <summary>
        /// Gets the required creation parameters when the control handle is created.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x80;
                return createParams;
            }
        }

        /// <summary>
        /// Occurs when the main-form finished loading.
        /// Starts looking for UPNP-Broadcasts.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains no event data.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(Properties.Settings.Default.CameraSettingsPath))
            {
                cameras = (List<Camera>)new DataContractJsonSerializer(
                    typeof(List<Camera>),
                    new DataContractJsonSerializerSettings
                    {
                        UseSimpleDictionaryFormat = true
                    }).ReadObject(File.OpenRead(Properties.Settings.Default.CameraSettingsPath));

                logger.Info($"Settings found located at {Properties.Settings.Default.CameraSettingsPath}");

                string message = string.Empty;

                foreach (Camera camera in cameras)
                {
                    message += Environment.NewLine + $"{new string('\t', 5)}IPAddress: {camera.IPAddress.ToString()},\tOutput-Directory: {camera.Directory}";
                }

                logger.Debug(message);
            }
            else
            {
                logger.Warn($"No Settings found located at {Properties.Settings.Default.CameraSettingsPath}");
            }

            mainTask = Discover();
        }

        /// <summary>
        /// Starts discovering SSDP-requests.
        /// </summary>
        /// <returns>
        /// A task that represents the discover-operation.
        /// </returns>
        private async Task Discover()
        {
            CancellationToken token = tokenSource.Token;

            await Task.Run(
                async () =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        using (UdpClient udpClient = new UdpClient())
                        {
                            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                            /* Listen to packets sent to any ipaddress on port 1900 */
                            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, upnpBroadcastPort));

                            logger.Debug($"Starting to listen to {udpClient.Client.LocalEndPoint}...");

                            /* Joining the default UDP broadcast group */
                            {
                                IPAddress ipAddress = (
                                    from address in await Dns.GetHostAddressesAsync(Dns.GetHostName())
                                    where !IPAddress.IsLoopback(address)
                                    select address).First();
                                udpClient.JoinMulticastGroup(upnpBroadcastIP, ipAddress);
                                logger.Debug($"Adding {ipAddress} to Multicast-Group {upnpBroadcastIP}");
                            }

                            logger.Info("Waiting for a request performed by a device that supports the MtpNullService...");
                            logger.Warn("Waiting for a request performed by a device that supports the MtpNullService...");
                            logger.Debug("Waiting for a request performed by a device that supports the MtpNullService...");
                            logger.Info("Waiting for a request performed by a device that supports the MtpNullService...");
                            logger.Warn("Waiting for a request performed by a device that supports the MtpNullService...");
                            logger.Info("Waiting for a request performed by a device that supports the MtpNullService...");

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

                                            logger.Info($"Downloading device-description from \"{request.RequestUri}\"...");

                                            try
                                            {
                                                DeviceDescription deviceInfo = (DeviceDescription)new XmlSerializer(typeof(DeviceDescription)).Deserialize((await request.GetResponseAsync()).GetResponseStream());

                                                logger.Info("Sucessfully downloaded the device-description:" + Environment.NewLine +
                                                    $"{new string('\t', 5)}Name:\t{deviceInfo.Device.Name}" + Environment.NewLine +
                                                    $"{new string('\t', 5)}Manufacturer:\t{deviceInfo.Device.Manufacturer}" + Environment.NewLine +
                                                    $"{new string('\t', 5)}{"Website".PadLeft(12)}:\t{deviceInfo.Device.ManufacturerUri}" + Environment.NewLine +
                                                    $"{new string('\t', 5)}GUID:\t{deviceInfo.Device.GUID}");
                                                logger.Info("Checking whether it's a Sony camera...");

                                                if (deviceInfo.Device.Manufacturer == "Sony Corporation")
                                                {
                                                    string path = cameras.FirstOrDefault(camera => camera.IPAddress.Equals(udpReceiveResult.RemoteEndPoint.Address))?.Directory ?? udpReceiveResult.RemoteEndPoint.Address.ToString();

                                                    logger.Info("Success!");
                                                    logger.Info($"The files received from {udpReceiveResult.RemoteEndPoint.Address} will be saved to \"{Path.GetFullPath(path)}\"");

                                                    if (!Directory.Exists(path))
                                                    {
                                                        Directory.CreateDirectory(path);
                                                    }

                                                        Process process = Process.Start(new ProcessStartInfo("bash", $"-c \"gphoto2 -P --skip-existing --port ptpip:{udpReceiveResult.RemoteEndPoint.Address}\"")
                                                        {
                                                            WorkingDirectory = Path.GetFullPath(path),
                                                            UseShellExecute = false,
                                                            CreateNoWindow = true,
                                                            RedirectStandardOutput = true,
                                                            RedirectStandardError = true
                                                        });

                                                        logger.Debug($"Running following command: {process.StartInfo.FileName} {process.StartInfo.Arguments}");

                                                        if (!process.WaitForExit(2 * 60 * 1000)) // Wait 2 minutes for the process to exit
                                                        {
                                                            logger.Warn("gphoto2 timed out! Killing the process...");
                                                            process.Kill();
                                                        }
                                                        else
                                                        {
                                                            if (!process.StandardOutput.EndOfStream)
                                                            {
                                                                string message = "Received a console output-message from gphoto2:" + Environment.NewLine;

                                                                while (!process.StandardOutput.EndOfStream)
                                                                {
                                                                    message += new string('\t', 5) + await process.StandardOutput.ReadLineAsync() + Environment.NewLine;
                                                                }

                                                                logger.Debug(message.TrimEnd(Environment.NewLine.ToCharArray()));
                                                            }

                                                            if (!process.StandardError.EndOfStream)
                                                            {
                                                                string message = "Received a console error-message from gphoto2:" + Environment.NewLine;

                                                                while (!process.StandardError.EndOfStream)
                                                                {
                                                                    message += new string('\t', 5) + await process.StandardError.ReadLineAsync() + Environment.NewLine;
                                                                }

                                                                logger.Warn(message.TrimEnd(Environment.NewLine.ToCharArray()));
                                                            }
                                                        }
                                                }

                                                break;
                                            }
                                            catch (WebException)
                                            {
                                                logger.Warn($"Couldn't download device-description!");
                                            }
                                        }
                                    }
                                }
                            }

                            logger.Info("Cleaning up, leaving MulticastGroups and closing the udp client...");
                            udpClient.DropMulticastGroup(upnpBroadcastIP);
                            udpClient.Close();
                        }
                    }
                },
                token);
        }

        /// <summary>
        /// Safely stops the <see cref="mainTask"/> and closes the form in order to exit the application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="FormClosingEventArgs"/> that contains the event data.</param>
        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mainTask.Status != TaskStatus.RanToCompletion)
            {
                e.Cancel = true;
                tokenSource.Cancel();
                await mainTask;
                notifyIcon.Visible = false;
                Close();
            }
        }

        /// <summary>
        /// Opens the log-window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains no event data.</param>
        private void LogWindowItem_Click(object sender, EventArgs e)
        {
            logForm.Refresh();
            logForm.Show();
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains no event data.</param>
        private void ExitItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Opens up the log-file.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains no event data.</param>
        private void LogFileItem_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Settings.Default.LogFileName);
        }
    }
}