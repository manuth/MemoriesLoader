using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesLoader
{
    /// <summary>
    /// Represents a camera.
    /// </summary>
    [DataContract]
    public class Camera
    {
        /// <summary>
        /// Gets or sets the IP-Address of the camera.
        /// </summary>
        public IPAddress IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the output-directory of the camera.
        /// </summary>
        [DataMember(Name = "directory")]
        public string Directory { get; set; }

        /// <summary>
        /// Gets or sets the IP-Address of the camera as a string.
        /// </summary>
        [DataMember(Name = "ipAddress")]
        private string IPAddressString
        {
            get
            {
                return IPAddress.ToString();
            }

            set
            {
                IPAddress = IPAddress.Parse(value);
            }
        }
    }
}
